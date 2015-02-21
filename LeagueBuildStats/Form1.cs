using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RiotSharp;
using RiotSharp.StaticDataEndpoint;
using LeagueBuildStats.Forms;
using System.Runtime.InteropServices;
using LeagueBuildStats.Classes.Items;
using LeagueBuildStats;
using LeagueBuildStats.Classes;
using System.Runtime.Serialization.Formatters.Binary;
using DevExpress.XtraEditors;
using LeagueBuildStats.UserControls;
using DevExpress.XtraBars;
using DevExpress.XtraRichEdit;
using LeagueBuildStats.UserControls.MainTopBar;
using System.Timers;
using System.Reflection;


namespace LeagueBuildStats
{
	public partial class Form1 : Form , IMessageFilter
	{
		public GetAllVersionAvailable getAllVersionAvailable = new GetAllVersionAvailable();

		public ItemsTab itemsTab;
		public ChampionsTab championsTab;
		public RunesTab runesTab;
		public MasteriesTab masteriesTab;
		private StatsTab statsTab;
		public MainTopBar mainTopBar;

		private PictureBox picboxCursor = new PictureBox();

		private DateTime LogTimeStart = DateTime.Now;


		#region GenericMethodsThatEffectOverallFunction

		private const int WM_HSCROLL = 0x114;
		private const int WM_VSCROLL = 0x115;
		//Fixes fuzzy scrolling slightly
		protected override void WndProc(ref Message m)
		{
			if ((m.Msg == WM_HSCROLL || m.Msg == WM_VSCROLL)
				&& (((int)m.WParam & 0xFFFF) == 5))
			{
				// Change SB_THUMBTRACK to SB_THUMBPOSITION
				m.WParam = (IntPtr)(((int)m.WParam & ~0xFFFF)
						   | 4);
			}
			base.WndProc(ref m);
		}

		//This is part of the requirments to make you scroll what you have mouse over
		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == 0x20a)
			{
				// WM_MOUSEWHEEL, find the control at screen position m.LParam
				Point pos = new Point(m.LParam.ToInt32() & 0xffff, m.LParam.ToInt32() >> 16);
				IntPtr hWnd = WindowFromPoint(pos);
				if (hWnd != IntPtr.Zero && hWnd != m.HWnd && Control.FromHandle(hWnd) != null)
				{
					SendMessage(hWnd, m.Msg, m.WParam, m.LParam);
					return true;
				}
			}
			return false;
		}
		// P/Invoke declarations
		[DllImport("user32.dll")]
		private static extern IntPtr WindowFromPoint(Point pt);
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);


		/// <summary>
		/// This helps to reduce flickering while scrolling controls on screen
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x02000000;
				return cp;
			}
		}

		#endregion



		public Form1()
		{
			SplashForm.ShowSplashScreen();

			
			InitializeComponent();


			string assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			this.Text = "League Build Stats " + assemblyVersion + " - For League of Legends (LoL)";

			this.BackColor = panelCtlMainTopBar.BackColor;

			if (CreateFolders())
			{
				LogWriter writer = LogWriter.Instance;
				writer.WriteToLog("Start");
				LoadControls();

				bool success = LoadRiotDataFromFile();

				if (!success) {
					SplashForm.ChangeToDownloading();
					success = InitializeDataCollection();
				}
				SplashForm.ChangeToLoading();
				if (success)
				{
					writer = LogWriter.Instance;
					writer.WriteToLog("Execute UpdateFormWithData()");
					UpdateFormWithData();
				}
				else
				{
					MessageBox.Show("Failed to load or download data!");
				}
				LogWriter writer2 = LogWriter.Instance;
				string elapsed = (DateTime.Now - LogTimeStart).ToString(@"hh\:mm\:ss\.fff");
				writer2.WriteToLog("Finish (time elapsed = " + elapsed + ")");
			}


			xtraTabControl.Selecting+=xtraTabControl_Selecting;


			//This is part of the requirments to make you scroll what you mouse over
			Application.AddMessageFilter(this);

			this.DoubleBuffered = true;
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

			SplashForm.CloseForm();
			//this.TopMost = true;
			//this.Focus();
			//this.BringToFront();
			//this.TopMost = false;
			this.Activate();
			//this.ShowDialog();

			//This timer is used to show a dragging image
			//0.002 second
			_timer1.Interval = 2;
			_timer1.Tick+=_timer1_Tick;
			_timer1.Start();
		}


		System.Windows.Forms.Timer _timer1 = new System.Windows.Forms.Timer();

		//This show a dragging image
		private void _timer1_Tick(object sender, EventArgs e)
		{
			if (itemsTab.dragger.isDragging)
			{
				picBoxCursor2.Visible = true;
				picBoxCursor2.Image = itemsTab.dragger.ItemImage();
				var relPoint = this.PointToClient(Cursor.Position);
				picBoxCursor2.Location = new Point(relPoint.X + 10, relPoint.Y + 10);
			}
			else if (championsTab.dragger.isDragging)
			{
				picBoxCursor2.Visible = true;
				picBoxCursor2.Image = championsTab.dragger.ItemImage();
				var relPoint = this.PointToClient(Cursor.Position);
				picBoxCursor2.Location = new Point(relPoint.X + 10, relPoint.Y + 10);
			}
			else
			{
				picBoxCursor2.Visible = false;
			}
		}


		private void xtraTabControl_Selecting(object sender, DevExpress.XtraTab.TabPageCancelEventArgs e)
		{
			if (e.PageIndex == 4)
			{
				statsTab.UpdateStatsTab();
			}
		}



		private bool CreateFolders()
		{
			bool success = false;
			try
			{
				List<string> paths = new List<string>();
				paths.Add(string.Format(@"{0}\Data\Champions\Images", PublicStaticVariables.thisAppDataDir));
				paths.Add(string.Format(@"{0}\Data\Items\Images", PublicStaticVariables.thisAppDataDir));
				paths.Add(string.Format(@"{0}\Data\Masteries\Images", PublicStaticVariables.thisAppDataDir));
				paths.Add(string.Format(@"{0}\Data\Runes\Images", PublicStaticVariables.thisAppDataDir));
				paths.Add(string.Format(@"{0}\Data\Logs", PublicStaticVariables.thisAppDataDir)); 
				foreach (string p in paths)
				{
					if (!Directory.Exists(p))
					{
						Directory.CreateDirectory(p);
					}
				
				}
				success = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			return success;
		}

		private void LoadControls()
		{

			itemsTab = new ItemsTab(this);
			itemsTab.Dock = DockStyle.Fill;
			Items.Controls.Add(itemsTab);

			championsTab = new ChampionsTab(this);
			championsTab.Dock = DockStyle.Fill;
			Champions.Controls.Add(championsTab);

			runesTab = new RunesTab(this);
			runesTab.Dock = DockStyle.Fill;
			Runes.Controls.Add(runesTab);

			masteriesTab = new MasteriesTab(this);
			masteriesTab.Dock = DockStyle.Fill;
			Masteries.Controls.Add(masteriesTab);

			statsTab = new StatsTab(this);
			statsTab.Dock = DockStyle.Fill;
			Stats.Controls.Add(statsTab);

			mainTopBar = new MainTopBar(this);
			mainTopBar.Dock = DockStyle.Fill;
			panelCtlMainTopBar.Controls.Add(mainTopBar);
		}

		private bool LoadRiotDataFromFile()
		{
			bool success = true;
			LogWriter writer = LogWriter.Instance;
			writer.WriteToLog("Execute LoadRiotVersionData()");

			//Always try to check what the latest versions are online
			success = getAllVersionAvailable.CollectVersionData();
			if (!success)
			{
				success = getAllVersionAvailable.LoadRiotVersionData();
			}

			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute LoadRiotItemData()");
				success = itemsTab.getItemsFromServer.LoadRiotItemData(getAllVersionAvailable.realm.V);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute LoadRiotChampionData()");
				success = championsTab.getChampionsFromServer.LoadRiotChampionData(getAllVersionAvailable.realm.V);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute LoadRiotRuneData()");
				success = runesTab.getRunesFromServer.LoadRiotRuneData(getAllVersionAvailable.realm.V);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute LoadRiotMasteryData()");
				success = masteriesTab.getMasteriesFromServer.LoadRiotMasteryData(getAllVersionAvailable.realm.V);
			}
			return success;
		}

		public bool LoadRiotDataFromFile(string version)
		{
			bool success = true;
			LogWriter writer = LogWriter.Instance;
			writer.WriteToLog("Execute LoadRiotVersionData()");

			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute LoadRiotItemData()");
				success = itemsTab.getItemsFromServer.LoadRiotItemData(version);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute LoadRiotChampionData()");
				success = championsTab.getChampionsFromServer.LoadRiotChampionData(version);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute LoadRiotRuneData()");
				success = runesTab.getRunesFromServer.LoadRiotRuneData(version);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute LoadRiotMasteryData()");
				success = masteriesTab.getMasteriesFromServer.LoadRiotMasteryData(version);
			}
			return success;
		}



		private bool InitializeDataCollection()
		{
			LogWriter writer = LogWriter.Instance;
			writer.WriteToLog("Execute CollectVersionData()");
			bool success = getAllVersionAvailable.CollectVersionData();
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute CollectItemData()");
				success = itemsTab.CollectItemData(getAllVersionAvailable.realm.V);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute CollectChampionData()");
				success = championsTab.CollectChampionData(getAllVersionAvailable.realm.V);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute CollectRuneData()");
				success = runesTab.CollectRuneData(getAllVersionAvailable.realm.V);
			}
			if (success)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute CollectMasteryData()");
				success = masteriesTab.CollectMasteryData(getAllVersionAvailable.realm.V);
			}
			return success;
		}


		public bool DataCollection(string version)
		{
			bool success = true;
			if (success)
			{
				LogWriter writer = LogWriter.Instance;
				writer.WriteToLog("Execute CollectItemData(version)");
				success = itemsTab.CollectItemData(version);
			}
			if (success)
			{
				LogWriter writer = LogWriter.Instance;
				writer.WriteToLog("Execute CollectChampionData(version)");
				success = championsTab.CollectChampionData(version);
			}
			if (success)
			{
				LogWriter writer = LogWriter.Instance;
				writer.WriteToLog("Execute CollectRuneData(version)");
				success = runesTab.CollectRuneData(version);
			}
			if (success)
			{
				LogWriter writer = LogWriter.Instance;
				writer.WriteToLog("Execute CollectMasteryData(version)");
				success = masteriesTab.CollectMasteryData(version);
			}
			return success;
		}

		public void UpdateFormWithData(bool updateVersionDropDown = true)
		{
			LogWriter writer = LogWriter.Instance;
			writer.WriteToLog("Execute UpdateItemControl()");
			itemsTab.UpdateItemControl();

			writer = LogWriter.Instance;
			writer.WriteToLog("Execute UpdateChampionControl()");
			championsTab.UpdateChampionControl();

			writer = LogWriter.Instance;
			writer.WriteToLog("Execute UpdateRuneControl()");
			runesTab.UpdateRuneControl();

			writer = LogWriter.Instance;
			writer.WriteToLog("Execute UpdateRuneControl()");
			masteriesTab.UpdateRuneControl();

			if (updateVersionDropDown)
			{
				writer = LogWriter.Instance;
				writer.WriteToLog("Execute UpdateMainTopBar()");
				mainTopBar.UpdateMainTopBarVersions();

				
				
			}
		}


	}
}

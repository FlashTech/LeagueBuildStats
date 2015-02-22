using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LeagueBuildStats.Classes;
using System.IO;
using RiotSharp.StaticDataEndpoint;

namespace LeagueBuildStats.UserControls
{
	public partial class ChampionControl : UserControl
	{
		public KeyValuePair<string, ChampionStatic> champ;
		Form1 frm1;

		public Image ChampImage
		{
			set { picBoxChampion.Image = value; }
		}

		public string ChampLabel
		{
			get { return lblChamp.Text; }
			set { lblChamp.Text = value; }
		}

		public ChampionControl(Form1 frm1, KeyValuePair<string, ChampionStatic> champ)
			: this()
		{
			this.frm1 = frm1;
			this.champ = champ;
		}

		public ChampionControl()
		{
			InitializeComponent();

			picBoxChampion.MouseDown += picBoxChampion_MouseDown;
			picBoxChampion.MouseMove += picBoxChampion_MouseMove;
			picBoxChampion.MouseUp += picBoxChampion_MouseUp;
			picBoxChampion.MouseDoubleClick += picBoxChampion_MouseDoubleClick;
			picBoxChampion.MouseClick += picBoxChampion_MouseClick;

		}


		void picBoxChampion_MouseClick(object sender, MouseEventArgs e)
		{
			frm1.championsTab.championControl_MouseClick(champ.Value);
		}

		void picBoxChampion_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			//Generate PicBox
			KeyValuePair<string, ChampionStatic> championPreped = champ;
			PictureBox championPicBox = new PictureBox();
			championPicBox.Size = new Size(48, 48);
			championPicBox.Location = new Point(0, 0);
			string file = string.Format(@"{0}\Data\Champions\Images\{1}\{2}", PublicStaticVariables.thisAppDataDir, frm1.championsTab.getChampionsFromServer.version, championPreped.Value.Image.Sprite);
			Image imageItem = Image.FromFile(file);
			Image image = CommonMethods.cropImage(imageItem, new Rectangle(championPreped.Value.Image.X, championPreped.Value.Image.Y, championPreped.Value.Image.Width, championPreped.Value.Image.Height));
			championPicBox.Image = image;
			championPicBox.MouseClick += frm1.mainTopBar.championPicBox_MouseClick;
			championPicBox.Cursor = Cursors.Hand;
			championPicBox.Tag = championPreped;



			Control cTemp = frm1.mainTopBar.Controls.Find("pnlChampion", true)[0];
			cTemp.Tag = championPreped;
			if (cTemp.Controls.Count > 0)
			{
				List<Control> ctrls = cTemp.Controls.Cast<Control>().ToList();
				cTemp.Controls.Clear();
				foreach (Control c in ctrls)
					c.Dispose();
			}
			cTemp.Controls.Add(championPicBox);

			string sTooltip = frm1.mainTopBar.CreateChampPicBoxTooltip(championPreped);

			//tooltip Todo: Comment ou the next two lines to disable this tooltip
			frm1.mainTopBar.ultraToolTipManagerGearIcon.SetUltraToolTip(championPicBox, frm1.mainTopBar.tipInfoChamp);
			frm1.mainTopBar.tipInfoChamp.ToolTipTextFormatted = sTooltip;
		}



		private bool pressed = false;
		private Point newMouseDelta = Point.Empty;
		private Point oldMouseDelta = Point.Empty;
		private float diffX;
		private float diffY;

		void picBoxChampion_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				pressed = true;
				oldMouseDelta = e.Location;
			}
		}

		void picBoxChampion_MouseMove(object sender, MouseEventArgs e)
		{
			if (pressed)
			{
				newMouseDelta = e.Location;

				diffX = (oldMouseDelta.X - newMouseDelta.X);
				diffY = (newMouseDelta.Y - oldMouseDelta.Y);

				if (Math.Abs(diffY) > 10 || Math.Abs(diffX) > 10)
				{
					oldMouseDelta = Point.Empty;
					pressed = false;
					Control control = sender as Control;

					frm1.championsTab.dragger.StartDragging(picBoxChampion);
					control.DoDragDrop("CHAMPION" + champ.Value.Id.ToString(), DragDropEffects.Copy | DragDropEffects.Move);
					frm1.championsTab.dragger.StopDragging();
				}
			}
		}

		void picBoxChampion_MouseUp(object sender, MouseEventArgs e)
		{
			pressed = false;
		}


	}
}

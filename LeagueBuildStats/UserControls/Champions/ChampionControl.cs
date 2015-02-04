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

			//Todo: this is temp tooltip used to see base stats for the champion
			string sTooltip = string.Format(@"
						<div style='max-width:300px;'>
						<p style='color:White; font-family:Tahoma; font-size:10pt; text-smoothing-mode:AntiAlias; max-width:300px;'> 
							<span style='font-size:12pt;'>{0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span> <br/><br/>
							Armor: {1}<br/>
							ArmorPerLevel: {2}<br/>
							AttackDamage: {3}<br/>
							AttackDamagePerLevel: {4}<br/>
							AttackRange: {5}<br/>
							AttackSpeed: {6}<br/>
							AttackSpeedPerLevel: {7}<br/>
							Crit: {8}<br/>
							CitPerLevel: {9}<br/>
							Hp: {10}<br/>
							HpPerLevel: {11}<br/>
							HpRegen: {12}<br/>
							HpRegenPerLevel: {13}<br/>
							MoveSpeed: {14}<br/>
							Mp: {15}<br/>
							MpPerLevel: {16}<br/>
							MpRegen: {17}<br/>
							MpRegenPerLevel: {18}<br/>
							SpellBlock: {19}<br/>
							SpellBlackPerLevel: {20}
						</p></div>",
						championPreped.Value.Name, 
						championPreped.Value.Stats.Armor,
						championPreped.Value.Stats.ArmorPerLevel,
						championPreped.Value.Stats.AttackDamage,
						championPreped.Value.Stats.AttackDamagePerLevel,
						championPreped.Value.Stats.AttackRange,
						Math.Round((0.625 / (1 + championPreped.Value.Stats.AttackSpeedOffset)), 3 , MidpointRounding.AwayFromZero),
						championPreped.Value.Stats.AttackSpeedPerLevel,
						championPreped.Value.Stats.Crit,
						championPreped.Value.Stats.CritPerLevel,
						championPreped.Value.Stats.Hp,
						championPreped.Value.Stats.HpPerLevel,
						championPreped.Value.Stats.HpRegen,
						championPreped.Value.Stats.HpRegenPerLevel,
						championPreped.Value.Stats.MoveSpeed,
						championPreped.Value.Stats.Mp,
						championPreped.Value.Stats.MpPerLevel,
						championPreped.Value.Stats.MpRegen,
						championPreped.Value.Stats.MpRegenPerLevel,
						championPreped.Value.Stats.SpellBlock,
						championPreped.Value.Stats.SpellBlockPerLevel);

			//tooltip Todo: Comment ou the next two lines to disable this tooltip
			frm1.mainTopBar.ultraToolTipManager1.SetUltraToolTip(championPicBox, frm1.mainTopBar.tipInfoChamp);
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

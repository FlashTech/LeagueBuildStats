using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using AutoUpdaterDotNET;

namespace TestApp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();

			InitializeEvents();

			this.label1.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();

			Form1_Load();
		}

		private void InitializeEvents()
		{
			btnCheckforUpdates.MouseClick += btnCheckforUpdates_MouseClick;
		}

		void btnCheckforUpdates_MouseClick(object sender, MouseEventArgs e)
		{
			
		}

		

		private void Form1_Load()
		{
			//Uncomment below line to see Russian version





			//AutoUpdater.CurrentCulture = CultureInfo.CreateSpecificCulture("ru-RU");

			//If you want to open download page when user click on download button uncomment below line.

			//AutoUpdater.OpenDownloadPage = true;

			//Don't want user to select remind later time in AutoUpdater notification window then uncomment 3 lines below so default remind later time will be set to 2 days.

			//AutoUpdater.LetUserSelectRemindLater = false;
			//AutoUpdater.RemindLaterTimeSpan = RemindLaterFormat.Days;
			//AutoUpdater.RemindLaterAt = 2;

			AutoUpdater.Start("http://www.angelfire.com/dragon3/rowing1s4me/LeagueBuildStats/LeagueBuildStats.xml");//www.angelfire.com/dragon3/rowing1s4me/LeagueBuildStats/LeagueBuildStats.xml");// http://rbsoft.org/updates/right-click-enhancer.xml");
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Forms
{
	public partial class CheckForUpdatesForm : Form
	{
		public bool readyContinue = false;
		private int exitCode = 100;
		public CheckForUpdatesForm()
		{
			InitializeComponent();
			btnYes.MouseClick += btnYes_MouseClick;

			try
			{
				ProcessStartInfo start = new ProcessStartInfo();
				start.Arguments = "/justcheck";
				start.FileName = Environment.CurrentDirectory + "\\LeagueBuildStatsUpdater.exe";

				//Check for updates siliently
				using (Process proc = Process.Start(start))
				{
					proc.WaitForExit();

					// Retrieve the app's exit code
					exitCode = proc.ExitCode;
				}

				//If updates were detected, check for updates again and show user interface
				if (exitCode == 0)
				{
					start.Arguments = "/checknow";
					using (Process proc = Process.Start(start))
					{
						proc.WaitForExit();

						// Retrieve the app's exit code
						exitCode = proc.ExitCode;
					}
				}
				else //If there were no updates then close this form and launch main form
				{
					CloseThisFormAndLaunchMainForm();
				}
			}
			catch
			{
				CloseThisFormAndLaunchMainForm();
			}
		}

		void btnYes_MouseClick(object sender, MouseEventArgs e)
		{
			CloseThisFormAndLaunchMainForm();
		}

		private void CloseThisFormAndLaunchMainForm()
		{
			this.Visible = false;
			this.Hide();
			var leagueBuildStatsForm = new LeagueBuildStatsForm(); //this takes ages
			leagueBuildStatsForm.Closed += (s, args) => Application.Exit();
			leagueBuildStatsForm.Show();
		}

	}
}

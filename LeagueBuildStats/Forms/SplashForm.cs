using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Forms
{
	public partial class SplashForm : Form
	{
		//Delegate for cross thread call to close
		private delegate void CloseDelegate();

		//The type of form to be displayed as the splash screen.
		private static SplashForm splashForm;

		/// <summary>
		/// Immobilizes this splashForm
		/// </summary>
		/// <param name="message"></param>
		protected override void WndProc(ref Message message)
		{
			const int WM_SYSCOMMAND = 0x0112;
			const int SC_MOVE = 0xF010;

			switch (message.Msg)
			{
				case WM_SYSCOMMAND:
					int command = message.WParam.ToInt32() & 0xfff0;
					if (command == SC_MOVE)
						return;
					break;
			}

			base.WndProc(ref message);
		}

		public SplashForm()
		{
			InitializeComponent();
		}

		static public void ShowSplashScreen()
		{
			// Make sure it is only launched once.

			if (splashForm != null)
				return;
			Thread thread = new Thread(new ThreadStart(SplashForm.ShowForm));
			thread.IsBackground = true;
			thread.SetApartmentState(ApartmentState.STA);
			thread.Start();
		}

		static private void ShowForm()
		{
			splashForm = new SplashForm();
			Application.Run(splashForm);
		}

		static public void CloseForm()
		{
			splashForm.Invoke(new CloseDelegate(SplashForm.CloseFormInternal));
		}

		static private void CloseFormInternal()
		{
			splashForm.Close();
			splashForm = null;
		}

		static public void ChangeToDownloading()
		{
			splashForm.Invoke(new CloseDelegate(SplashForm.ExecutechangeTextToDownloading));
		}

		static private void ExecutechangeTextToDownloading()
		{
			splashForm.lblText.Text = "Downloading, please wait...";
		}

		static public void ChangeToLoading()
		{
			splashForm.Invoke(new CloseDelegate(SplashForm.ExecutechangeTextToLoading));
		}

		static private void ExecutechangeTextToLoading()
		{
			splashForm.lblText.Text = "Loading, please wait...";
		}

		private delegate void delegateMethod(string text);

		static public void ChangeText(string text)
		{
			if (splashForm != null)
			{
				delegateMethod addNumbers = new delegateMethod(SplashForm.ExecutechangeText);
				splashForm.Invoke(addNumbers, text);
			}
		}

		static private void ExecutechangeText(string text)
		{
			splashForm.lblText.Text = text;
		}

	}
}

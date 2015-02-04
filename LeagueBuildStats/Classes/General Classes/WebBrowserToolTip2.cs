using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes
{
	class WebBroswerToolTip2 : ToolStripDropDown
	{
		public Control ctl;

		public WebBroswerToolTip2(Control aControl)
			: base()
		{
			this.ctl = aControl;
			Initialize();
		}

		public void Initialize()
		{
			this.AutoSize = false;
			ToolStripControlHost host = new ToolStripControlHost(this.ctl);
			this.Margin = Padding.Empty;
			this.Padding = Padding.Empty;
			host.Margin = Padding.Empty;
			host.Padding = Padding.Empty;
			host.AutoSize = false;
			host.Size = ctl.Size;
			this.Size = ctl.Size;
			this.Items.Add(host);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes
{
	class MyFlowLayoutPanel : FlowLayoutPanel
	{
		public MyFlowLayoutPanel()
		{
			this.DoubleBuffered = true;
		}
		protected override void OnScroll(ScrollEventArgs se)
		{
			this.Invalidate();
			base.OnScroll(se);
		}
	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.UserControls
{
	public partial class TransparentPanel : Panel
	{
		public TransparentPanel()
        : base()
		{
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 0x00000020; //WS_EX_TRANSPARENT
				return cp;
			}
		}

		protected void InvalidateEx()
		{
			if (Parent == null)
				return;
			Rectangle rc = new Rectangle(this.Location, this.Size);
			Parent.Invalidate(rc, true);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			//do not allow the background to be painted 
		}
	}
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes.DragUtils
{
	public class Drag
	{
		public Drag() { }
		
		[StructLayout(LayoutKind.Sequential)]
		private struct ICONINFO
		{
			public bool fIcon;
			public int xHotspot;
			public int yHotspot;
			public IntPtr hbmMask;
			public IntPtr hbmColor;
		}

		[DllImport("gdi32.dll", SetLastError = true)]
		private static extern bool DeleteObject(IntPtr hObject);

		public Control Dragged;
		public bool isDragging;
		private Bitmap bm;

		public void StartDragging(Control c)
		{
			Dragged = c;
			isDragging = true;

			bm = CursorUtil.AsBitmap(c);
		}

		public void StopDragging()
		{
			isDragging = false;
			Dragged = new Control();
		}

		public Image ItemImage()
		{
			return bm;
		}
	}
}

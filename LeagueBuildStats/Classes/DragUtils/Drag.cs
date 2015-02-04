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

		[DllImport("user32")]
		private static extern bool GetIconInfo(IntPtr hIcon, out ICONINFO pIconInfo);

		[DllImport("user32.dll")]
		private static extern IntPtr LoadCursorFromFile(string lpFileName);

		[DllImport("gdi32.dll", SetLastError = true)]
		private static extern bool DeleteObject(IntPtr hObject);

		private Bitmap BitmapFromCursor(Cursor cur)
		{
			ICONINFO ii;
			GetIconInfo(cur.Handle, out ii);

			Bitmap bmp = Bitmap.FromHbitmap(ii.hbmColor);
			DeleteObject(ii.hbmColor);
			DeleteObject(ii.hbmMask);

			BitmapData bmData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
			Bitmap dstBitmap = new Bitmap(bmData.Width, bmData.Height, bmData.Stride, PixelFormat.Format32bppArgb, bmData.Scan0);
			bmp.UnlockBits(bmData);

			return new Bitmap(dstBitmap);
		}

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

		internal Image ItemImage()
		{
			return bm;
		}
	}


}

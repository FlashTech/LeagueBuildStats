using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LeagueBuildStats.Classes
{
	public class CommonMethods
	{
		/// <summary>
		/// Crops an image
		/// </summary>
		/// <param name="img"></param>
		/// <param name="cropArea">new Rectangle(x, y, width, height)</param>
		/// <returns></returns>
		public static Image cropImage(Image img, Rectangle cropArea)
		{
			Bitmap bmpImage = new Bitmap(img);
			Bitmap bmpCrop = bmpImage.Clone(cropArea,
			bmpImage.PixelFormat);
			return (Image)(bmpCrop);
		}

		/// <summary>
		/// Download Image from Internet
		/// </summary>
		/// <param name="uri"></param>
		/// <param name="fileName"></param>
		/// <returns></returns>
		public static bool DownloadRemoteImageFile(string uri, string fileName)
		{
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
			HttpWebResponse response;
			try
			{
				response = (HttpWebResponse)request.GetResponse();
			}
			catch (Exception)
			{
				return false;
			}

			// Check that the remote file was found. The ContentType
			// check is performed since a request for a non-existent
			// image file might be redirected to a 404-page, which would
			// yield the StatusCode "OK", even though the image was not
			// found.
			if ((response.StatusCode == HttpStatusCode.OK ||
				response.StatusCode == HttpStatusCode.Moved ||
				response.StatusCode == HttpStatusCode.Redirect) &&
				response.ContentType.StartsWith("image", StringComparison.OrdinalIgnoreCase))
			{

				// if the remote file was found, download it
				using (Stream inputStream = response.GetResponseStream())
				using (Stream outputStream = File.OpenWrite(fileName))
				{
					byte[] buffer = new byte[4096];
					int bytesRead;
					do
					{
						bytesRead = inputStream.Read(buffer, 0, buffer.Length);
						outputStream.Write(buffer, 0, bytesRead);
					} while (bytesRead != 0);
				}
				return true;
			}
			else
				return false;
		}
	}
}

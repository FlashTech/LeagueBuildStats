using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes.Items
{
	public class CreateItemVersionSelection
	{

		public CreateItemVersionSelection(LeagueBuildStatsForm form, List<string> versions)
		{
			var matches = form.Controls.Find("cmbBoxEditItemVersion", true);
			foreach (Control c in matches)
			{
				ComboBoxEdit cmbBoxItemVersions = (ComboBoxEdit)c;
				ComboBoxItemCollection coll = cmbBoxItemVersions.Properties.Items;
				coll.BeginUpdate();
				try
				{
					foreach (string ver in versions)
					{
						coll.Add(ver);
					}
				}
				finally
				{
					coll.EndUpdate();
				}
			}
		}
	}
}

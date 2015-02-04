using LeagueBuildStats.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeagueBuildStats.Classes.Items
{
	public class CreateItemSortMenu
	{
		private PublicStaticVariables publicStaticVariables = new PublicStaticVariables();
		public List<string> selectedTags = new List<string>();
		private Control pnlItemSort;
		private Control flowLayoutPanelItems2;

		private DevExpress.XtraEditors.DropDownButton drpDownBtnSearchButton;
		private DevExpress.XtraEditors.TextEdit txtEditSearchBar;
		private bool performTagSort = true;

		public CreateItemSortMenu(Control c, Control flowLayoutPanelItems2)
		{
			this.pnlItemSort = c;
			this.flowLayoutPanelItems2 = flowLayoutPanelItems2;

			InitializeSearchControls();

			int posY = 30; //starting y position
			foreach (KeyValuePair<string, string> tagRow in publicStaticVariables.ListForItemTableSelections)
			{
				if (tagRow.Key.Contains("_"))
				{
					Label label1 = new Label();
					label1.AutoSize = true;
					label1.Location = new System.Drawing.Point(4, posY);
					label1.Name = "lblItemTag" + tagRow.Value;
					label1.Tag = tagRow.Value;
					label1.Text = tagRow.Key.Replace("_", ""); ;
					pnlItemSort.Controls.Add(label1);
					posY += 15;
				}
				else
				{
					CheckBox checkbox1 = new CheckBox();
					checkbox1.AutoSize = true;
					checkbox1.Location = new System.Drawing.Point(7, posY);
					checkbox1.Name = "checkItemTag" + tagRow.Value;
					checkbox1.Tag = tagRow.Value;
					checkbox1.Text = tagRow.Key;
					checkbox1.CheckedChanged += checkboxItemSort_CheckedChanged;
					pnlItemSort.Controls.Add(checkbox1);
					posY += 17;
				}
			}
		}

		public void checkboxItemSort_CheckedChanged(object sender, EventArgs e)
		{
			if (performTagSort)
			{
				CheckBox chkBox = (CheckBox)sender;
				string tag = (string)chkBox.Tag;
				if (chkBox.Checked)
				{
					if (!selectedTags.Contains(tag))
					{
						selectedTags.Add(tag);
					}
				}
				else
				{
					if (selectedTags.Contains(tag))
					{
						selectedTags.Remove(tag);
					}
				}
				if (flowLayoutPanelItems2 != null)
				{
					//Clear search text bar
					txtEditSearchBar.Text = "";

					//Perform tag sorting
					List<KeyValuePair<Control, bool>> ItemControlUpdates = new List<KeyValuePair<Control, bool>>();

					foreach (Control cItem in flowLayoutPanelItems2.Controls)
					{
						bool itemVisible = true;
						List<string> iTags = ((ItemTags)cItem.Tag).Tags;
						if (iTags != null)
						{
							foreach (string sTag in selectedTags)
							{
								if (!iTags.Contains(sTag))
								{
									itemVisible = false;
								}
							}
						}
						ItemControlUpdates.Add(new KeyValuePair<Control, bool>(cItem, itemVisible));
					}
					flowLayoutPanelItems2.SuspendLayout();
					foreach (KeyValuePair<Control, bool> i in ItemControlUpdates)
					{
						i.Key.Visible = i.Value;
					}
					flowLayoutPanelItems2.ResumeLayout();
				}
			}
		}







		private void InitializeSearchControls()
		{
			txtEditSearchBar = new DevExpress.XtraEditors.TextEdit();
			drpDownBtnSearchButton = new DevExpress.XtraEditors.DropDownButton();
			((System.ComponentModel.ISupportInitialize)(txtEditSearchBar.Properties)).BeginInit();
			pnlItemSort.Controls.Add(drpDownBtnSearchButton);
			pnlItemSort.Controls.Add(txtEditSearchBar);
			// 
			// txtEditSearchBar
			// 
			this.txtEditSearchBar.Location = new System.Drawing.Point(3, 3);
			this.txtEditSearchBar.Name = "txtEditSearchBar";
			this.txtEditSearchBar.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtEditSearchBar.Properties.Appearance.Options.UseFont = true;
			this.txtEditSearchBar.Size = new System.Drawing.Size(114, 22);
			this.txtEditSearchBar.TabIndex = 1;
			// 
			// drpDownBtnSearchButton
			// 
			this.drpDownBtnSearchButton.DropDownArrowStyle = DevExpress.XtraEditors.DropDownArrowStyle.Hide;
			this.drpDownBtnSearchButton.Location = new System.Drawing.Point(123, 3);
			this.drpDownBtnSearchButton.Name = "drpDownBtnSearchButton";
			this.drpDownBtnSearchButton.Size = new System.Drawing.Size(55, 22);
			this.drpDownBtnSearchButton.TabIndex = 2;
			this.drpDownBtnSearchButton.Text = "Search";
			((System.ComponentModel.ISupportInitialize)(txtEditSearchBar.Properties)).EndInit();

			txtEditSearchBar.KeyPress += txtEditSearchBar_KeyPress;
			drpDownBtnSearchButton.MouseClick += drpDownBtnSearchButton_MouseClick;
		}

		void drpDownBtnSearchButton_MouseClick(object sender, MouseEventArgs e)
		{
			PerformSearch();
		}

		private void PerformSearch()
		{
			//Clear tags selected
			selectedTags.Clear();
			performTagSort = false;
			foreach (Control c in pnlItemSort.Controls)
			{
				try
				{
					CheckBox chk = c as CheckBox;
					chk.Checked = false;
				}
				catch
				{
					//do nothing
				}
			}
			performTagSort = true;

			List<KeyValuePair<Control, bool>> ItemControlUpdates = new List<KeyValuePair<Control, bool>>();

			foreach (Control cItem in flowLayoutPanelItems2.Controls)
			{
				bool itemVisible = false;


				string cName = ((ItemTags)cItem.Tag).ItemName.ToLower();

				string cName2 = cName.Replace("'", "");
				if (cName.Contains(txtEditSearchBar.Text.ToLower()) || cName2.Contains(txtEditSearchBar.Text.ToLower()))
				{
					itemVisible = true;
				}

				ItemControlUpdates.Add(new KeyValuePair<Control, bool>(cItem, itemVisible));
			}
			flowLayoutPanelItems2.SuspendLayout();
			foreach (KeyValuePair<Control, bool> i in ItemControlUpdates)
			{
				i.Key.Visible = i.Value;
			}
			flowLayoutPanelItems2.ResumeLayout();

			//Refocus search text bar
			txtEditSearchBar.Focus();


		}

		void txtEditSearchBar_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == Convert.ToInt32(Keys.Enter))
			{
				PerformSearch();
			}
		}

	}
}

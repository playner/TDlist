using System;
using MetroFramework.Controls;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TD
{
    public partial class CustomMessageBox : MetroFramework.Forms.MetroForm
    {
        private MetroTextBox textBox; 
        public int CheckboxIndexToDelete { get; set; }

        public CustomMessageBox(String WKOBTN)
        {
            // 폼 초기화 및 버튼 추가
            this.Text = "";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Size = new System.Drawing.Size(300, 200);

            #region 텍스트 박스

            textBox = new MetroTextBox();
            textBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            textBox.Lines = new string[] { "metroTextBox1" };
            textBox.Location = new System.Drawing.Point(5, 50);
            textBox.Name = "metroTextBox1";
            textBox.Size = new System.Drawing.Size(285, 89);
            textBox.Text = "";
            textBox.Multiline = true;


            #endregion

            MetroButton confirmButton = new MetroButton() { Text = "수정", DialogResult = DialogResult.OK };
            MetroButton deleteButton = new MetroButton() { Text = "삭제", DialogResult = DialogResult.No };

            if (WKOBTN == "addBoxBtn") confirmButton.Text = "추가";

            deleteButton.Location = new Point((this.Width - confirmButton.Width * 2) - 30, this.Height - confirmButton.Height - 15);
            confirmButton.Location = new Point((this.Width - deleteButton.Width) - 15, this.Height - deleteButton.Height - 15);

            confirmButton.Click += (sender, e) => this.Close();
            deleteButton.Click += (sender, e) => this.Close();

            this.Controls.Add(confirmButton);
            this.Controls.Add(textBox);

            if (WKOBTN == "criteriaModifyBtn") this.Controls.Add(deleteButton);


            //this.FormClosing += new FormClosingEventHandler(this.WarningClosed);
        }

        public string GetInputText()
        {
            return textBox.Text;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TD
{
    public partial class Warning : MetroFramework.Forms.MetroForm
    {
        public Warning()
        {
            this.Text = "경고";
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Size = new System.Drawing.Size(300, 200);
            this.TopMost = true;

            MetroButton confirmButton = new MetroButton() { Text = "확인" };
            PictureBox pictureBox = new PictureBox();
            MetroLabel label = new MetroLabel();

            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose(); // 이미지를 해제
                pictureBox.Image = null; // 이미지를 해제
            }

            pictureBox.Image = Image.FromFile("Icon\\warning.png");
            pictureBox.Size = new Size(64, 56); // PictureBox의 크기 조절
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Location = new System.Drawing.Point(10, 80);

            confirmButton.Location = new Point((this.Width - confirmButton.Width * 2) - 30, this.Height - confirmButton.Height - 15);

            label.Text = "첫 번째 리스트는 \n삭제가 불가합니다.";
            label.Location = new System.Drawing.Point(90, 80);
            label.Theme = MetroFramework.MetroThemeStyle.Dark;
            label.FontWeight = MetroFramework.MetroLabelWeight.Bold;
            label.FontSize = MetroFramework.MetroLabelSize.Tall;
            label.AutoSize = true;

            this.Controls.Add(pictureBox);
            this.Controls.Add(confirmButton);
            this.Controls.Add(label);

            confirmButton.Click += (sender, e) => this.Close();

            this.FormClosing += new FormClosingEventHandler(this.WarningClosed);
        }
        private void WarningClosed(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}

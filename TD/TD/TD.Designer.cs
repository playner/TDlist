using System.Drawing;
using System.Windows.Forms;

namespace TD
{
    partial class TD
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TD));
            this.metroProgressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.addBoxBtn = new MetroFramework.Controls.MetroButton();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.CheckBox = new MetroFramework.Controls.MetroTabControl();
            this.CheckBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // metroProgressBar1
            // 
            resources.ApplyResources(this.metroProgressBar1, "metroProgressBar1");
            this.metroProgressBar1.Name = "metroProgressBar1";
            this.metroProgressBar1.Theme = MetroFramework.MetroThemeStyle.Dark;
            // 
            // addBoxBtn
            // 
            this.addBoxBtn.FontSize = MetroFramework.MetroButtonSize.Tall;
            resources.ApplyResources(this.addBoxBtn, "addBoxBtn");
            this.addBoxBtn.Name = "addBoxBtn";
            this.addBoxBtn.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.addBoxBtn.UseSelectable = true;
            this.addBoxBtn.Click += new System.EventHandler(this.addBoxBtn_Click);
            // 
            // debugTextBox
            // 
            resources.ApplyResources(this.debugTextBox, "debugTextBox");
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ReadOnly = true;
            // 
            // metroTabPage2
            // 
            resources.ApplyResources(this.metroTabPage2, "metroTabPage2");
            this.metroTabPage2.HorizontalScrollbar = true;
            this.metroTabPage2.HorizontalScrollbarBarColor = false;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 0;
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.VerticalScrollbar = true;
            this.metroTabPage2.VerticalScrollbarBarColor = false;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 0;
            // 
            // CheckBox
            // 
            this.CheckBox.Controls.Add(this.metroTabPage2);
            resources.ApplyResources(this.CheckBox, "CheckBox");
            this.CheckBox.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.SelectedIndex = 0;
            this.CheckBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.CheckBox.UseSelectable = true;
            // 
            // TD
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.debugTextBox);
            this.Controls.Add(this.CheckBox);
            this.Controls.Add(this.addBoxBtn);
            this.Controls.Add(this.metroProgressBar1);
            this.Name = "TD";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.TD_Load);
            this.CheckBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroProgressBar metroProgressBar1;
        private MetroFramework.Controls.MetroButton addBoxBtn;
        private System.Windows.Forms.TextBox debugTextBox;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
        private MetroFramework.Controls.MetroTabControl CheckBox;
    }
}


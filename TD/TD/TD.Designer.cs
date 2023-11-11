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
            this.체크박스0 = new MetroFramework.Controls.MetroCheckBox();
            this.metroProgressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.addBoxBtn = new MetroFramework.Controls.MetroButton();
            this.판넬0 = new MetroFramework.Controls.MetroPanel();
            this.criteriaModifyBtn = new MetroFramework.Controls.MetroButton();
            this.CheckBox = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.판넬0.SuspendLayout();
            this.CheckBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // 체크박스0
            // 
            resources.ApplyResources(this.체크박스0, "체크박스0");
            this.체크박스0.FontSize = MetroFramework.MetroCheckBoxSize.Tall;
            this.체크박스0.FontWeight = MetroFramework.MetroCheckBoxWeight.Bold;
            this.체크박스0.Name = "체크박스0";
            this.체크박스0.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.체크박스0.UseSelectable = true;
            this.체크박스0.CheckedChanged += new System.EventHandler(this.criteriaCheckBoxChange);
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
            // 판넬0
            // 
            resources.ApplyResources(this.판넬0, "판넬0");
            this.판넬0.Controls.Add(this.criteriaModifyBtn);
            this.판넬0.Controls.Add(this.체크박스0);
            this.판넬0.HorizontalScrollbar = true;
            this.판넬0.HorizontalScrollbarBarColor = true;
            this.판넬0.HorizontalScrollbarHighlightOnWheel = false;
            this.판넬0.HorizontalScrollbarSize = 10;
            this.판넬0.Name = "판넬0";
            this.판넬0.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.판넬0.VerticalScrollbar = true;
            this.판넬0.VerticalScrollbarBarColor = true;
            this.판넬0.VerticalScrollbarHighlightOnWheel = false;
            this.판넬0.VerticalScrollbarSize = 10;
            // 
            // criteriaModifyBtn
            // 
            this.criteriaModifyBtn.FontSize = MetroFramework.MetroButtonSize.Tall;
            resources.ApplyResources(this.criteriaModifyBtn, "criteriaModifyBtn");
            this.criteriaModifyBtn.Name = "criteriaModifyBtn";
            this.criteriaModifyBtn.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.criteriaModifyBtn.UseSelectable = true;
            this.criteriaModifyBtn.Click += new System.EventHandler(this.criteriaModifyBtn_Click);
            // 
            // CheckBox
            // 
            this.CheckBox.Controls.Add(this.metroTabPage1);
            this.CheckBox.Controls.Add(this.metroTabPage2);
            resources.ApplyResources(this.CheckBox, "CheckBox");
            this.CheckBox.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.CheckBox.Name = "CheckBox";
            this.CheckBox.SelectedIndex = 0;
            this.CheckBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.CheckBox.UseSelectable = true;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.HorizontalScrollbarBarColor = false;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            resources.ApplyResources(this.metroTabPage1, "metroTabPage1");
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.VerticalScrollbarBarColor = false;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
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
            // debugTextBox
            // 
            resources.ApplyResources(this.debugTextBox, "debugTextBox");
            this.debugTextBox.Name = "debugTextBox";
            this.debugTextBox.ReadOnly = true;
            // 
            // TD
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.debugTextBox);
            this.Controls.Add(this.CheckBox);
            this.Controls.Add(this.판넬0);
            this.Controls.Add(this.addBoxBtn);
            this.Controls.Add(this.metroProgressBar1);
            this.Name = "TD";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.TD_Load);
            this.판넬0.ResumeLayout(false);
            this.판넬0.PerformLayout();
            this.CheckBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroCheckBox 체크박스0;
        private MetroFramework.Controls.MetroProgressBar metroProgressBar1;
        private MetroFramework.Controls.MetroButton addBoxBtn;
        private MetroFramework.Controls.MetroPanel 판넬0;
        private MetroFramework.Controls.MetroTabControl CheckBox;
        private MetroFramework.Controls.MetroButton criteriaModifyBtn;
        private System.Windows.Forms.TextBox debugTextBox;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
    }
}


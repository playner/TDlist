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
            this.criteriaCheckBox = new MetroFramework.Controls.MetroCheckBox();
            this.metroProgressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.addBoxBtn = new MetroFramework.Controls.MetroButton();
            this.checkBoxPanel = new MetroFramework.Controls.MetroPanel();
            this.criteriaModifyBtn = new MetroFramework.Controls.MetroButton();
            this.tabControl = new MetroFramework.Controls.MetroTabControl();
            this.metroTabPage1 = new MetroFramework.Controls.MetroTabPage();
            this.metroTabPage2 = new MetroFramework.Controls.MetroTabPage();
            this.debugTextBox = new System.Windows.Forms.TextBox();
            this.checkBoxPanel.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // criteriaCheckBox
            // 
            resources.ApplyResources(this.criteriaCheckBox, "criteriaCheckBox");
            this.criteriaCheckBox.FontSize = MetroFramework.MetroCheckBoxSize.Tall;
            this.criteriaCheckBox.FontWeight = MetroFramework.MetroCheckBoxWeight.Bold;
            this.criteriaCheckBox.Name = "criteriaCheckBox";
            this.criteriaCheckBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.criteriaCheckBox.UseSelectable = true;
            this.criteriaCheckBox.CheckedChanged += new System.EventHandler(this.criteriaCheckBoxChange);
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
            // checkBoxPanel
            // 
            resources.ApplyResources(this.checkBoxPanel, "checkBoxPanel");
            this.checkBoxPanel.Controls.Add(this.criteriaModifyBtn);
            this.checkBoxPanel.Controls.Add(this.criteriaCheckBox);
            this.checkBoxPanel.HorizontalScrollbar = true;
            this.checkBoxPanel.HorizontalScrollbarBarColor = true;
            this.checkBoxPanel.HorizontalScrollbarHighlightOnWheel = false;
            this.checkBoxPanel.HorizontalScrollbarSize = 10;
            this.checkBoxPanel.Name = "checkBoxPanel";
            this.checkBoxPanel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.checkBoxPanel.VerticalScrollbar = true;
            this.checkBoxPanel.VerticalScrollbarBarColor = true;
            this.checkBoxPanel.VerticalScrollbarHighlightOnWheel = false;
            this.checkBoxPanel.VerticalScrollbarSize = 10;
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
            // tabControl
            // 
            this.tabControl.Controls.Add(this.metroTabPage1);
            this.tabControl.Controls.Add(this.metroTabPage2);
            this.tabControl.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            resources.ApplyResources(this.tabControl, "tabControl");
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.tabControl.UseSelectable = true;
            this.tabControl.TabIndex = 0;
            // 
            // metroTabPage1
            // 
            this.metroTabPage1.HorizontalScrollbarBarColor = true;
            this.metroTabPage1.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.HorizontalScrollbarSize = 10;
            resources.ApplyResources(this.metroTabPage1, "metroTabPage1");
            this.metroTabPage1.Name = "metroTabPage1";
            this.metroTabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.metroTabPage1.VerticalScrollbarBarColor = true;
            this.metroTabPage1.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage1.VerticalScrollbarSize = 10;
            this.metroTabPage1.TabIndex = 0;
            // 
            // metroTabPage2
            // 
            this.metroTabPage2.HorizontalScrollbarBarColor = true;
            this.metroTabPage2.HorizontalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.HorizontalScrollbarSize = 10;
            resources.ApplyResources(this.metroTabPage2, "metroTabPage2");
            this.metroTabPage2.Name = "metroTabPage2";
            this.metroTabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.metroTabPage2.VerticalScrollbarBarColor = true;
            this.metroTabPage2.VerticalScrollbarHighlightOnWheel = false;
            this.metroTabPage2.VerticalScrollbarSize = 10;
            this.metroTabPage2.TabIndex = 1;
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
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.checkBoxPanel);
            this.Controls.Add(this.addBoxBtn);
            this.Controls.Add(this.metroProgressBar1);
            this.Name = "TD";
            this.TextAlign = MetroFramework.Forms.MetroFormTextAlign.Center;
            this.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.Load += new System.EventHandler(this.TD_Load);
            this.checkBoxPanel.ResumeLayout(false);
            this.checkBoxPanel.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroCheckBox criteriaCheckBox;
        private MetroFramework.Controls.MetroProgressBar metroProgressBar1;
        private MetroFramework.Controls.MetroButton addBoxBtn;
        private MetroFramework.Controls.MetroPanel checkBoxPanel;
        private MetroFramework.Controls.MetroTabControl tabControl;
        private MetroFramework.Controls.MetroButton criteriaModifyBtn;
        private System.Windows.Forms.TextBox debugTextBox;
        private MetroFramework.Controls.MetroTabPage metroTabPage1;
        private MetroFramework.Controls.MetroTabPage metroTabPage2;
    }
}


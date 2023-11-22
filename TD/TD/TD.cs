using IniParser;
using IniParser.Model;
using MetroFramework;
using MetroFramework.Controls;
using MetroFramework.Drawing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TD
{
    public partial class TD : MetroFramework.Forms.MetroForm
    {
        #region INI파일 API
        public static Encoding EUCKREncoding()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding euckr = Encoding.GetEncoding("euc-kr");
            return euckr;
        }
        //[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]

        //private static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true, EntryPoint = "WritePrivateProfileStringW")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder value, int size, string filePath);

        #endregion

        #region API 정의 함수

        private string getIni(string lpAppName, string lpKeyName, string lpDefault, string filePath)
        {
            string iniFile = filePath;

            try
            {
                StringBuilder result = new StringBuilder(2048);
                GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, result, result.Capacity, iniFile);

                return result.ToString();
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return "실패";
            }
        }

        private Boolean setIni(string lpAppName, string lpKeyName, string lpValue, string filePath)
        {
            try
            {
                string iniFile = filePath;
                WritePrivateProfileString(lpAppName, lpKeyName, lpValue, iniFile);
                return true;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                return false;
            }
        }

        private Boolean CreateIni(string strFileName)
        {
            try
            {
                string strCheckFolder = "";

                strCheckFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
                strCheckFolder += "\\INI";
                if (!System.IO.Directory.Exists(strCheckFolder)) System.IO.Directory.CreateDirectory(strCheckFolder);

                strCheckFolder += "\\" + strFileName + ".ini";

                if (!System.IO.File.Exists(strCheckFolder))
                {
                    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(strCheckFolder, false, EUCKREncoding()))
                    {
                        sw.Write("");
                        sw.Flush();
                        sw.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

                return false;
            }

            return true;
        }

        #endregion

        #region 윈도우 시작시 실행        
        private Microsoft.Win32.RegistryKey GetRegKey(string regPath, bool writable)
        {
            return Microsoft.Win32.Registry.CurrentUser.OpenSubKey(regPath, writable);
        }

        // 부팅시 시작 프로그램 등록
        public void AddStartupProgram(string programName, string executablePath)
        {
            using (var regKey = GetRegKey(_startupRegPath, true))
            {
                try
                {
                    // 키가 이미 등록돼 있지 않을때만 등록
                    if (regKey.GetValue(programName) == null)
                        regKey.SetValue(programName, executablePath);

                    regKey.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        #endregion

        string strCheckFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
        string strCheckFolder2 = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));

        List<Tuple<MetroButton, MetroCheckBox>> buttonCheckBoxPairs = new List<Tuple<MetroButton, MetroCheckBox>>();
        List<MetroPanel> tabPanels = new List<MetroPanel>();

        private FileIniDataParser iniParser = new FileIniDataParser();
        private MouseEventArgs mouseEventArgsForRemove;

        private static readonly string _startupRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        string buttonText = "";
        int hoveredTabIndex = -1;
        int newData = 0;

        public TD()
        {
            InitializeComponent();
            CreateIni("CheckBoxData");
            CreateIni("CheckBoxState");

            CheckBox.TabPages[CheckBox.TabCount - 1].Text = "";
            CheckBox.Padding = new Point(14, 4);

            CheckBox.MouseDown += new MouseEventHandler(tabControl_MouseDown);
            CheckBox.Selecting += new TabControlCancelEventHandler(tabControl_Selecting);
            CheckBox.MouseEnter += tabControl_MouseEnter;
            CheckBox.MouseLeave += tabControl_MouseLeave;
            CheckBox.MouseMove += tabControl_MouseMove;
            CheckBox.CustomPaintForeground += tabControl_CustomPaint;
            CheckBox.SelectedIndexChanged += tabControl_SelectedIndexChanged;
        }
        //asdasd
        private void TD_Load(object sender, EventArgs e)
        {
            AddStartupProgram("TD", Application.ExecutablePath);

            //string dataINIFilePath = strCheckFolder + "\\INI\\CheckBoxData.ini";
            //string stateINIFilePath = strCheckFolder + "\\INI\\CheckBoxState.ini";

            strCheckFolder += "\\INI\\CheckBoxData.ini";
            strCheckFolder2 += "\\INI\\CheckBoxState.ini";

            IniData iniData = iniParser.ReadFile(strCheckFolder, EUCKREncoding());
            KeyDataCollection keyDatas = iniData["CheckBox"];
            string[] modifyIniLines = File.ReadAllLines(strCheckFolder, EUCKREncoding());
            string[] modifyIniLines2 = File.ReadAllLines(strCheckFolder2, EUCKREncoding());

            if (keyDatas.Count == 0)
            {
                setIni("CheckBox", "CriteriaCheckBox", "기본", strCheckFolder);
                setIni("CheckBoxState", "CriteriaCheckBox", "False", strCheckFolder2);
            }

            embodyINIData(modifyIniLines);
            LoadCheckboxStates(modifyIniLines2);
        }

        private void criteriaCheckBoxChange(object sender, EventArgs e)
        {
            var checkBox = (MetroCheckBox)sender;
            var button = buttonCheckBoxPairs.FirstOrDefault(pair => pair.Item2 == checkBox);
            if (button != null)
            {
                setIni("CheckBoxState", checkBox.Name.ToString(), $"{checkBox.Checked}", strCheckFolder2);
            }
        }

        private void addBoxBtn_Click(object sender, EventArgs e)
        {
            InitializeNewList();
            //int changePosition = 판넬0.VerticalScroll.Value + 판넬0.VerticalScroll.SmallChange * 30;
            //판넬0.AutoScrollPosition = new Point(0, changePosition);
            metroProgressBar1.Maximum++;
        }

        private void InitializeNewList()
        {
            CustomMessageBox messageBox = new CustomMessageBox("addBoxBtn");
            DialogResult result = messageBox.ShowDialog();
            buttonText = messageBox.GetInputText();

            if (result == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(buttonText))
                {
                    MetroTabPage currentTab = (MetroTabPage)CheckBox.SelectedTab;
                    MetroPanel currentPanel = tabPanels[CheckBox.SelectedIndex];

                    Tuple<MetroButton, MetroCheckBox> lastPair = buttonCheckBoxPairs[buttonCheckBoxPairs.Count - 1];

                    MetroCheckBox newCheckBox = new MetroCheckBox();
                    newCheckBox.FontSize = MetroCheckBoxSize.Tall;
                    newCheckBox.FontWeight = MetroCheckBoxWeight.Bold;
                    newCheckBox.Text = buttonText;
                    newCheckBox.Name = "체크박스" + currentPanel.Controls.OfType<MetroCheckBox>().Count();
                    newCheckBox.Theme = MetroThemeStyle.Dark;
                    newCheckBox.UseSelectable = true;
                    if (currentPanel.Controls.OfType<MetroCheckBox>().Count() == 0) newCheckBox.Location = new Point(15 + newCheckBox.Width, 10 + newCheckBox.Height);
                    else newCheckBox.Location = new Point(lastPair.Item2.Left, lastPair.Item2.Top + lastPair.Item2.Height + 5);
                    newCheckBox.Size = new Size(57, 25);
                    newCheckBox.AutoSize = true;
                    newCheckBox.CheckedChanged += new EventHandler(criteriaCheckBoxChange);

                    currentPanel.Controls.Add(newCheckBox);

                    MetroButton newButton = new MetroButton();
                    newButton.Text = "...";
                    newButton.Theme = MetroThemeStyle.Dark;
                    newButton.FontSize = MetroButtonSize.Tall;
                    newButton.Name = "버튼" + buttonCheckBoxPairs.Count;
                    newButton.Size = new Size(44, 25);
                    if (currentPanel.Controls.OfType<MetroButton>().Count() == 0) newButton.Location = new Point(228 + newButton.Size.Width, 10 + newButton.Size.Height);
                    else newButton.Location = new Point(lastPair.Item1.Left, lastPair.Item1.Top + lastPair.Item1.Height + 5);

                    newButton.Click += new EventHandler(criteriaModifyBtn_Click);

                    currentPanel.Controls.Add(newButton);
                    buttonCheckBoxPairs.Add(new Tuple<MetroButton, MetroCheckBox>(newButton, newCheckBox));


                    setIni(currentTab.Name + "CBData", newCheckBox.Name, buttonText, strCheckFolder);
                    setIni(currentTab.Name + "CBState", newCheckBox.Name, "False", strCheckFolder2);
                }
            }
        }

        private void criteriaModifyBtn_Click(object sender, EventArgs e)
        {
            MetroButton clickedButton = sender as MetroButton;
            CustomMessageBox messageBox = new CustomMessageBox("criteriaModifyBtn");
            KeyDataCollection boxSectionData = new KeyDataCollection();
            KeyDataCollection checksectionData = new KeyDataCollection();
            IniData iniData = iniParser.ReadFile(strCheckFolder, EUCKREncoding());
            IniData iniData2 = iniParser.ReadFile(strCheckFolder2, EUCKREncoding());
            List<string> sectionNames = new List<string>();

            DialogResult result = messageBox.ShowDialog();
            buttonText = messageBox.GetInputText();
            
            bool inTargetSection = false, isComplete = false, isFirst = false, isFirst2 = false;
            string[] modifyIniLines = File.ReadAllLines(strCheckFolder, EUCKREncoding());
            string[] modifyIniLines2 = File.ReadAllLines(strCheckFolder2, EUCKREncoding());
            string section = "CheckBox"; // 섹션 이름
            string section2 = "CheckBoxState"; // 섹션 이름
            string checkRTK = "";
            int pairIndex = buttonCheckBoxPairs.FindIndex(pair => pair.Item1 == clickedButton);
            int sectionNum = 0;
            int checkboxNum = 1;

            foreach (SectionData section3 in iniData.Sections)
            {
                sectionNames.Add(section3.SectionName);
            }

            if (result == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(buttonText))
                {
                    for (int i = 0; i < modifyIniLines.Length; i++)
                    {
                        if (modifyIniLines[i].StartsWith($"[{sectionNames[sectionNum]}]"))
                        {
                            inTargetSection = true; // 목표 섹션 내부에 있다는 플래그를 설정
                        }

                        else if (inTargetSection && !isComplete)
                        {
                            int realIndex = pairIndex + 1;

                            string[] keyValue = modifyIniLines[realIndex].Split(new char[] { '=' }, 2);

                            if (keyValue.Length == 2)
                            {
                                string key = keyValue[0].Trim();

                                setIni(sectionNames[sectionNum], key, buttonText, strCheckFolder);
                                isComplete = true;
                            }
                            checkboxNum++;
                        }

                        if (i == modifyIniLines.Length - 1 || modifyIniLines[i + 1].StartsWith("["))
                        {
                            sectionNum++;
                            inTargetSection = false;

                        }
                    }
                   //RefreshUI(sectionNames[sectionNum], result, pairIndex, modifyIniLines, buttonText);
                }
            }

            else if (result == DialogResult.No)
            {
                for (int i = 0; i < modifyIniLines.Length; i++)
                {
                    if (modifyIniLines[i].StartsWith($"[{section}]"))
                    {
                        inTargetSection = true; // 목표 섹션 내부에 있다는 플래그를 설정
                    }

                    else if (inTargetSection && !isComplete)
                    {
                        int realIndex = pairIndex + 1;

                        string[] keyValue = modifyIniLines[realIndex].Split(new char[] { '=' }, 2);

                        if (keyValue.Length == 2)
                        {
                            string removeToKey = keyValue[0].Trim();
                            string value = keyValue[1].Trim();
                            bool keyBoolValue = bool.Parse(iniData2[section2][removeToKey]);

                            if (removeToKey != "CriteriaCheckBox")
                            {
                                iniData[section].RemoveKey(removeToKey);
                                iniData2[section2].RemoveKey(removeToKey);
                                checkRTK = value;
                                metroProgressBar1.Maximum--;
                                if (keyBoolValue == true)
                                {
                                    newData--;
                                }
                                isComplete = true;
                            }

                            else
                            {
                                Warning warningSign = new Warning();
                                warningSign.Show();
                                isComplete = true;
                            }
                        }
                    }

                    else
                    {
                        inTargetSection = false; // 다음 섹션으로 이동
                    }
                }

                if (pairIndex != 0)
                {
                    RefreshUI(section, result, pairIndex, modifyIniLines, null);

                    #region 섹션데이터에 키값 미리 추가

                    int j = 0, l = 0;

                    for (int i = 0; i < modifyIniLines.Length; i++)
                    {
                        if (modifyIniLines[i].StartsWith($"[{section}]"))
                        {
                            inTargetSection = true;
                            isFirst = true;
                        }

                        else if (inTargetSection)
                        {
                            string[] keyValue = modifyIniLines[i].Split(new char[] { '=' }, 2);

                            if (keyValue.Length == 2)
                            {
                                string value = keyValue[1].Trim();

                                if (isFirst)
                                {
                                    boxSectionData.AddKey("CriteriaCheckBox", value);
                                    isFirst = false;
                                }

                                else if (value != checkRTK)
                                {
                                    j++;
                                    boxSectionData.AddKey("체크박스" + j, value);
                                }
                            }
                        }

                        else
                            inTargetSection = false;
                    }

                    for (int i = 0; i < modifyIniLines2.Length - 1; i++)
                    {
                        if (modifyIniLines2[i].StartsWith($"[{section2}]"))
                        {
                            inTargetSection = true;
                            isFirst2 = true;
                        }

                        else if (inTargetSection)
                        {
                            string[] keyValue = modifyIniLines2[i].Split(new char[] { '=' }, 2);

                            if (keyValue.Length == 2)
                            {
                                string value = keyValue[1].Trim();

                                if (isFirst2)
                                {
                                    checksectionData.AddKey("CriteriaCheckBox", value);
                                    isFirst2 = false;
                                }

                                else
                                {
                                    l++;
                                    checksectionData.AddKey("체크박스" + l, value);
                                }
                            }
                        }

                        else
                            inTargetSection = false;
                    }

                    #endregion

                    iniData.Sections.RemoveSection(section);
                    iniData2.Sections.RemoveSection(section2);

                    iniParser.WriteFile(strCheckFolder, iniData, Encoding.Default);
                    iniParser.WriteFile(strCheckFolder2, iniData, Encoding.Default);

                    for (int k = 0; k < boxSectionData.Count; k++)
                    {
                        if (k == 0)
                        {
                            KeyData keyData = boxSectionData.GetKeyData("CriteriaCheckBox");
                            string stringkey = keyData.Value.Trim();
                            setIni(section, "CriteriaCheckBox", stringkey, strCheckFolder);
                        }

                        else
                        {
                            KeyData keyData = boxSectionData.GetKeyData("체크박스" + k);
                            string stringkey = keyData.Value.Trim();
                            setIni(section, "체크박스" + k, stringkey, strCheckFolder);
                        }
                    }

                    for (int k = 0; k < boxSectionData.Count; k++)
                    {
                        if (k == 0)
                        {
                            KeyData keyData2 = checksectionData.GetKeyData("CriteriaCheckBox");
                            string stringkey = keyData2.Value.Trim();
                            setIni(section2, "CriteriaCheckBox", stringkey, strCheckFolder2);
                        }

                        else
                        {
                            KeyData keyData2 = checksectionData.GetKeyData("체크박스" + k);
                            string stringkey = keyData2.Value.Trim();
                            setIni(section2, "체크박스" + k, stringkey, strCheckFolder2);
                        }
                    }
                }
            }
        }

        private void embodyINIData(string[] modifyIniLines)
        {
            IniData iniData = iniParser.ReadFile(strCheckFolder, EUCKREncoding());
            KeyDataCollection sectionData = new KeyDataCollection();
            List<string> sectionNames = new List<string>();
            int sectionNum = 0;
            int checkboxNum = 0;
            bool inTargetSection = false;

            foreach (SectionData section in iniData.Sections)
            {
                sectionNames.Add(section.SectionName);
            }

            #region 섹션데이터에 키값 미리 추가
            /*for (int i = 0; i < modifyIniLines.Length; i++)
            {
                if (modifyIniLines[i].StartsWith($"[{sectionNames[sectionNum]}]"))
                {
                    inTargetSection = true;
                    checkboxNum = 1;
                }

                else if (inTargetSection)
                {
                    string[] keyValue = modifyIniLines[i].Split(new char[] { '=' }, 2);

                    if (keyValue.Length == 2)
                    {
                        string value = keyValue[1].Trim();
                        sectionData.AddKey($"{sectionNum}체크박스{checkboxNum}", value);

                        KeyData keyData2 = sectionData.GetKeyData($"{sectionNum}체크박스{checkboxNum}");
                        string stringkey2 = keyData2.Value.Trim();
                        DisplayDebugInfo(stringkey2);
                        checkboxNum++;

                    }

                    if (i == modifyIniLines.Length - 1 || modifyIniLines[i + 1].StartsWith("["))
                    {
                        sectionNum++;
                        inTargetSection = false;
                    }
                }
            }*/

            sectionNum = 0;

            #endregion

            for (int i = 0; i < modifyIniLines.Length; i++)
            {
                if (modifyIniLines[i].StartsWith($"[{sectionNames[sectionNum]}]"))
                {
                    inTargetSection = true; // 목표 섹션 내부에 있다는 플래그를 설정                   

                    MetroTabPage newTab = new MetroTabPage();
                    newTab.Text = sectionNames[sectionNum];
                    newTab.Name = sectionNames[sectionNum];

                    MetroPanel newPanel = new MetroPanel();
                    newPanel.Dock = DockStyle.Fill;
                    newPanel.Name = newTab.Text;
                    newPanel.Theme = MetroThemeStyle.Dark;
                    newPanel.Location = new Point(23, 88);
                    newPanel.Size = new Size(287, 501);
                    tabPanels.Add(newPanel);
                    Controls.Add(newPanel);

                    CheckBox.TabPages.Insert(CheckBox.TabCount - 1, newTab);
                    CheckBox.SelectedIndex = CheckBox.TabCount - 2;
                    CheckBox.SelectedTab = CheckBox.TabPages[CheckBox.TabPages.Count - 1];

                    //count = iniData[sectionNames[sectionNum]].Count;

                    checkboxNum = 1;
                }

                else if (inTargetSection)
                {
                    string[] keyValue = modifyIniLines[i].Split(new char[] { '=' }, 2);

                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();

                        /*for (int k = 0; k < count; k++)
                        {
                            KeyData keyData = sectionData.GetKeyData(sectionNum + "체크박스" + (k + 1));
                            string stringkey = keyData.Value.Trim();
                            setIni(sectionNames[sectionNum], "체크박스" + (k + 1), stringkey, strCheckFolder);
                            iniParser.WriteFile(strCheckFolder, iniData, Encoding.Default);
                        }*/

                        MetroTabPage currentTab = (MetroTabPage)CheckBox.SelectedTab;
                        MetroPanel currentPanel = tabPanels.FirstOrDefault(panel => panel.Name == currentTab.Name);

                        MetroCheckBox newCheckBox = new MetroCheckBox();
                        newCheckBox.FontSize = MetroCheckBoxSize.Tall;
                        newCheckBox.FontWeight = MetroCheckBoxWeight.Bold;
                        newCheckBox.Text = value;
                        newCheckBox.Name = "체크박스" + checkboxNum;
                        newCheckBox.Theme = MetroThemeStyle.Dark;
                        newCheckBox.UseSelectable = true;

                        if (currentPanel.Controls.OfType<MetroCheckBox>().Count() == 0) newCheckBox.Location = new Point(15, 10 + newCheckBox.Height);
                        else
                        {
                            MetroCheckBox lastCheckBox = currentPanel.Controls.OfType<MetroCheckBox>().Last();
                            newCheckBox.Location = new Point(lastCheckBox.Left, lastCheckBox.Top + lastCheckBox.Height + 5);
                        }

                        newCheckBox.Size = new Size(57, 25);
                        newCheckBox.AutoSize = true;
                        newCheckBox.CheckedChanged += new EventHandler(criteriaCheckBoxChange);

                        currentPanel.Controls.Add(newCheckBox);

                        MetroButton newButton = new MetroButton();
                        newButton.Text = "...";
                        newButton.Theme = MetroThemeStyle.Dark;
                        newButton.FontSize = MetroButtonSize.Tall;
                        newButton.Name = "버튼" + buttonCheckBoxPairs.Count;
                        newButton.Size = new Size(44, 25);

                        if (currentPanel.Controls.OfType<MetroButton>().Count() == 0) newButton.Location = new Point(228, 10 + newButton.Size.Height);
                        else
                        {
                            MetroButton lastButton = currentPanel.Controls.OfType<MetroButton>().Last();
                            newButton.Location = new Point(lastButton.Left, lastButton.Top + lastButton.Height + 5);
                        }
                        newButton.Click += new EventHandler(criteriaModifyBtn_Click);
                        currentPanel.Controls.Add(newButton);

                        checkboxNum++;
                    }

                    if (i == modifyIniLines.Length - 1 || modifyIniLines[i + 1].StartsWith("["))
                    {
                        sectionNum++;
                        inTargetSection = false;
                    }
                }
            }
        }

        private void RefreshUI(string section, DialogResult result, int index, string[] modifyIniLines, string message)
        {
            if (result == DialogResult.OK)
            {
                DisplayDebugInfo(index.ToString());
                Tuple<MetroButton, MetroCheckBox> pair = buttonCheckBoxPairs[index]; // 클릭한 버튼과 연결된 쌍을 가져옴

                bool inTargetSection = false;
                bool isComplete = false;

                for (int i = 0; i < modifyIniLines.Length; i++)
                {
                    if (modifyIniLines[i].StartsWith($"[{section}]"))
                    {
                        inTargetSection = true;
                    }

                    else if (inTargetSection && !isComplete)
                    {
                        int realIndex = index + 1;
                        string[] keyValue = modifyIniLines[realIndex].Split(new char[] { '=' }, 2);

                        if (keyValue.Length == 2)
                        {
                            string key = keyValue[0].Trim();

                            pair.Item2.Text = message;

                            isComplete = true;
                        }

                        else
                        {
                            inTargetSection = false; // 다음 섹션으로 이동
                        }
                    }
                }
            }

            else if (result == DialogResult.No)
            {
                Tuple<MetroButton, MetroCheckBox> pair = buttonCheckBoxPairs[index]; // 클릭한 버튼과 연결된 쌍을 가져옴

                if (pair.Item2.Name != "CriteriaCheckBox")
                {
                    //판넬0.Controls.Remove(pair.Item1); // 패널에서 버튼 제거
                    //판넬0.Controls.Remove(pair.Item2); // 패널에서 체크 박스 제거
                    pair.Item1.Dispose(); // 버튼 메모리에서 해제
                    pair.Item2.Dispose(); // 체크 박스 메모리에서 해제
                    buttonCheckBoxPairs.RemoveAt(index); // 리스트에서 쌍 제거

                    for (int i = index; i < buttonCheckBoxPairs.Count; i++)
                    {
                        Tuple<MetroButton, MetroCheckBox> pairMove = buttonCheckBoxPairs[i];
                        pairMove.Item1.Top -= (pair.Item1.Height + 5); // 위로 이동
                        pairMove.Item2.Top -= (pair.Item2.Height + 5); // 위로 이동
                    }
                }

            }
        }

        private void LoadCheckboxStates(string[] modifyIniLines)
        {
            string section = "CheckBoxState";
            bool inTargetSection = false;

            for (int i = 0; i < modifyIniLines.Length; i++)
            {
                if (modifyIniLines[i].StartsWith($"[{section}]"))
                    inTargetSection = true;

                else if (inTargetSection)
                {
                    string[] parts = modifyIniLines[i].Split(new char[] { '=' }, 2);

                    if (parts.Length == 2)
                    {
                        var buttonName = parts[0].Trim();
                        var isChecked = bool.Parse(parts[1].Trim());

                        var button = buttonCheckBoxPairs.FirstOrDefault(pair => pair.Item2.Name == buttonName);

                        if (button != null)
                        {
                            button.Item2.Checked = isChecked;
                        }
                        if (isChecked)
                        {
                            newData++;
                        }
                    }

                    else
                        inTargetSection = false;

                }
            }
            double percentage = (double)newData / metroProgressBar1.Maximum * 100;
            metroProgressBar1.Maximum = modifyIniLines.Length - 1;
            metroProgressBar1.Value = (int)percentage;
        }

        private void tabControl_MouseDown(object sender, MouseEventArgs e)
        {
            int lastTapIndex = CheckBox.TabCount - 1;

            if (CheckBox.GetTabRect(lastTapIndex).Contains(e.Location))
            {
                AddTabWithPanel();
            }

            else
            {
                mouseEventArgsForRemove = e;
                RemoveSelectedTabWithPanel();
            }
        }

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == CheckBox.TabCount - 1)
                e.Cancel = true;
        }

        private void tabControl_CustomPaint(object sender, MetroPaintEventArgs e)
        {
            MetroTabControl tabControl = (MetroTabControl)sender;
            Point mousePosition = tabControl.PointToClient(Control.MousePosition);

            for (int i = 0; i < tabControl.TabCount; i++)
            {
                Rectangle tabRect = tabControl.GetTabRect(i);
                tabRect.Inflate(-2, -2);

                if (i == tabControl.TabCount - 1)
                {
                    var addImage = Image.FromFile("Icon\\Add.png");
                    int imageX = tabRect.Left + (tabRect.Width - addImage.Width) / 2;
                    int imageY = tabRect.Top + (tabRect.Height - addImage.Height) / 2;
                    if (i == hoveredTabIndex)
                    {
                        using (SolidBrush brush = new SolidBrush(MetroColors.Blue))
                        {
                            e.Graphics.FillRectangle(brush, tabRect);
                        }

                        using (Pen pen = new Pen(MetroColors.Blue, 2))
                        {
                            e.Graphics.DrawRectangle(pen, tabRect);
                        }
                    }
                    e.Graphics.DrawImage(addImage, imageX, imageY + 2);
                }

                else
                {
                    var closeImage = Image.FromFile("Icon\\IICDM.png");
                    int imageX = tabRect.Right - closeImage.Width;
                    int imageY = tabRect.Top + (tabRect.Height - closeImage.Height) / 2;
                    if (i == hoveredTabIndex)
                    {
                        using (SolidBrush brush = new SolidBrush(Color.FromArgb(25, MetroColors.Blue)))
                        {
                            e.Graphics.FillRectangle(brush, tabRect);
                        }

                        if (mousePosition.X >= imageX && mousePosition.X <= imageX + closeImage.Width && mousePosition.Y >= imageY && mousePosition.Y <= imageY + closeImage.Height)
                        {
                            using (SolidBrush brush = new SolidBrush(MetroColors.Blue))
                            {
                                e.Graphics.FillEllipse(brush, imageX - 2, imageY - 1, closeImage.Width, closeImage.Height);
                            }

                            using (Pen pen = new Pen(MetroColors.Blue, 2))
                            {
                                e.Graphics.DrawEllipse(pen, imageX - 2, imageY - 1, closeImage.Width, closeImage.Height);
                            }
                        }
                    }
                    e.Graphics.DrawImage(closeImage, imageX, imageY + 2);
                }
            }
        }

        private void tabControl_MouseEnter(object sender, EventArgs e)
        {
            MetroTabControl tabControl = (MetroTabControl)sender;
            Point mousePosition = tabControl.PointToClient(Cursor.Position);

            for (int i = 0; i < tabControl.TabCount; i++)
            {
                Rectangle tabRect = tabControl.GetTabRect(i);

                if (tabRect.Contains(mousePosition))
                {
                    hoveredTabIndex = i;
                    tabControl.Invalidate();
                    break;
                }
            }
        }

        private void tabControl_MouseMove(object sender, MouseEventArgs e)
        {
            MetroTabControl tabControl = (MetroTabControl)sender;

            Point mousePosition = e.Location;

            for (int i = 0; i < tabControl.TabCount; i++)
            {
                Rectangle tabRect = tabControl.GetTabRect(i);

                if (tabRect.Contains(mousePosition))
                {
                    hoveredTabIndex = i;
                    tabControl.Invalidate();
                    break;
                }
            }
        }

        private void tabControl_MouseLeave(object sender, EventArgs e)
        {
            hoveredTabIndex = -1;
            MetroTabControl tabControl = (MetroTabControl)sender;
            tabControl.Invalidate();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 현재 선택된 탭의 인덱스
            int selectedTabIndex = CheckBox.SelectedIndex;

            // 선택된 탭에 해당하는 판넬을 보여주도록 설정
            ShowSelectedPanel(selectedTabIndex);
        }

        private void AddTabWithPanel()
        {
            CustomMessageBox messageBox = new CustomMessageBox("addBoxBtn");
            DialogResult result = messageBox.ShowDialog();
            buttonText = messageBox.GetInputText();
            int lastTapIndex = CheckBox.TabCount - 1;

            if (result == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(buttonText))
                {
                    MetroTabPage newTab = new MetroTabPage();
                    newTab.Text = buttonText;
                    newTab.Name = buttonText;
                    CheckBox.TabPages.Insert(lastTapIndex, newTab);
                    CheckBox.SelectedIndex = lastTapIndex;

                    MetroPanel newPanel = new MetroPanel();
                    newPanel.Dock = DockStyle.Fill;
                    newPanel.Name = "판넬" + CheckBox.SelectedIndex;
                    newPanel.Theme = MetroThemeStyle.Dark;
                    newPanel.Location = new Point(23, 88);
                    newPanel.Size = new Size(287, 501);
                    tabPanels.Add(newPanel);

                    Controls.Add(newPanel);

                    CheckBox.SelectedTab = CheckBox.TabPages[CheckBox.TabPages.Count - 1];
                }
            }
        }

        private void RemoveSelectedTabWithPanel()
        {
            for (var i = 0; i < CheckBox.TabPages.Count; i++)
            {
                var tabRect = CheckBox.GetTabRect(i);
                tabRect.Inflate(-2, -2);
                var closeImage = Image.FromFile("Icon\\IICDM.png");
                var imageRect = new Rectangle((tabRect.Right - closeImage.Width), tabRect.Top + (tabRect.Height - closeImage.Height) / 2, closeImage.Width, closeImage.Height);

                if (imageRect.Contains(mouseEventArgsForRemove.Location))
                {
                    CheckBox.TabPages.RemoveAt(i);
                    Panel panelToRemove = tabPanels[i];
                    tabPanels.RemoveAt(i);
                    panelToRemove.Dispose(); // 패널 제거
                    break;
                }
            }
        }

        private void ShowSelectedPanel(int selectedTabIndex)
        {
            for (int i = 0; i < tabPanels.Count; i++)
            {
                if (i == selectedTabIndex)
                {
                    // 선택한 탭의 판넬은 Visible을 true로 설정
                    tabPanels[i].Visible = true;
                }
                else
                {
                    // 나머지 판넬들은 Visible을 false로 설정
                    tabPanels[i].Visible = false;
                }
            }
        }
        private void DisplayDebugInfo(string debugMessage)
        {
            debugTextBox.AppendText(debugMessage + "\r\n");
        }

    }
}
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Collections.Specialized.BitVector32;
using System.Reflection;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using IniParser;
using IniParser.Model;
using System.Windows.Forms.VisualStyles;

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

        string strCheckFolder = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
        string strCheckFolder2 = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
        List<Tuple<MetroButton, MetroCheckBox>> buttonCheckBoxPairs = new List<Tuple<MetroButton, MetroCheckBox>>();
        private FileIniDataParser iniParser = new FileIniDataParser();

        private static readonly string _startupRegPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        string buttonText = "";
        int newData = 0;

        public TD()
        {
            InitializeComponent();
            CreateIni("CheckBoxData");
            CreateIni("CheckBoxState");
        }

        private void TD_Load(object sender, EventArgs e)
        {
            AddStartupProgram("TD", Application.ExecutablePath);

            strCheckFolder += "\\INI\\CheckBoxData.ini";
            strCheckFolder2 += "\\INI\\CheckBoxState.ini";
            IniData iniData = iniParser.ReadFile(strCheckFolder, EUCKREncoding());
            KeyDataCollection keyDatas = iniData["CheckBox"];
            string[] modifyIniLines = File.ReadAllLines(strCheckFolder, EUCKREncoding());
            string[] modifyIniLines2 = File.ReadAllLines(strCheckFolder2, EUCKREncoding());

            buttonCheckBoxPairs.Add(new Tuple<MetroButton, MetroCheckBox>(criteriaModifyBtn, criteriaCheckBox));
            

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

        private void addBoxBtn_Click(object sender, EventArgs e)
        {
            InitializeNewList();
            int changePosition = checkBoxPanel.VerticalScroll.Value + checkBoxPanel.VerticalScroll.SmallChange * 30;
            checkBoxPanel.AutoScrollPosition = new Point(0, changePosition);
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
                    Tuple<MetroButton, MetroCheckBox> lastPair = buttonCheckBoxPairs[buttonCheckBoxPairs.Count - 1];

                    MetroCheckBox newCheckBox = new MetroCheckBox();
                    newCheckBox.FontSize = MetroFramework.MetroCheckBoxSize.Tall;
                    newCheckBox.FontWeight = MetroFramework.MetroCheckBoxWeight.Bold;
                    newCheckBox.Text = buttonText;
                    newCheckBox.Name = "체크박스" + buttonCheckBoxPairs.Count;
                    newCheckBox.Theme = MetroFramework.MetroThemeStyle.Dark;
                    newCheckBox.UseSelectable = true;
                    newCheckBox.Location = new System.Drawing.Point(lastPair.Item2.Left, lastPair.Item2.Top + lastPair.Item2.Height + 5);
                    newCheckBox.Size = new Size(criteriaCheckBox.Size.Width, criteriaCheckBox.Size.Height);
                    newCheckBox.AutoSize = true;
                    newCheckBox.CheckedChanged += new System.EventHandler(this.criteriaCheckBoxChange);

                    checkBoxPanel.Controls.Add(newCheckBox);

                    MetroButton newButton = new MetroButton();
                    newButton.Text = "...";
                    newButton.Theme = MetroFramework.MetroThemeStyle.Dark;
                    newButton.FontSize = MetroFramework.MetroButtonSize.Tall;
                    newButton.Name = "버튼" + buttonCheckBoxPairs.Count;
                    newButton.Size = new Size(criteriaModifyBtn.Size.Width, criteriaModifyBtn.Size.Height);
                    newButton.Location = new System.Drawing.Point(lastPair.Item1.Left, lastPair.Item1.Top + lastPair.Item1.Height + 5);
                    newButton.Click += new System.EventHandler(this.criteriaModifyBtn_Click);

                    checkBoxPanel.Controls.Add(newButton);
                    buttonCheckBoxPairs.Add(new Tuple<MetroButton, MetroCheckBox>(newButton, newCheckBox));

                    setIni("CheckBox", newCheckBox.Name, buttonText, strCheckFolder);
                    setIni("CheckBoxState", newCheckBox.Name, "False", strCheckFolder2);
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

            DialogResult result = messageBox.ShowDialog();
            buttonText = messageBox.GetInputText();
            bool inTargetSection = false, isComplete = false, isFirst = false, isFirst2 = false;
            string[] modifyIniLines = File.ReadAllLines(strCheckFolder, EUCKREncoding());
            string[] modifyIniLines2 = File.ReadAllLines(strCheckFolder2, EUCKREncoding());
            string section = "CheckBox"; // 섹션 이름
            string section2 = "CheckBoxState"; // 섹션 이름
            string checkRTK = "";
            int pairIndex = buttonCheckBoxPairs.FindIndex(pair => pair.Item1 == clickedButton);

            if (result == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(buttonText))
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
                                string key = keyValue[0].Trim();

                                setIni("CheckBox", key, buttonText, strCheckFolder);
                                isComplete = true;
                            }
                        }

                        else
                        {
                            inTargetSection = false; // 다음 섹션으로 이동
                        }
                    }
                    RefreshUI(section, result, pairIndex, modifyIniLines, buttonText);
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
            string section = "CheckBox";
            IniData iniData = iniParser.ReadFile(strCheckFolder, EUCKREncoding());
            KeyDataCollection sectionData = new KeyDataCollection();
            
            bool inTargetSection = false, iAmFirst = false;

            #region 섹션데이터에 키값 미리 추가
            for (int i = 0; i < modifyIniLines.Length; i++)
            {
                if (modifyIniLines[i].StartsWith($"[{section}]"))
                {
                    inTargetSection = true;
                    iAmFirst = true;
                }

                else if (inTargetSection)
                {
                    string[] keyValue = modifyIniLines[i].Split(new char[] { '=' }, 2);


                    if (keyValue.Length == 2)
                    {
                        string value = keyValue[1].Trim();

                        if (iAmFirst)
                        {
                            sectionData.AddKey("CriteriaCheckBox", value);
                            iAmFirst = false;
                        }
                        else
                        {
                            sectionData.AddKey("체크박스" + (i - 1), value);
                        }
                    }
                }

                else
                    inTargetSection = false;
            }

            #endregion
            
            for (int i = 0; i < modifyIniLines.Length; i++)
            {
                if (modifyIniLines[i].StartsWith($"[{section}]"))
                {
                    inTargetSection = true; // 목표 섹션 내부에 있다는 플래그를 설정
                    iAmFirst = true;
                }

                else if (inTargetSection)
                {
                    // 현재 섹션 내부에 있는 키-값 쌍을 처리
                    string[] keyValue = modifyIniLines[i].Split(new char[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();
                        string value = keyValue[1].Trim();

                        if (iAmFirst)
                        {
                            this.criteriaCheckBox.Text = value;
                            this.criteriaCheckBox.Name = "CriteriaCheckBox";

                            if (key != "CriteriaCheckBox")
                            {
                                iniData.Sections.RemoveSection(section);

                                iniParser.WriteFile(strCheckFolder, iniData, Encoding.Default);

                                for (int k = 0; k < sectionData.Count; k++)
                                {
                                    if (k == 0)
                                    {
                                        KeyData keyData = sectionData.GetKeyData("CriteriaCheckBox");
                                        string stringkey = keyData.Value.Trim();
                                        setIni(section, "CriteriaCheckBox", stringkey, strCheckFolder);
                                    }
                                    
                                    else
                                    {
                                        KeyData keyData = sectionData.GetKeyData("체크박스" + k);
                                        string stringkey = keyData.Value.Trim();
                                        setIni(section, "체크박스" + k, stringkey, strCheckFolder);
                                    }
                                }
                            }
                            iAmFirst = false;
                        }

                        else
                        {
                            Tuple<MetroButton, MetroCheckBox> lastPair = buttonCheckBoxPairs[buttonCheckBoxPairs.Count - 1];

                            MetroCheckBox newCheckBox = new MetroCheckBox();
                            newCheckBox.FontSize = MetroFramework.MetroCheckBoxSize.Tall;
                            newCheckBox.FontWeight = MetroFramework.MetroCheckBoxWeight.Bold;
                            newCheckBox.Name = key;
                            newCheckBox.Text = value;
                            newCheckBox.Theme = MetroFramework.MetroThemeStyle.Dark;
                            newCheckBox.UseSelectable = true;
                            newCheckBox.Location = new System.Drawing.Point(lastPair.Item2.Left, lastPair.Item2.Top + lastPair.Item2.Height + 5);
                            newCheckBox.Size = new Size(criteriaCheckBox.Size.Width, criteriaCheckBox.Size.Height);
                            newCheckBox.CheckedChanged += new System.EventHandler(this.criteriaCheckBoxChange);
                            newCheckBox.AutoSize = true;

                            checkBoxPanel.Controls.Add(newCheckBox);

                            MetroButton newButton = new MetroButton();
                            newButton.Text = "...";
                            newButton.Theme = MetroFramework.MetroThemeStyle.Dark;
                            newButton.FontSize = MetroFramework.MetroButtonSize.Tall;
                            newButton.Size = new Size(criteriaModifyBtn.Size.Width, criteriaModifyBtn.Size.Height);
                            newButton.Location = new System.Drawing.Point(lastPair.Item1.Left, lastPair.Item1.Top + lastPair.Item1.Height + 5);
                            newButton.Click += new System.EventHandler(this.criteriaModifyBtn_Click);

                            checkBoxPanel.Controls.Add(newButton);

                            buttonCheckBoxPairs.Add(new Tuple<MetroButton, MetroCheckBox>(newButton, newCheckBox));
                        }
                    }

                    else
                    {
                        inTargetSection = false; // 다음 섹션으로 이동
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
           
            else if(result == DialogResult.No)
            { 
                Tuple<MetroButton, MetroCheckBox> pair = buttonCheckBoxPairs[index]; // 클릭한 버튼과 연결된 쌍을 가져옴

                if (pair.Item2.Name != "CriteriaCheckBox")
                {
                    checkBoxPanel.Controls.Remove(pair.Item1); // 패널에서 버튼 제거
                    checkBoxPanel.Controls.Remove(pair.Item2); // 패널에서 체크 박스 제거
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

        private void DisplayDebugInfo(string debugMessage)
        {
            debugTextBox.AppendText(debugMessage + "\r\n");
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
                        if(isChecked)
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
        //private void DisplayDebugInfo(string debugMessage)
        //{
        //    for (int i = 0; i < modifyIniLines.Length; i++)
        //    {
        //        if (modifyIniLines[i].StartsWith($"[{section}]"))
        //             inTargetSection = true;


        //        else if (inTargetSection)
        //        {
        //            string[] keyValue = modifyIniLines[i].Split(new char[] { '=' }, 2);

        //            if (keyValue.Length == 2)
        //            {
        //                string key = keyValue[0].Trim();
        //                string value = keyValue[1].Trim();
        //            }

        //            else

        //                inTargetSection = false;

        //        }
        //    }
        //}
    }
}

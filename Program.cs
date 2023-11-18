using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentPick
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }

    public static class runtime
    {
        public static string datafileloc = "data.txt";
        public static string fulldataraw;
        public static List<Student> allstudents = new List<Student>();
        public static bool AppDataIsLatest(string aad)
        {
            return String.Equals(fulldataraw, aad);
        }
        public static string LoadData()
        {
            if (!File.Exists(datafileloc))
            {
                fulldataraw = "";
            } else
            {
                fulldataraw = File.ReadAllText(datafileloc);
            }
            if (AppDataIsOld(fulldataraw) || fulldataraw.Equals(""))
            {
                if (!fulldataraw.Equals(""))
                {

                    MessageBox.Show("StudentPick has changed to a new App Data format. After you press OK, your data will be migrated to this new format", "Appdata upgrade", MessageBoxButtons.OK, MessageBoxIcon.Information);
                } else
                {
                    MessageBox.Show("Hello! Welcome to StudentPick. After you press OK, you will be asked to choose a class name. After you answer that initial prompt, you can add more classes and students at your discretion", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                OldAppdataUpgrade o = new OldAppdataUpgrade();
                if (fulldataraw.Equals(""))
                {
                    o.Text = "Set a class name";
                }
                DialogResult d = o.ShowDialog();
                string nname = o.name;
                //MessageBox.Show(fulldataraw);
                foreach (string sss in fulldataraw.Replace('\r','\n').Split('\n'))
                {
                    if (sss.Equals(""))
                    {
                        continue;//Skip empty names
                    }
                    string[] ssp = sss.Split(';');
                    Student student = new Student(ssp[0].Trim().Replace(" ", ""));
                    student.ClassName = nname;
                    allstudents.Add(student);
                }
                fulldataraw = "";
                //Write with new data
                SaveData();
                return nname;

            }
            else
            {
                foreach (string line in fulldataraw.Split('\n'))
                {
                    if (line.Replace(" ", "").Replace("\r", "").Equals(""))
                    {
                        continue;
                    }
                    allstudents.Add(Student.FromDataString(line));
                }
            }
            return "";
        }
        public static void SaveData()
        {
            fulldataraw = "";
            foreach (Student s in allstudents)
            {
                fulldataraw += s.OutToDataFile() + "\n";
            }
            File.WriteAllText(datafileloc, fulldataraw);
        }
        public static bool AppDataIsOld(string data)
        {
            if (data.Split('\n')[0].Split(';').Length < 3)
            {
                return true;
            }
            return false;
        }
        public static int BoolToInt(bool value)
        {
            if (value)
            {
                return 1;
            } else
            {
                return 0;
            }
        }
        public static bool IntToBool(int value)
        {
            if (value == 0)
            {
                return false;
            } else
            {
                return true;
            }
        }
    }
    public class Student
    {
        public string Name { get; set; }
        public bool ishidden = false;
        public string ClassName { get; set; }
        public Student (string name)
        {
            Name = name;
        }
        public string OutToDataFile()
        {
            return $"{Name};{runtime.BoolToInt(ishidden).ToString()};{ClassName}";
        }
        public void Hide()
        {
            ishidden = true;
        }
        public void Show()
        {
            ishidden = false;
        }
        public static Student FromDataString(string s)
        {
            string[] split = s.Split(';');
            Student z = new Student(split[0]);
            z.ishidden = runtime.IntToBool(int.Parse(split[1]));
            try
            {
                z.ClassName = split[2];
            } catch
            {
                //Do absolutely nothing
            }
            return z;
        }
    }
}

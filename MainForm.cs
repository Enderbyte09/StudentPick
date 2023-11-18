using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentPick
{
    public partial class MainForm : Form
    {
        string ActiveClass;
        public MainForm()
        {
            InitializeComponent();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void updateslist()
        {
            foreach (Student s in runtime.allstudents)
            {
                //MessageBox.Show($"{s.Name} {s.ClassName} {ActiveClass}");
                if (s.ishidden || s.ClassName != ActiveClass)
                {
                    continue;
                }
                listBox1.Items.Add(s.Name);
            }
        }
        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string n = "";
            foreach (Student s in runtime.allstudents)
            {
                if (checkBox4.Checked)
                {
                    s.ishidden = false;
                }
                n += s.OutToDataFile() + "\n";
            }
            if (!runtime.AppDataIsLatest(n))
            {
                DialogResult d = MessageBox.Show("You have made changes to this program's configuration without saving. Do you want to save before exiting?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (d == DialogResult.Yes)
                {
                    runtime.fulldataraw = n;
                    runtime.SaveData();
                    e.Cancel = false;
                } else if (d == DialogResult.Cancel)
                {
                    e.Cancel = true;
                } else if (d == DialogResult.No)
                {
                    e.Cancel = false;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string iio = runtime.LoadData();
            
            listBox1.Items.Clear();

            this.FormClosing += Form1_Closing;
            this.ControlBox = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ShowIcon = true;
            //checkBox2.Checked = true;

            //Now populate class list
            comboBox1.Items.Clear();
            if (iio.Length > 0)
            {
                comboBox1.Items.Add(iio);
            }
            foreach (Student s in runtime.allstudents)
            {
                if (!comboBox1.Items.Contains(s.ClassName))
                {
                    comboBox1.Items.Add(s.ClassName);
                }
            }
            comboBox1.SelectedIndex = 0;
            ActiveClass = comboBox1.SelectedItem.ToString();
            listBox1.Items.Clear();
            updateslist();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1)
            {
                MessageBox.Show("Please add at least one student","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); return;
            }
            Random r = new Random();
            int chosen = r.Next(0, listBox1.Items.Count);
            string cname = listBox1.Items[chosen].ToString();
            
            label1.Text = cname;
            if (checkBox3.Checked)
            {
                Random rnd = r;
                Color randomColor;
                while (true)
                {
                    randomColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
                    if ((randomColor.R + randomColor.G + randomColor.B) / 3 < 200)
                    {
                        break;
                    }
                }

                label1.ForeColor = randomColor;
            } else
            {
                label1.ForeColor = Color.FromArgb(0, 0, 0);
            }
            if (checkBox2.Checked)
            {
                MessageBox.Show(cname, "Chosen student");
            }
            if (checkBox1.Checked)
            {
                listBox1.Items.RemoveAt(chosen);
                runtime.allstudents[chosen].ishidden = true;

            }
            }
        

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (Student s in runtime.allstudents)
            {
                s.ishidden = false;
                if (s.ClassName != ActiveClass)
                {
                    continue;
                }
                listBox1.Items.Add(s.Name);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NewStudentForm f2 = new NewStudentForm();
            DialogResult d = f2.ShowDialog();
            if (d == DialogResult.OK)
            {
                string[] dn = f2.name.Split(',');
                foreach (string n in dn) {
                    Student ns = Student.FromDataString(n + $";0");
                    ns.ClassName = ActiveClass;
                    runtime.allstudents.Add(ns);
                }
                //MessageBox.Show("Student added", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listBox1.Items.Clear();
                updateslist();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            runtime.SaveData();
        }


        private void button4_Click(object sender, EventArgs e)
        {
            List<string> ntr = new List<string>();
            foreach (string sel in listBox1.SelectedItems)
            {
                ntr.Add(sel);
            }

            List<Student> ts = new List<Student>();

            foreach (Student student in runtime.allstudents)
            {
                if (ntr.Contains(student.Name))
                {
                    continue;
                } else
                {
                    ts.Add(student);
                }
            }

            runtime.allstudents = ts;

            listBox1.Items.Clear();
            updateslist();

        }
        private void button9_Click(object sender, EventArgs e)
        {
            Process p = new Process();
            p.StartInfo.FileName = "documentation.html";
            p.StartInfo.UseShellExecute = true;
            p.Start();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClassManager c = new ClassManager();
            List<string> nnn = new List<string>();
            Student[] stee = new Student[runtime.allstudents.Count];
            
            runtime.allstudents.CopyTo(stee);
            List < Student > slist = stee.ToList();
            foreach (string sssssss in comboBox1.Items)
            {
                nnn.Add(sssssss);
            }
            c.PopulateList(nnn.ToArray());
            c.ShowDialog();
            foreach (string s in c.toadd)
            {
                comboBox1.Items.Add(s);
            }
            List<Student> namestodelete = new List<Student>();
            foreach (KeyValuePair<string,string> entry in c.toModify)
            {
                comboBox1.Items.Remove(entry.Key);
                comboBox1.Items.Add(entry.Value);
                foreach (Student st in slist)
                {
                    if (st.ClassName.Equals(entry.Key))
                    {
                        st.ClassName = entry.Value;
                    }
                }
            }
            foreach (string s2 in c.todelete)
            {
                comboBox1.Items.Remove(s2);
                foreach (Student st in slist)
                {
                    if (st.ClassName.Equals(s2))
                    {
                        namestodelete.Add(st);
                    }
                }
            }
            foreach (Student td in namestodelete)
            {
                slist.Remove(td);
            }
            runtime.allstudents = slist;
            listBox1.Items.Clear();
            updateslist();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveClass = comboBox1.SelectedItem.ToString();
            listBox1.Items.Clear();
            updateslist();
        }
    }
}

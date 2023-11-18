﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
            runtime.LoadData();
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
            foreach (Student s in runtime.allstudents)
            {
                if (!comboBox1.Items.Contains(s.ClassName))
                {
                    comboBox1.Items.Add(s.ClassName);
                }
            }
            comboBox1.SelectedIndex = 0;
            ActiveClass = comboBox1.SelectedItem.ToString();
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
                    runtime.allstudents.Add(Student.FromDataString(n+";0"));
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
            MessageBox.Show("StudentPick by Enderbyte Programs\n\nHELP MENU\n" +
                "\n" +
                "All data for this program is stored in data.txt which must exist in the same directory as the program\n" +
                "BUTTONS:\n" +
                "Generate - Choose a random student from the list and show it in a message box\n" +
                "About - Show some info about this software\n" +
                "Help - Display this help menu\n" +
                "Quit - Quit the software\n" +
                "Reset session - Reset the list of names to its full capacity\n" +
                "\n" +
                "Add - Add one or more students to the list\n" +
                "Remove - Remove selected students from the list\n" +
                "Save - Commit changes in the student list to data.txt\n\n" +
                "Allow only one answer per student - Selected students will be temporarily removed from the list. Press Reset Session to return the names\n" +
                "Pop out student name - Student's name will be shown as a message box.\n" +
                "Colourful Names - Generated names will be set to a random colour. Disable if you are having trouble reading.");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClassManager c = new ClassManager();
            c.ShowDialog();
            
        }
    }
}
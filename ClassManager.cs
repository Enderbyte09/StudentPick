using System;
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
    public partial class ClassManager : Form
    {
        public List<string> toadd = new List<string>();
        public Dictionary<string,string> toModify = new Dictionary<string,string>();
        public List<string> todelete = new List<string>();
        public ClassManager()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OldAppdataUpgrade o = new OldAppdataUpgrade();
            DialogResult d = o.ShowDialog();
            string nname = o.name;
            listBox1.Items.Add(nname);
            toadd.Add(nname);
            
        }
        public void PopulateList(string[] items)
        {
            foreach (string s in items)
            {
                listBox1.Items.Add(s);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string oldname = listBox1.SelectedItem.ToString();
            OldAppdataUpgrade o = new OldAppdataUpgrade();
            o.Text = "Edit name";
            
            DialogResult d = o.ShowDialog();
            string nname = o.name;
            listBox1.Items.Remove(oldname);
            listBox1.Items.Add(nname);
            toModify.Add(oldname, nname);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("This will delete all names in this class. Are you sure you want to delete the class?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (listBox1.SelectedItem == null)
                {
                    return;
                }
                todelete.Add(listBox1.SelectedItem.ToString());
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

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
        List<string> toadd = new List<string>();
        Dictionary<string,string> toModify = new Dictionary<string,string>();
        List<string> todelete = new List<string>();
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

        private void button2_Click(object sender, EventArgs e)
        {
            string oldname = listBox1.SelectedItem.ToString();
            OldAppdataUpgrade o = new OldAppdataUpgrade();
            o.Name = "Edit name";
            DialogResult d = o.ShowDialog();
            string nname = o.name;
            toModify.Add(oldname, nname);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            todelete.Add(listBox1.SelectedItem.ToString());
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

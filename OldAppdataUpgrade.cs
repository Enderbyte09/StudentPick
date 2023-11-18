using System;
using System.Windows.Forms;

namespace StudentPick
{
    public partial class OldAppdataUpgrade : Form
    {
        public string name;
        public OldAppdataUpgrade()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (name.Contains(";"))
            {
                MessageBox.Show("Names may not contain semicolons.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } else
            {
                this.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            name = textBox1.Text;
        }
        public void ChangeTitle(string title)
        {
            this.Name = title;
        }
    }
}

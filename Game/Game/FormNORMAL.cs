using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game
{
    public partial class FormNORMAL : Form
    {
        private Form1 mainForm;
        public FormNORMAL(Form1 mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mainForm.Show(); // 메인 폼 다시 표시
            this.Close();
        }
    }
}

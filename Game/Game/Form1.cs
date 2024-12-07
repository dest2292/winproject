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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(); //  폼2 객체 생성
            form2.Show(); // 폼2 표시
            this.Hide(); // 폼1 숨기기
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit(); // 프로그램 종료
        }
    }
}

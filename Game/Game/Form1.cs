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
            InitializeComboBoxItems();
        }

        private void InitializeComboBoxItems()
        {
            // ComboBox에 레벨 항목을 추가
            comboBoxLEVEL.Items.Clear();
            comboBoxLEVEL.Items.Add("EASY");
            comboBoxLEVEL.Items.Add("NORMAL");
            comboBoxLEVEL.Items.Add("HARD");
            comboBoxLEVEL.SelectedIndex = 0; // 기본 선택값 설정
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string selectedLEVEL = comboBoxLEVEL.SelectedItem.ToString();

            // 난이도에 따라 다른 폼 열기
            Form gameForm;

            if (selectedLEVEL == "EASY")
            {
                gameForm = new FormEASY();
            }
            else if (selectedLEVEL == "NORMAL")
            {
                gameForm = new FormNORMAL();
            }
            else // "어려움"
            {
                gameForm = new FormHARD();
            }
            if (gameForm != null)
            {
                gameForm.Show();
                this.Hide(); // 현재 폼 숨기기
                             // 선택된 폼을 모달로 표시
                gameForm.Show();
                this.Hide(); // 현재 폼 숨기기 (선택 사항)
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit(); // 프로그램 종료
        }
    }
}

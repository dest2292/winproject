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
    public partial class FormHARD : Form
    {
        private Form1 mainForm;
        private List<PictureBox> bullets = new List<PictureBox>(); // 여러 개의 총알 관리
        private Timer bulletTimer; // 총알 이동 타이머
        private Timer scoreTimer; // 점수 타이머
        private int bulletSpeed = 10; // 총알 속도
        private int bulletCount = 12; // 총알 개수
        private int score = 0; // 점수 (초 단위)
        private int playerSpeed = 10; // 캐릭터 이동 속도
        private bool isGameOver = false;

        public FormHARD(Form1 mainform)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            player.BackColor = Color.Transparent; // 플레이어의 배경을 투명하게 설정
            bullet.BackColor = Color.Transparent; // 총알의 배경을 투명하게 설정
            this.DoubleBuffered = true; // 더블버퍼

            bullet.Visible = false; // 원본 PictureBox 숨기기
            InitializeBullets(); // 총알 초기화
            InitializeTimers(); // 타이머 초기화
        }

        private void InitializeBullets()
        {
            Random rand = new Random();

            for (int i = 0; i < bulletCount; i++)
            {
                // 기존 bullet PictureBox를 복제하여 여러 개 생성
                PictureBox newBullet = new PictureBox
                {
                    Size = bullet.Size, // 기존 bullet 크기 사용
                    Image = bullet.Image, // 총알 이미지 설정
                    SizeMode = bullet.SizeMode, // 이미지 맞춤
                    Top = rand.Next(-500, -50), // 랜덤 위치에서 시작
                    Left = rand.Next(0, this.ClientSize.Width - bullet.Width),
                    BackColor = Color.Transparent
                };

                bullets.Add(newBullet); // 리스트에 추가
                this.Controls.Add(newBullet); // 폼에 추가
            }
            Console.WriteLine($"생성된 총알 개수: {bullets.Count}");
        }

        private void InitializeTimers()
        {
            // 총알 이동 타이머
            bulletTimer = new Timer
            {
                Interval = 50 // 총알 이동 간격 (ms)
            };
            bulletTimer.Tick += BulletTimer_Tick;
            bulletTimer.Start();

            Console.WriteLine("타이머 시작");
            // 점수 타이머
            scoreTimer = new Timer
            {
                Interval = 1000 // 1초마다 점수 증가
            };
            scoreTimer.Tick += ScoreTimer_Tick;
            scoreTimer.Start();
        }

        private void BulletTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("BulletTimer_Tick 호출됨");

            foreach (var bullet in bullets)
            {
                // 총알 이동
                bullet.Top += bulletSpeed;

                // 총알이 화면 아래로 벗어나면 다시 위로 초기화
                if (bullet.Top > this.ClientSize.Height)
                {
                    bullet.Top = -50; // 랜덤 위치에서 다시 시작
                    bullet.Left = new Random().Next(0, this.ClientSize.Width - bullet.Width);
                    Console.WriteLine($"Bullet reset: {bullet.Left}, {bullet.Top}");
                }

                // 충돌 감지
                if (!isGameOver && player.Bounds.IntersectsWith(bullet.Bounds))
                {
                    GameOver();
                    return;
                }
            }
        }
        private void ScoreTimer_Tick(object sender, EventArgs e)
        {
            if (!isGameOver)
            {
                score++; // 1초마다 점수 증가
                this.Text = $"점수: {score}초"; // 점수를 폼 제목에 표시
            }
        }
        private void GameOver()
        {
            if (isGameOver) return; // 중복 호출 방지
            isGameOver = true;

            bulletTimer.Stop(); // 총알 타이머 중지
            scoreTimer.Stop(); // 점수 타이머 중지

            MessageBox.Show($"게임 오버! 당신의 점수: {score}초", "게임 종료");
            mainForm.Show(); // 메인 폼 다시 표시
            this.Close(); // 현재 폼 닫기
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left && player.Left > 0)
            {
                player.Left -= playerSpeed; // 왼쪽 이동
            }
            else if (keyData == Keys.Right && player.Right < this.ClientSize.Width)
            {
                player.Left += playerSpeed; // 오른쪽 이동
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GameOver(); // GameOver 호출
        }
    }
}

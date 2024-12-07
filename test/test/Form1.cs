using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form1 : Form
    {
        private Bitmap playerImage;
        private Bitmap bulletImage;
        private Point playerPosition;
        private Point bulletPosition;
        private Timer gameTimer;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            this.Text = "Transparent Overlap Fix";
            this.Size = new Size(400, 600);

            // 이미지 로드
            playerImage = new Bitmap(@"C:\Users\김민석\Desktop\player.png"); // 경로 변경
            bulletImage = new Bitmap(@"C:\Users\김민석\Desktop\bullet.png"); // 경로 변경

            // 초기 위치 설정
            playerPosition = new Point(175, 500); // 화면 하단
            bulletPosition = new Point(190, 0);   // 화면 상단

            // Timer 설정
            gameTimer = new Timer
            {
                Interval = 50
            };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            // 키보드 입력 설정
            this.KeyDown += Form1_KeyDown;
            this.DoubleBuffered = true; // 화면 깜빡임 방지
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // Bullet 움직임
            bulletPosition.Y += 5;

            // Bullet이 화면 아래로 벗어나면 다시 위로 초기화
            if (bulletPosition.Y > this.ClientSize.Height)
            {
                bulletPosition.Y = 0;
                bulletPosition.X = new Random().Next(0, this.ClientSize.Width - bulletImage.Width);
            }

            // 충돌 감지
            if (IsPixelPerfectCollision())
            {
                gameTimer.Stop();
                MessageBox.Show("Game Over! Bullet hit the player.");
                ResetGame();
            }

            // 화면 다시 그리기
            this.Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            const int playerSpeed = 10;

            if (e.KeyCode == Keys.Left && playerPosition.X > 0)
            {
                playerPosition.X -= playerSpeed;
            }
            else if (e.KeyCode == Keys.Right && playerPosition.X < this.ClientSize.Width - playerImage.Width)
            {
                playerPosition.X += playerSpeed;
            }

            // 화면 다시 그리기
            this.Invalidate();
        }

        private void ResetGame()
        {
            playerPosition = new Point(175, 500);
            bulletPosition = new Point(190, 0);
            gameTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Graphics 객체를 사용하여 불투명한 픽셀만 그림
            DrawImageWithoutTransparency(e.Graphics, playerImage, playerPosition);
            DrawImageWithoutTransparency(e.Graphics, bulletImage, bulletPosition);
        }

        private void DrawImageWithoutTransparency(Graphics g, Bitmap image, Point position)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Color pixelColor = image.GetPixel(x, y);
                    if (pixelColor.A > 0) // 투명하지 않은 픽셀만 그림
                    {
                        using (Brush brush = new SolidBrush(pixelColor))
                        {
                            g.FillRectangle(brush, position.X + x, position.Y + y, 1, 1);
                        }
                    }
                }
            }
        }

        private bool IsPixelPerfectCollision()
        {
            // 두 이미지 간의 교차 영역(Rectangle) 계산
            Rectangle rect1 = new Rectangle(playerPosition, playerImage.Size);
            Rectangle rect2 = new Rectangle(bulletPosition, bulletImage.Size);
            Rectangle intersect = Rectangle.Intersect(rect1, rect2);

            // 교차 영역이 없으면 충돌하지 않음
            if (intersect.IsEmpty) return false;

            // 교차 영역에서 픽셀 단위 비교
            for (int y = intersect.Top; y < intersect.Bottom; y++)
            {
                for (int x = intersect.Left; x < intersect.Right; x++)
                {
                    // 두 이미지의 상대적 좌표 계산
                    int bmp1X = x - rect1.Left;
                    int bmp1Y = y - rect1.Top;
                    int bmp2X = x - rect2.Left;
                    int bmp2Y = y - rect2.Top;

                    // 픽셀 색상 가져오기
                    Color color1 = playerImage.GetPixel(bmp1X, bmp1Y);
                    Color color2 = bulletImage.GetPixel(bmp2X, bmp2Y);

                    // 투명하지 않은 픽셀끼리 충돌하면 true 반환
                    if (color1.A > 0 && color2.A > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

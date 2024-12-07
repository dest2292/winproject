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
        private Point playerPosition;
        private Size playerSize = new Size(74, 79);

        private List<Bullet> bullets; // Bullet 리스트
        private Timer gameTimer;
        private Random random;

        public Form1()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            // 리소스에서 Player 이미지 로드
            playerImage = new Bitmap(Properties.Resources.player);
            playerImage = new Bitmap(playerImage, playerSize); // 크기 조정
            playerPosition = new Point(307, 280);

            // Bullet 리스트 초기화
            bullets = new List<Bullet>();
            random = new Random();

            // Timer 설정
            gameTimer = new Timer { Interval = 50 };
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();

            // 키보드 입력 설정
            this.KeyDown += Form1_KeyDown;
            this.DoubleBuffered = true; // 화면 깜빡임 방지

            // 초기 Bullet 추가
            AddBullet();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            // Bullet 이동
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Move();

                // Bullet이 화면 아래로 벗어나면 삭제
                if (bullets[i].Position.Y > this.ClientSize.Height)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }

            // 새로운 Bullet 주기적으로 생성
            if (random.Next(0, 10) < 2) // 약 20% 확률로 생성
            {
                AddBullet();
            }

            // 충돌 감지
            foreach (var bullet in bullets)
            {
                if (IsPixelPerfectCollision(playerImage, new Rectangle(playerPosition, playerSize), bullet.Image, bullet.GetBounds()))
                {
                    gameTimer.Stop();
                    MessageBox.Show("Game Over! Bullet hit the player.");
                    ResetGame();
                    return;
                }
            }

            // 화면 다시 그리기
            this.Invalidate();
        }

        private void AddBullet()
        {
            // Bullet 생성
            Bitmap bulletImage = new Bitmap(Properties.Resources.bullet);
            bulletImage = new Bitmap(bulletImage, new Size(15, 40)); // 크기 조정
            Point position = new Point(random.Next(0, this.ClientSize.Width - bulletImage.Width), 0); // 랜덤 위치
            int speed = random.Next(5, 10); // 랜덤 속도

            bullets.Add(new Bullet(position, bulletImage.Size, speed, bulletImage));
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            const int playerSpeed = 10;

            if (e.KeyCode == Keys.Left && playerPosition.X > 0)
            {
                playerPosition.X -= playerSpeed;
            }
            else if (e.KeyCode == Keys.Right && playerPosition.X < this.ClientSize.Width - playerSize.Width)
            {
                playerPosition.X += playerSpeed;
            }

            // 화면 다시 그리기
            this.Invalidate();
        }

        private void ResetGame()
        {
            playerPosition = new Point(307, 280);
            bullets.Clear();
            AddBullet();
            gameTimer.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Player 그리기
            DrawImageWithoutTransparency(e.Graphics, playerImage, playerPosition);

            // Bullet 그리기
            foreach (var bullet in bullets)
            {
                DrawImageWithoutTransparency(e.Graphics, bullet.Image, bullet.Position);
            }
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

        private bool IsPixelPerfectCollision(Bitmap bmp1, Rectangle rect1, Bitmap bmp2, Rectangle rect2)
        {
            Rectangle intersect = Rectangle.Intersect(rect1, rect2);

            if (intersect.IsEmpty) return false;

            for (int y = intersect.Top; y < intersect.Bottom; y++)
            {
                for (int x = intersect.Left; x < intersect.Right; x++)
                {
                    int bmp1X = x - rect1.Left;
                    int bmp1Y = y - rect1.Top;
                    int bmp2X = x - rect2.Left;
                    int bmp2Y = y - rect2.Top;

                    Color color1 = bmp1.GetPixel(bmp1X, bmp1Y);
                    Color color2 = bmp2.GetPixel(bmp2X, bmp2Y);

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

public class Bullet
{
    public Point Position { get; set; }
    public Size Size { get; set; }
    public int Speed { get; set; }
    public Bitmap Image { get; set; }

    public Bullet(Point position, Size size, int speed, Bitmap image)
    {
        Position = position;
        Size = size;
        Speed = speed;
        Image = image;
    }

    // Bullet 움직임
    public void Move()
    {
        Position = new Point(Position.X, Position.Y + Speed);
    }

    // 충돌 영역 반환
    public Rectangle GetBounds()
    {
        return new Rectangle(Position, Size);
    }
}
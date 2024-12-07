using System;
using System.Drawing;

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
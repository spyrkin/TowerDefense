using System;
using System.Globalization;


/// <summary>
/// Точка в логических координатах игрового поля.
/// Начало координат — левый верхний угол.
/// X увеличивается вправо, Y увеличивается вниз.
/// </summary>
public readonly record struct Point2D(double X, double Y)
{
    /// <summary>
    /// Точка (0, 0).
    /// </summary>
    public static Point2D Zero => new(0, 0);

    /// <summary>
    /// Расстояние до другой точки.
    /// </summary>
    public double DistanceTo(Point2D other)
    {
        double dx = X - other.X;
        double dy = Y - other.Y;

        return Math.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Квадрат расстояния до другой точки.
    /// Используется там, где не нужен Math.Sqrt.
    /// </summary>
    public double DistanceSquaredTo(Point2D other)
    {
        double dx = X - other.X;
        double dy = Y - other.Y;

        return dx * dx + dy * dy;
    }

    /// <summary>
    /// Возвращает новую точку, смещенную относительно текущей.
    /// </summary>
    public Point2D Offset(double dx, double dy)
    {
        return new Point2D(X + dx, Y + dy);
    }

    public static Point2D operator +(Point2D left, Point2D right)
    {
        return new Point2D(left.X + right.X, left.Y + right.Y);
    }

    public static Point2D operator -(Point2D left, Point2D right)
    {
        return new Point2D(left.X - right.X, left.Y - right.Y);
    }

    public static Point2D operator *(Point2D point, double scale)
    {
        return new Point2D(point.X * scale, point.Y * scale);
    }

    public static Point2D operator /(Point2D point, double scale)
    {
        return new Point2D(point.X / scale, point.Y / scale);
    }

    public override string ToString()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "({0}, {1})",
            X,
            Y);
    }
}
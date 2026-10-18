using System.Collections.Generic;

public sealed class PathGeometryTests
{
    private readonly PathGeometry _geometry = new();

    [Fact]
    public void GetLength_ReturnsPolylineLength()
    {
        IReadOnlyList<Point2D> path = new List<Point2D>
        {
            new(0, 0),
            new(10, 0),
            new(10, 10)
        };

        double length = _geometry.GetLength(path);

        Assert.Equal(20.0, length);
    }

    [Fact]
    public void GetPointAtDistance_InterpolatesPoint()
    {
        IReadOnlyList<Point2D> path = new List<Point2D>
        {
            new(0, 0),
            new(100, 0)
        };

        Point2D point = _geometry.GetPointAtDistance(path, 30.0);

        Assert.Equal(30.0, point.X);
        Assert.Equal(0.0, point.Y);
    }

    [Fact]
    public void GetPointAtDistance_ReturnsLastPointWhenDistanceExceedsLength()
    {
        IReadOnlyList<Point2D> path = new List<Point2D>
        {
            new(0, 0),
            new(100, 0)
        };

        Point2D point = _geometry.GetPointAtDistance(path, 250.0);

        Assert.Equal(100.0, point.X);
        Assert.Equal(0.0, point.Y);
    }

    [Fact]
    public void GetDistanceToPath_ReturnsDistanceToSegment()
    {
        IReadOnlyList<Point2D> path = new List<Point2D>
        {
            new(0, 0),
            new(100, 0)
        };

        double distance = _geometry.GetDistanceToPath(path, new Point2D(50, 20));

        Assert.Equal(20.0, distance);
    }
}
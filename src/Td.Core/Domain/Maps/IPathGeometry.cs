public interface IPathGeometry
{
    double GetLength(IReadOnlyList<Point2D> path);

    Point2D GetPointAtDistance(
        IReadOnlyList<Point2D> path,
        double distance);

    double GetDistanceToPath(
        IReadOnlyList<Point2D> path,
        Point2D point);
}
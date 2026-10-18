using System;
using System.Collections.Generic;

/// <summary>
/// Базовая реализация геометрии пути на основе ломаной линии.
/// </summary>
public sealed class PathGeometry : IPathGeometry
{
    public double GetLength(IReadOnlyList<Point2D> path)
    {
        if (path is null || path.Count < 2)
        {
            return 0.0;
        }

        double length = 0.0;

        for (int i = 1; i < path.Count; i++)
        {
            length += path[i - 1].DistanceTo(path[i]);
        }

        return length;
    }

    public Point2D GetPointAtDistance(
        IReadOnlyList<Point2D> path,
        double distance)
    {
        if (path is null || path.Count == 0)
        {
            return Point2D.Zero;
        }

        if (path.Count == 1)
        {
            return path[0];
        }

        if (distance <= 0.0)
        {
            return path[0];
        }

        double remaining = distance;

        for (int i = 1; i < path.Count; i++)
        {
            Point2D segmentStart = path[i - 1];
            Point2D segmentEnd = path[i];
            double segmentLength = segmentStart.DistanceTo(segmentEnd);

            if (remaining <= segmentLength)
            {
                if (segmentLength == 0.0)
                {
                    return segmentStart;
                }

                double t = remaining / segmentLength;
                return new Point2D(
                    segmentStart.X + (segmentEnd.X - segmentStart.X) * t,
                    segmentStart.Y + (segmentEnd.Y - segmentStart.Y) * t);
            }

            remaining -= segmentLength;
        }

        return path[^1];
    }

    public double GetDistanceToPath(
        IReadOnlyList<Point2D> path,
        Point2D point)
    {
        if (path is null || path.Count == 0)
        {
            return double.MaxValue;
        }

        if (path.Count == 1)
        {
            return point.DistanceTo(path[0]);
        }

        double minDistance = double.MaxValue;

        for (int i = 1; i < path.Count; i++)
        {
            double distance = DistancePointToSegment(point, path[i - 1], path[i]);
            if (distance < minDistance)
            {
                minDistance = distance;
            }
        }

        return minDistance;
    }

    private static double DistancePointToSegment(
        Point2D point,
        Point2D segmentStart,
        Point2D segmentEnd)
    {
        double vx = segmentEnd.X - segmentStart.X;
        double vy = segmentEnd.Y - segmentStart.Y;

        double wx = point.X - segmentStart.X;
        double wy = point.Y - segmentStart.Y;

        double lengthSquared = vx * vx + vy * vy;

        if (lengthSquared == 0.0)
        {
            return point.DistanceTo(segmentStart);
        }

        double projection = (wx * vx + wy * vy) / lengthSquared;
        projection = Math.Clamp(projection, 0.0, 1.0);

        Point2D closest = new Point2D(
            segmentStart.X + vx * projection,
            segmentStart.Y + vy * projection);

        return point.DistanceTo(closest);
    }
}
public sealed record MapDefinition
{
    public required string Id { get; init; }
    public required string Name { get; init; }

    public required double Width { get; init; }
    public required double Height { get; init; }

    public required IReadOnlyList<Point2D> Path { get; init; }

    public required double PathBufferRadius { get; init; }
}
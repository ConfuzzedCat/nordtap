namespace frontend.Components.Elements.ElementData;

public record struct GridPosition
{
    public static readonly GridPosition Empty = new (-1d,-1d);
    public double X { get; set; }
    public double Y { get; set; }
    

    public GridPosition(double x, double y)
    {
        X = x;
        Y = y;
    }
    public override string ToString()
    {
        return $"{X},{Y}";
    }

    public bool IsEmpty()
    {
        return Equals(this, Empty);
    }
}
namespace frontend.Components.Elements.ElementData;

public struct GridPosition
{
    public int Column { get; set; }
    public int Row { get; set; }

    public GridPosition(int column, int row)
    {
        Column = column;
        Row = row;
    }
    public override string ToString()
    {
        return $"{Column},{Row}";
    }
}
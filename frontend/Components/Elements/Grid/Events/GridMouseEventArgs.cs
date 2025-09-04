using frontend.Components.Elements.ElementData;
using Microsoft.AspNetCore.Components.Web;

namespace frontend.Components.Elements.Grid.Events;

public class GridMouseEventArgs : MouseEventArgs
{
    public GridPosition Position { get; set; }

    public GridMouseEventArgs(GridPosition position, MouseEventArgs e)
    {
        Position = position;
        Clone(e);
    }

    private void Clone(MouseEventArgs args)
    {
        this.AltKey = args.AltKey;
        this.CtrlKey = args.CtrlKey;
        this.ShiftKey = args.ShiftKey;
        this.MetaKey = args.MetaKey;
        this.Type = args.Type;
        this.Buttons = args.Buttons;
        this.Button = args.Button;
        this.Detail = args.Detail;
        this.ClientX = args.ClientX;
        this.ClientY = args.ClientY;
        this.MovementX = args.MovementX;
        this.MovementY = args.MovementY;
        this.OffsetX = args.OffsetX;
        this.OffsetY = args.OffsetY;
        this.PageX = args.PageX;
        this.PageY = args.PageY;
        this.ScreenX = args.ScreenX;
        this.ScreenY = args.ScreenY;
    }
}
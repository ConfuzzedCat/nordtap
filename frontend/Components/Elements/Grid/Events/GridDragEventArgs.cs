using frontend.Components.Elements.ElementData;
using Microsoft.AspNetCore.Components.Web;

namespace frontend.Components.Elements.Grid.Events;

public class GridDragEventArgs : DragEventArgs
{
    public GridPosition Position { get; set; }
    
    public GridDragEventArgs(GridPosition pos, DragEventArgs e)
    {
        Position = pos;
        Clone(e);
    }

    public void Clone(GridDragEventArgs obj)
    {
        Position = obj.Position;
        DataTransfer = obj.DataTransfer;
        Detail = obj.Detail;
        ScreenX = obj.ScreenX;
        ScreenY = obj.ScreenY;
        ClientX = obj.ClientX;
        ClientY = obj.ClientY;
        OffsetX = obj.OffsetX;
        OffsetY = obj.OffsetY;
        PageX = obj.PageX;
        PageY = obj.PageY;
        MovementX = obj.MovementX;
        MovementY = obj.MovementY;
        Button = obj.Button;
        Buttons = obj.Buttons;
        CtrlKey = obj.CtrlKey;
        ShiftKey = obj.ShiftKey;
        AltKey = obj.AltKey;
        MetaKey = obj.MetaKey;
        Type = obj.Type;
    }
    
    private void Clone(DragEventArgs obj)
    {
        DataTransfer = obj.DataTransfer;
        Detail = obj.Detail;
        ScreenX = obj.ScreenX;
        ScreenY = obj.ScreenY;
        ClientX = obj.ClientX;
        ClientY = obj.ClientY;
        OffsetX = obj.OffsetX;
        OffsetY = obj.OffsetY;
        PageX = obj.PageX;
        PageY = obj.PageY;
        MovementX = obj.MovementX;
        MovementY = obj.MovementY;
        Button = obj.Button;
        Buttons = obj.Buttons;
        CtrlKey = obj.CtrlKey;
        ShiftKey = obj.ShiftKey;
        AltKey = obj.AltKey;
        MetaKey = obj.MetaKey;
        Type = obj.Type;
    }
}
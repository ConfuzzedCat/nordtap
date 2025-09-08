using frontend.Components.Elements.Grid;
using Microsoft.AspNetCore.Components;

namespace frontend.Web;

public interface IContext
{
    public bool showContextMenu { get; set; }
    void NotifyChange();
}
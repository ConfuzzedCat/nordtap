using Godot;

namespace Nordtap;

public partial class ToggleMotionButton : Button
{
    public override void _Ready()
    {
        var player = GetNode<Sprite2D>("/root/Node2D/Player");
        
        //GD.Print(player.GetSignalList());
        
        player.Connect(new StringName("HealthDepleted"), new Callable(this, new StringName(nameof(PlayerOnHealthDepleted)))); 
        
        //player.HealthDepleted += PlayerOnHealthDepleted;
    }

    private void PlayerOnHealthDepleted()
    {
        SetDisabled(true);
    }
}
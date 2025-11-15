using Godot;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Nordtap;

public partial class RotateTest : Sprite2D
{
	private int _speed = 400;
	private float _angularSpeed = Mathf.Pi;
	private int _health = 10;
	
	[Signal]
	public delegate void HealthDepletedEventHandler();

	public override void _Ready()
	{
	}
	
	

	public void TakeDamage(int amount)
	{
		GD.Print($"Taking {amount} damage.");
		_health -= amount;
		if (_health <= 0)
		{
			EmitSignal(SignalName.HealthDepleted);
		}
	} 

	private void TimerOnTimeout()
	{
		Visible = !Visible;
	}

	public override void _Process(double delta)
	{
		var direction = 0;

		if (Input.IsActionPressed("ui_left"))
		{
			direction = -1;
		}
		if (Input.IsActionPressed("ui_right"))
		{
			direction = 1;
		}

		Rotation += _angularSpeed * direction * (float)delta;
		
		
		var velocity = Vector2.Zero;
		if (Input.IsActionPressed("ui_up"))
		{
			velocity = Vector2.Up.Rotated(Rotation) * _speed;
		}
		
		Position += velocity * (float)delta;

		if (Input.IsActionJustPressed("ui_accept"))
		{
			TakeDamage(1);
		}
	}

	private void OnButtonPressed()
	{
		GD.Print("OnButtonPressed");
		SetProcess(!IsProcessing());
	}
}

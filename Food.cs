using Godot;
using System;

public partial class Food : Area3D
{
	// Emitted when the player picks up the food.
	[Signal]
	public delegate void PickedUpEventHandler();

	public void OnBodyEntered(Node3D body)
	{
		if (body.Name == "Player")
		{
			EmitSignal(SignalName.PickedUp);
			QueueFree(); // Remove the food after being picked up
		}
	}

	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
	}
}

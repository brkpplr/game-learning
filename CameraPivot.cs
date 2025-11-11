using Godot;
using System;

public partial class CameraPivot : Marker3D
{
	[Export] public float Speed { get; set; } = 10.0f;

	public override void _Process(double delta)
	{
		Vector3 direction = Vector3.Zero;

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 3.0f;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 3.0f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 3.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 3.0f;
		}

		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			Position += direction * Speed * (float)delta;
		}
	}
}

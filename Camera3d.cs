using Godot;
using System;

public partial class Camera3d : Camera3D
{
	// Drag your player node here in the Godot Inspector
	[Export]
	public Node3D Target { get; set; } 

	// How far behind the player the camera should be
	[Export]
	public Vector3 PositionOffset { get; set; } = new Vector3(0, 5, 10);

	// How quickly the camera catches up (lower value is smoother)
	[Export]
	public float LerpSpeed { get; set; } = 5.0f;

	public override void _Process(double delta)
	{
		if (Target != null)
		{
			// Calculate the desired position for the camera
			Vector3 targetPosition = Target.GlobalTransform.Origin + PositionOffset;

			// Smoothly move the camera's position to the target position
			// Lerp (Linear Interpolation) is the key to the smoothness
			GlobalTransform = GlobalTransform.InterpolateWith(
				new Transform3D(GlobalTransform.Basis, targetPosition),
				(float)(LerpSpeed * delta)
			);

			// Make the camera always look at the player
			LookAt(Target.GlobalTransform.Origin);
		}
	}
}

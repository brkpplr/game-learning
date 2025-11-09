using System;
using Godot;

public partial class Player : CharacterBody3D
{
	// How fast the player moves in meters per second.
	[Export]
	public int Speed { get; set; } = 14;
	
	// The downward acceleration when in the air, in meters per second squared.
	[Export]
	public int FallAcceleration { get; set; } = 75;
	
	[Export]
	public int JumpImpulse { get; set; } = 20;
	
	[Export]
	public int BounceImpulse { get; set; } = 16;

	private Vector3 _targetVelocity = Vector3.Zero;

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;
		
		if (IsOnFloor() && Input.IsActionJustPressed("jump"))
		{
			_targetVelocity.Y = JumpImpulse;
		}

		if (Input.IsActionPressed("move_right"))
		{
			direction.X += 1.0f;
		}
		if (Input.IsActionPressed("move_left"))
		{
			direction.X -= 1.0f;
		}
		if (Input.IsActionPressed("move_back"))
		{
			direction.Z += 1.0f;
		}
		if (Input.IsActionPressed("move_forward"))
		{
			direction.Z -= 1.0f;
		}
		
		if (Input.IsActionPressed("jump"))
		{
			direction.Y += 5.0f;
		}

		if (direction != Vector3.Zero)
		{
			direction = direction.Normalized();
			GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
		}

		// Ground velocity
		_targetVelocity.X = direction.X * Speed;
		_targetVelocity.Z = direction.Z * Speed;

		// Vertical velocity
		if (!IsOnFloor()) // If in the air, fall towards the floor. Literally gravity
		{
			_targetVelocity.Y -= FallAcceleration * (float)delta;
		}
		
		 for (int index = 0; index < GetSlideCollisionCount(); index++)
		{
			// We get one of the collisions with the player.
			KinematicCollision3D collision = GetSlideCollision(index);

			// If the collision is with a mob.
			// With C# we leverage typing and pattern-matching
			// instead of checking for the group we created.
			if (collision.GetCollider() is Mob mob)
			{
				// We check that we are hitting it from above.
				if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
				{
					// If so, we squash it and bounce.
					mob.Squash();
					_targetVelocity.Y = BounceImpulse;
					// Prevent further duplicate calls.
					break;
				}
			}
		}

		// Moving the character
		Velocity = _targetVelocity;
		MoveAndSlide();
	}
}

using System;
using Godot;

public partial class Player : CharacterBody3D
{
	// How fast the player moves in meters per second.
	[Export]
	public int Speed { get; set; } = 100;
	
	// The downward acceleration when in the air, in meters per second squared.
	[Export]
	public int FallAcceleration { get; set; } = 75;
	
	private Vector3 _targetVelocity = Vector3.Zero;
	private bool _isAutoPickingUp = false;
	private Food _targetFood = null;
	private Main _mainNode;

	public override void _Ready()
	{
		_mainNode = GetNode<Main>("/root/Main");
	}

	public override void _PhysicsProcess(double delta)
	{
		var direction = Vector3.Zero;


		if (Input.IsActionPressed("auto_pick_up"))
		{
			_isAutoPickingUp = !_isAutoPickingUp;
		}

		if (!_isAutoPickingUp)
		{
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
		}
		else
		{
			if (_targetFood != null)
			{
				direction = (_targetFood.Position - Position).Normalized();
				if (IsInRange(_targetFood))
				{
					_targetFood.PickUp();
					_targetFood = FindClosestFood(); // Find next closest food
				}
			}
			else
			{
				_targetFood = FindClosestFood();
			}
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
		
		// Moving the character
		Velocity = _targetVelocity;
		MoveAndSlide();

		if (_targetFood != null && IsInRange(_targetFood))
		{
			_targetFood.PickUp();
			_targetFood = null;
		}
	}

	private Food FindClosestFood()
	{
		Food closestFood = null;
		float minDistance = float.MaxValue;

		foreach (Node node in _mainNode.GetChildren())
		{
			if (node is Food food)
			{
				float distance = Position.DistanceTo(food.Position);
				if (distance < minDistance)
				{
					minDistance = distance;
					closestFood = food;
				}
			}
		}
		return closestFood;
	}

	private bool IsInRange(Food food)
	{
		return Position.DistanceTo(food.Position) < 1.5f; // Adjust range as needed
	}
}

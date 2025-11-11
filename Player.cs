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
	private Vector3 direction = Vector3.Zero;
	private bool _isAutoPickingUp = true;
	private Food _targetFood = null;
	private Main _mainNode;
	private House _houseNode;

	private int _foodCarried = 0;
	private const int MaxFoodCarried = 10;

	public override void _Ready()
	{
		_mainNode = GetNode<Main>("/root/Main");
		_houseNode = GetNode<House>("/root/Main/House");
	}

	public override void _PhysicsProcess(double delta)
	{
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
	}
	
	public override void _Process(double delta) {
		if (_isAutoPickingUp)
		{
			if (_foodCarried >= MaxFoodCarried)
			{
				// Go to the house to drop off food
				direction = (_houseNode.Position - Position).Normalized();
			}
			else
						{
				_targetFood = FindClosestFood(); // Find next closest food

				if (_targetFood != null)
				{
					direction = (_targetFood.Position - Position).Normalized();
					if (IsInRange(_targetFood))
					{
						_targetFood.PickUp();
						_foodCarried++;
						_targetFood = null;
					}
				}
				else
				{
					direction = Vector3.Zero;
				}
			}
		}
	}

	public void DropOffFood()
	{
		GD.Print($"Player dropped off {_foodCarried} food items.");
		_houseNode.StoreFood(_foodCarried);
		_foodCarried = 0;
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

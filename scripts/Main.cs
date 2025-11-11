using Godot;
using System;

public partial class Main : Node
{
	[Export] public PackedScene FoodScene { get; set; }
	[Export] public PackedScene HouseScene { get; set; }

	private int _score;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();
		if (GetNodeOrNull<Player>("Player") == null)
		{
			GD.PrintErr("Player node not found as a child of Main. Mobs will not spawn correctly.");
		}

		GetNode<Timer>("FoodTimer").Timeout += OnFoodTimerTimeout;
		GetNode<House>("House").FoodScene = FoodScene;
	}

	private void OnFoodTimerTimeout()
	{
		SpawnFood();
	}

	private void SpawnFood()
	{
		Food food = FoodScene.Instantiate<Food>();
		AddChild(food);
		food.PickedUp += OnFoodPickedUp;

		// Position the food randomly on the ground
		var ground = GetNode<StaticBody3D>("Ground");
		var groundShape = ground.GetNode<CollisionShape3D>("CollisionShape3D").Shape as BoxShape3D;
		var groundSize = groundShape.Size;

		float randomX = (float)GD.RandRange(-groundSize.X / 2, groundSize.X / 2);
		float randomZ = (float)GD.RandRange(-groundSize.Z / 2, groundSize.Z / 2);

		food.Position = new Vector3(randomX, 0, randomZ);
	}

	private void OnFoodPickedUp()
	{
		_score++;
		GetNode<PlayerFoodLabel>("UserInterface/PlayerFoodLabel").OnFoodPickedUp();
	}

	public void OnFoodDroppedOff(int count)
	{
		GetNode<PlayerFoodLabel>("UserInterface/PlayerFoodLabel").OnFoodDroppedOff();
		GetNode<StoredFoodLabel>("UserInterface/StoredFoodLabel").OnFoodDroppedOff(count);
	}
}

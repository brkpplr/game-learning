using Godot;
using System;
using System.Collections.Generic;

public partial class House : StaticBody3D
{
	[Export] public PackedScene FoodScene { get; set; }
	private List<Node3D> _storedFood = new List<Node3D>();
	private const int MaxFoodDisplay = 5; // To avoid clutter, only display a few food items

	public override void _Ready()
	{
		// Removed incorrect call to OnBodyEntered in _Ready()
	}

	private void OnBodyEntered(Node3D body)
	{
		if (body is Player player)
		{
			GD.Print("Player entered the house.");
			player.DropOffFood();
			UpdateFoodDisplay();
		}
	}

	public void StoreFood(int count)
	{
		GD.Print($"Storing {count} food items.");
		for (int i = 0; i < count; i++)
		{
			if (FoodScene != null)
			{
				Node3D foodInstance = FoodScene.Instantiate<Node3D>();
				AddChild(foodInstance);
				_storedFood.Add(foodInstance);
				// Position food instances inside the house, avoiding overlap
				foodInstance.Position = GetFoodDisplayPosition(_storedFood.Count - 1);
				foodInstance.Visible = (_storedFood.Count - 1) < MaxFoodDisplay;
			}
		}
	}

	private void UpdateFoodDisplay()
	{
		for (int i = 0; i < _storedFood.Count; i++)
		{
			_storedFood[i].Position = GetFoodDisplayPosition(i);
			_storedFood[i].Visible = i < MaxFoodDisplay;
		}
	}

	private Vector3 GetFoodDisplayPosition(int index)
	{
		// Simple grid-like positioning inside the house
		float x = (index % 2) * 0.5f - 0.25f; // Two columns
		float z = (index / 2) * 0.5f - 0.25f; // Rows
		float y = 0.5f; // Lift slightly off the ground
		return new Vector3(x, y, z);
	}
}

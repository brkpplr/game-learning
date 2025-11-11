using Godot;
using System;

public partial class PlayerFoodLabel : Label
{
	private int _score = 0;

	public void OnFoodPickedUp()
	{
		_score += 1;
		Text = $"Player Food: {_score}";
	}

	public void OnFoodDroppedOff()
	{
		_score = 0;
		Text = $"Player Food: {_score}";
	}
}

using Godot;
using System;

public partial class StoredFoodLabel : Label
{
	private int _score = 0;

	public void OnFoodDroppedOff(int score)
	{
		_score += score;
		Text = $"Stored Food: {_score}";
	}
}

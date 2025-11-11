using Godot;
using System;

public partial class ScoreLabel : Label
{
	private int _score = 0;

	public void OnMobSquashed()
	{
		_score += 1;
		Text = $"Score: {_score}";
	}

	public void UpdateScore(int score)
	{
		_score = score;
		Text = $"Score: {_score}";
	}
}

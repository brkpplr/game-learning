using Godot;
using System;

public partial class Main : Node
{
	[Export] public PackedScene MobScene { get; set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Randomize();
		if (GetNodeOrNull<Player>("Player") == null)
		{
			GD.PrintErr("Player node not found as a child of Main. Mobs will not spawn correctly.");
		}
	}

	private void OnMobTimerTimeout()
	{
		Mob mob = MobScene.Instantiate<Mob>();

		var mobSpawnLocation = GetNode<PathFollow3D>("SpawnPath/SpawnLocation");
		mobSpawnLocation.ProgressRatio = GD.Randf();

		var playerPosition = GetNode<Player>("Player").Position;
		mob.Initialize(mobSpawnLocation.Position, playerPosition);

		AddChild(mob);
		mob.Squashed += GetNode<ScoreLabel>("UserInterface/ScoreLabel").OnMobSquashed;
	}
}

using Godot;
using System;
using System.Collections.Generic;

public partial class EnemySpawner : Node2D
{
	[Export]
	int MaxGhost = 5;
	[Export]
	PackedScene ghost1;
	[Export]
	PackedScene ghost2;
	[Export]
	PackedScene ghost3;
	[Export]
	PackedScene ghost4;
	[Export]
	PackedScene ghost5;

	int EnemyCounter = 0;

	RandomNumberGenerator rng = new RandomNumberGenerator();

	Godot.Collections.Array<Godot.Node> spawners;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spawners = this.GetChildren();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void CreateGhost()
	{
		int random = rng.RandiRange(1, 5);

		RigidBody2D ghost = ghost1.Instantiate() as RigidBody2D;
		spawners[random].AddChild(ghost);
		var children = ghost.GetChildren();
		for(int i=0; i < children.Count; i++)
		{
			if(children[i] is EnemyBase enemy)
				enemy.OnDeath += ()=> {EnemyCounter--;};
		}
		EnemyCounter--;
	}
}

using Godot;
using System;

public partial class TesteDano : Area2D
{

		int damage = 20;

		PlayerScripts PlayerInRange;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public static PlayerScripts GetPlayerScript(Godot.Collections.Array<Node> Children)
	{
		for(int i =0; i < Children.Count; i++)
		{
			if (Children[i] is PlayerScripts Player)
			{
				return Player;
			}
		}
		return null;
	}
	protected void OnAttackDetect(Node2D Body) //detect player for Hurtbox (attack Player)
	{
		var BodyChildren = Body.GetChildren();
		GetPlayerScript(BodyChildren);
		var Player = GetPlayerScript(BodyChildren);
		if (Player != null)
		{
			PlayerInRange = Player;
		}	
		
	}
	}
	

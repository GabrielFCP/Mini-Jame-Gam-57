using Godot;
using System;

public partial class TesteDanoScript : Node
{
	[Export]
	protected Area2D HurtBox;
	[Export]
	protected double AttackSpeed = 0.5d;
	protected PlayerScripts PlayerInRange;

	int Damage =20;

	protected PlayerScripts PlayerPotentialDamage;
	protected double TimeSinceLastAttack = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		HurtBox.BodyEntered += OnAttackDetect; //detect for attack
		HurtBox.BodyExited += OnAttackDetectOut;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		AttackPlayer(delta);
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
	protected void OnAttackDetectOut(Node2D Body)
	{
		PlayerInRange = null;
	}

	protected void AttackPlayer(double delta)
	{ 
		TimeSinceLastAttack+= delta;
		//GD.Print(TimeSinceLastAttack);
		if(PlayerInRange != null)
		{
			if (TimeSinceLastAttack >= AttackSpeed)
			{
				PlayerInRange.Hit(Damage);
				TimeSinceLastAttack= 0;
			}
		}
	}
}

using Godot;
using System;

public abstract partial class EnemyBase : Node2D
{
	[Export]
	protected AnimatedSprite2D Sprite;
	[Export]
	protected int Health;
	[Export]
	protected int MaxHealth = 100;
	[Export]
	protected int Damage = 10;
	[Export]
	protected float Speed = 100;
	[Export]
	protected float StopDistance = 300;
	[Export]
	protected float MeleeDistance = 20f;
	[Export]
	protected RigidBody2D RB;
	[Export]
	protected SpriteFrames Spawn;
	[Export]
	protected SpriteFrames Walk;
	[Export]
	protected SpriteFrames TakeDamage;
	[Export]
	protected SpriteFrames Dissapear;
	[Export]
	protected RigidBody2D Target;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Target != null)
		{
			var TargetChildren = Target.GetChildren();
			for(int i = 0; i < TargetChildren.Count; i++)
			{
				if (Target.GlobalPosition.DistanceTo(RB.GlobalPosition) > MeleeDistance)
				{
					return;
				}
				// else if (TargetChildren[i] is PlayerScripts player)
				// {
				// 	player.Take_damage(Damage);
				// }
			}
		}
	}
}

using Godot;
using System;

public abstract partial class EnemyBase : Node
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
	protected float Speed = 150;
	[Export]
	protected float StopDistance = 300;
	[Export]
	protected float MeleeDistance = 20f;
	[Export]
	protected RigidBody2D RB;
	[Export]
	protected Area2D LineOfSight;
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

	protected void Targeting(Node2D body)
    {
        var bodyChildren = body.GetChildren();
		for(int i = 0; i < bodyChildren.Count; i++)
		{
			if (bodyChildren[i] is PlayerScripts player)
			{
				Target = body as RigidBody2D;

			}
		}
    }

	protected void Chase()
	{
		if (Target == null)
		{
			return;
		}
		float Distance = RB.GlobalPosition.DistanceSquaredTo(Target.GlobalPosition);
		if (Distance < StopDistance)
		{
			RB.LinearVelocity = Vector2.Zero;

		}
		else
		{
			Vector2 dir = (Target.GlobalPosition - RB.GlobalPosition).Normalized();
			RB.LinearVelocity = dir * Speed;
		}
	}

	protected void Attack()
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
				 else if (TargetChildren[i] is PlayerScripts player)
				 {
					player.Hit(Damage);
				 }
			}
		}
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		LineOfSight.BodyEntered += Targeting;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Target != null)
		{
			Chase();
			Attack();
		}
	}	
}

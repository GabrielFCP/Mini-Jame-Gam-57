using Godot;
using System;

public abstract partial class EnemyBase : Node
{
	[Export]
	protected AnimatedSprite2D Sprite;
	[Export]
	protected Sprite2D Shadow;
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
	protected float AttackSpeed = 1;
	double TSLA = 0;
	[Export]
	protected RigidBody2D RB;
	[Export]
	protected Area2D LineOfSight;
	[Export]
	protected SpriteFrames Spawn;
	[Export]
	protected SpriteFrames Walk;
	[Export]
	protected RigidBody2D Target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		FloatTweenAnim();
		LineOfSight.BodyEntered += Targeting;
		LineOfSight.BodyExited += NTargeting;
		Health = MaxHealth;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		TSLA += delta;
		if(Target != null)
		{
			FlipAnim();
			Chase();
			Attack();
		}
	}

		/// <summary>
	/// Can a Ghost die?
	/// </summary>
	protected void Death()
	{
		Sprite.Frame++;
		DissolveTweenAnim();
	}

	public void TakeDamage()
	{
		GD.Print("GhostHit");
		NextFrame();
		
	}

	private void NTargeting(Node body)
	{
		Target = null;
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
				 else if (TargetChildren[i] is PlayerScripts player && AttackSpeed <= TSLA)
				 {
					player.Hit(Damage);
					TSLA = 0;
				 }
			}
		}
	}

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

	#region AnimationHelpers

	protected void FlipAnim()
	{
		if(RB.LinearVelocity.X > 0)
			Sprite.FlipH = false;
		else
			Sprite.FlipH = true;
	}


	/// <summary>
	/// Starts a float effect on the Ghost EnemyBase Sprite class variable. This is a Loop and should only be started once.
	/// </summary>
	protected void FloatTweenAnim()
	{
		Tween tween = CreateTween();
		tween.SetLoops();
		tween.SetEase(Tween.EaseType.Out);
		tween.TweenProperty(Sprite, "position", new Vector2(Sprite.Position.X, -2.5f), 2f);
		tween.TweenProperty(Sprite, "position", new Vector2(Sprite.Position.X, 2.5f), 2f);
		tween.TweenInterval(0.1f);
	}

	/// <summary>
	/// Creates a dissolve effect on both the EnemyBase Sprite and it's shadow.
	/// </summary>
	protected void DissolveTweenAnim()
	{
		Tween tween = CreateTween();
		tween.TweenInterval(0.3f);
		tween.SetEase(Tween.EaseType.OutIn);
		tween.SetParallel();
		tween.TweenProperty(Sprite, "modulate:a", 0, 2f);
		tween.TweenProperty(Shadow, "modulate:a", 0, 2f);
	}

	protected int GhostCurrentSpriteIndex = 0;
	/// <summary>
	/// Whenever the enemy suffers damage, changing for the next frame will slowly make the ghost cleaner. Do not use it for death.
	/// </summary>
	protected void NextFrame()
	{
		int lenght = Sprite.SpriteFrames.GetFrameCount("default");
		if(GhostCurrentSpriteIndex < lenght - 1) // Stop at the last clean frame. 
		{
			Sprite.Frame = Sprite.Frame + 1;
			GhostCurrentSpriteIndex++;
		}
	}

	#endregion

	
}

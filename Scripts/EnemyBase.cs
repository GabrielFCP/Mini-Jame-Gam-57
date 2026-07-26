using Godot;
using System;

public abstract partial class EnemyBase : Node2D
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
		FloatTweenAnim();
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
		if(GhostCurrentSpriteIndex > lenght - 2) // Stop at the last clean frame. 
		{
			Sprite.Frame++;
			GhostCurrentSpriteIndex++;
		}
	}

	#endregion

	/// <summary>
	/// Can a Ghost die?
	/// </summary>
	protected void Death()
	{
		Sprite.Frame++;
		DissolveTweenAnim();
	}

	public void TakeDamage2()
	{
		NextFrame();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		FlipAnim();
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
}

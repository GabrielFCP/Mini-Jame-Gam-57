using Godot;
using System;

public partial class PlayerScripts : Node
{
	[Export]
	RigidBody2D RB;
	[Export]
	AnimatedSprite2D Sprite;
	[Export]
	SpriteFrames Idle;
	[Export]
	SpriteFrames Run;
	[Export]
	SpriteFrames Attack;
	Vector2 V2Input;
	float WalkSpeed = 400;
	float MaxHP = 100;
	float HP = 100;
	double TSLH = -1;
	bool IsAlive = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(IsAlive == true)
		{
			TSLH += delta;
			Inputget();
		}
		if(HP <= 0)
			IsAlive = false;
	}

    public override void _PhysicsProcess(double delta)
    {
        Movement();
    }
///////////////////////////////////////////////////
	private void Inputget()
	{
		V2Input = Input.GetVector("Left", "Right", "Up", "Down");
	}

	private void Movement()
	{
		RB.LinearVelocity = V2Input * WalkSpeed;
	}

	public void Hit(float Value, bool IsHeal = false)
	{
		if(IsHeal == true)
		{
			GD.Print($"Hit! {Value}");
			HP -= Value;
			HitTween();
		}
		else if(IsHeal == true)
		{
			GD.Print($"Healed! {Value}");
			HP += Value;
		}
	}

	private void HitTween()
	{
		Tween tween = CreateTween();
		tween.SetLoops(4);
		tween.TweenProperty(Sprite, "modulate:a", 0.2, 0.25);
		tween.TweenProperty(Sprite, "modulate:a", 1, 0.25);
	}
}

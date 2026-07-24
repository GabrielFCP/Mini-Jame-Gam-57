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
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Inputget();
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
}

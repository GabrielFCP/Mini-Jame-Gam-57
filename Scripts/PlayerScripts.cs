using Godot;
using System;

public partial class PlayerScripts : Node
{
	[Export]
	RigidBody2D RB;
	[Export]
	Area2D Hurtbox;
	[Export]
	AnimatedSprite2D Sprite;
	[Export]
	SpriteFrames IdleAnim;
	[Export]
	SpriteFrames RunAnim;
	[Export]
	SpriteFrames AttackAnim;
	Vector2 V2Input;
	float LRadInput;
	float WalkSpeed = 400;
	float AttackSpeed = 1;
	float MaxHP = 100;
	float HP = 100;
	double TSLH = -1; //Time Since Last Hurt
	double TSLA = 999; //Time Since Last Attack
	double AT = 0; //Usa negativo/0 Acaba
	bool IsAlive = true;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(IsAlive == true) //Morto não faz nada
		{
			TSLH += delta;
			TSLA += delta;
			AT += delta;
			Inputget();
		}
		if(HP <= 0) //Morreu? sei-lá
			IsAlive = false;

		if(AT >= 0) //Fim do ataque
			Hurtbox.Monitoring = false;

		if(Input.IsActionJustPressed("Attack") && AttackSpeed <= TSLA) //Ataque
			Attack(1);
	}

    public override void _PhysicsProcess(double delta)
    {
        Movement();
    }
///////////////////////////////////////////////////
	private void Inputget()
	{
		V2Input = Input.GetVector("Left", "Right", "Up", "Down"); //Input
		var RadInput = V2Input.Angle(); //Loucura ->

		if(V2Input != Vector2.Zero)
		{
			Hurtbox.Rotation = RadInput;
			LRadInput = RadInput;
		}
		else
			Hurtbox.Rotation = LRadInput;
	}

	private void Attack(float segundos)
	{
		//Sprite.SpriteFrames = AttackAnim;
		//Sprite.Play();
		AT = 0;
		Hurtbox.Monitoring = true;
	}

	private void Movement()
	{
		RB.LinearVelocity = V2Input * WalkSpeed;
		if(V2Input != Vector2.Zero) //Anim
		{
			//Sprite.SpriteFrames = RunAnim;
			//Sprite.Play();
		}
		else
		{
			//Sprite.SpriteFrames = IdleAnim;
			//Sprite.Play();
		}
	}

	public void Hit(float Value, bool IsHeal = false)
	{
		if(IsHeal == false) //Dano
		{
			GD.Print($"Hit! {Value}");
			HP -= Value;
			if(HP <= 0)
				YouDied();
			else
				HitTween();
			
		}
		else if(IsHeal == true) // Cura
		{
			GD.Print($"Healed! {Value}");
			HP += Value;
		}
	}

	private void HitTween()
	{
		Tween Transparency = CreateTween();
		Tween Color = CreateTween();
		Transparency.SetLoops(4);
		Color.SetLoops(4);
		Transparency.TweenProperty(Sprite, "modulate:a", 0.2, 0.25);
		Color.TweenProperty(Sprite, "modulate:r", 1, 0.25);
		Transparency.TweenProperty(Sprite, "modulate:a", 1, 0.25);
		Color.TweenProperty(Sprite, "modulate:r", 0, 0.25);
	}

	private void YouDied()
	{
		RB.Freeze = true;
		IsAlive = false;
		//Tween tween CreateTween()
	}
}

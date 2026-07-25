using Godot;
using System;

public partial class PlayerScripts : Node
{
	[Export]
	RigidBody2D RB;
	[Export]
	Area2D Hurtbox;

// Animation Sprites
	[Export]
	AnimatedSprite2D Sprite;
	[Export]
	SpriteFrames IdleSide;
	[Export]
	SpriteFrames IdleFront;
	[Export]
	SpriteFrames IdleBack;
	[Export]
	SpriteFrames WalkFront;															///divide animations on three separte ones for each side
	[Export]
	SpriteFrames WalkSide;
	[Export]
	SpriteFrames WalkBack;
	[Export]
	SpriteFrames AttackSide;
	[Export]
	SpriteFrames AttackBack;
	[Export]
	SpriteFrames AttackFront;
	[Export]
	SpriteFrames DeathAnim;


	Vector2 V2Input;
	float LRadInput;
	float WalkSpeed = 400;
	float AttackSpeed = 1;
	float MaxHP = 100;
	float HP = 100;
	double TSLH = -1; //Time Since Last Hurt
	double TSLA = 999; //Time Since Last Attack
	double AT = 0; //Usa negativo/0 Acaba
	bool CanInteract = true;
	bool CanDo = true;
	////Actions
	public Action Morte;
	public Action<float, float> IChangedHP;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameManager.Inst.StopMoving += Stop;
		GameManager.Inst.CanMove += Now;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(CanDo == false) //Se voltar movimento cedo (Death Tricks)
			CanInteract = false;

		if(CanInteract == true) //Morto não faz nada
		{
			TSLH += delta;
			TSLA += delta;
			AT += delta;
			Inputget();
		}

		if(HP <= 0) //Morreu? sei-lá
			CanInteract = false;

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

		if(V2Input == Vector2.Zero) 
		{
			if(Sprite.SpriteFrames == WalkFront) //Idle front
			{
				Sprite.SpriteFrames = IdleFront;
				Sprite.Play();
			}

			if(Sprite.SpriteFrames == WalkSide)
			{
				Sprite.SpriteFrames = IdleSide;
				Sprite.Play();
			}

			if(Sprite.SpriteFrames == WalkBack)
			{
				Sprite.SpriteFrames = IdleBack;
				Sprite.Play();
			}
		}

		if(V2Input != Vector2.Zero) //Animations walk
		{
			if(V2Input.X > 0 && V2Input.Y == 0 ) //andando para direita (x positivo e y =0)
			{
				Sprite.SpriteFrames = WalkSide;
				Sprite.FlipH = true;
				Sprite.Play();
			}

			if (V2Input.X < 0 && V2Input.Y == 0) //Andando para esquerda (x negativo e y = 0)
			{
				Sprite.SpriteFrames = WalkSide;
				Sprite.FlipH = false;
				Sprite.Play();
			}

			if(V2Input.X ==0 && V2Input.Y > 0) //walking front
			{	
				Sprite.SpriteFrames = WalkFront;
				Sprite.Play();
			}

			if(V2Input.X ==0 && V2Input.Y < 0) //walking back
			{
				Sprite.SpriteFrames = WalkBack;
				Sprite.Play();
			}
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
		IChangedHP?.Invoke(HP, MaxHP);
	}

	private void HitTween()
	{
		Tween Transparency = CreateTween();
		Tween ColorG = CreateTween();
		Tween ColorB = CreateTween();
		Transparency.SetLoops(2);
		ColorG.SetLoops(2);
		ColorB.SetLoops(2);
		Transparency.TweenProperty(Sprite, "modulate:a", 0.2, 0.25);
		Transparency.TweenProperty(Sprite, "modulate:a", 1, 0.25);
		ColorG.TweenProperty(Sprite, "modulate:g", 0, 0.25);
		ColorB.TweenProperty(Sprite, "modulate:b", 0, 0.25);
		ColorG.TweenProperty(Sprite, "modulate:g", 1, 0.25);
		ColorB.TweenProperty(Sprite, "modulate:b", 1, 0.25);
	}

	private void YouDied()
	{
		//Sprite.SpriteFrames = DeathAnim;
		//Sprite.Play();
		RB.Freeze = true;
		CanInteract = false;
		Morte?.Invoke();
		Tween tween = CreateTween();
		tween.TweenInterval(1); //Tempo de morte
		tween.Finished += Restart;
	}

	private void Restart()
	{
		RB.Freeze = false;
		CanInteract = true;
	}
	
	//////////////////////////
	private void Stop()
	{
		CanDo = false;
		CanInteract = false;
	}

	private void Now()
	{
		CanDo = true;
		CanInteract = true;
	}
}

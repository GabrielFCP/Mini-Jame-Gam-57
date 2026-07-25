using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Inst;
	public Action StopMoving;
	public Action CanMove;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if(Inst != null)
		{
			GD.PrintErr("Dois GameManagers");
			this.QueueFree();
		}
		Inst = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

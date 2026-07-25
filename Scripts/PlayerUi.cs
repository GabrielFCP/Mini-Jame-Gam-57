using Godot;
using System;

public partial class PlayerUi : Control
{
	[Export]
	PlayerScripts Player;
	[Export]
	ProgressBar Healthbar;
	[Export]
	Label HealthText;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player.IChangedHP += HPChange;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void HPChange(float HP, float MaxHP)
	{
		Healthbar.Value = HP;
		HealthText.Text = HP + "/" + MaxHP;
	}
}

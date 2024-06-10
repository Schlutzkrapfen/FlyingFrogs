using System;
using Godot;

namespace MMJ.Skripts;

public partial class Quit : Button
{

	private readonly Random random = new Random();
	private AudioStreamPlayer soundPlayer;
	private AudioStreamPlayer chargePlayer;
	private TextureProgressBar quitProgress;
	private Control selectMenu;
	private Control settingsMenu;
	private TextureRect tutorial;
	private double time;
	private bool bButtonPress;
	private double maxTime = 3;
	public override void _Ready(){
		soundPlayer = GetNode<AudioStreamPlayer>("../../../../../MainMenuSound/Click");
		quitProgress = GetNode<TextureProgressBar>("HBoxContainer/TextureRect/TextureProgressBar");
		chargePlayer = GetNode<AudioStreamPlayer>("../../../../../MainMenuSound/ChargeQuit");
		selectMenu = GetNode<Control>("../../../SelectMenu");
		settingsMenu = GetNode<Control>("../../HSplitContainer4/Settings");
		 tutorial = GetNode<TextureRect>("../../../TextureRect2");
		if (selectMenu == null)
		{
			GD.PrintErr("selectMenu not found #Quit.cs");
		}
		if(settingsMenu == null)
		{
			GD.PrintErr("settingsMenu not found #Quit.cs");
		}
	
	}

private void _on_button_up()
{
	if(soundPlayer != null){
			float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
			soundPlayer.PitchScale = randomPitch;
			soundPlayer.Play();
		}
		GetTree().Quit();
}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("FrogQuit0")&& !selectMenu.Visible && !settingsMenu.Visible 
		&&!tutorial.Visible )
		{
			chargePlayer.Play();
			bButtonPress = true;
		}
		if (@event.IsActionReleased("FrogQuit0"))
		{
			chargePlayer.Stop();
			bButtonPress = false;
		}
	}
	public override void _Process(double delta)
	{
		if (bButtonPress)
		{
			time +=delta;
			if (time >= maxTime)
			{
				GetTree().Quit();
			}
		}
		else if (!bButtonPress && time >= 0)
		{
			time -= delta;
		}
		quitProgress.Value = time*10;
		
	}
}



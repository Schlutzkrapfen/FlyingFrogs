using Godot;
using System;


public partial class tutorial : TextureRect
{
	private AudioStreamPlayer clickPlayer;
	private Button aButton;
	//i grab the focus of the slider otherwise it would click somethin
	private Slider slider;
	private settings settings;
	
	private readonly Random random = new Random();

	public override void _Ready()
	{
		clickPlayer = GetNode<AudioStreamPlayer>("../../../MainMenuSound/Click");
		aButton = GetNode<Button>("AButton");
		settings = GetNode<settings>("../HBoxContainer/HSplitContainer4/Settings");
		slider = GetNode<Slider>("../HBoxContainer/HSplitContainer4/Settings/Panel/VBoxContainer/HBoxContainer4/HSliderEffects");
	}

private void _on_button_button_up()
{
	if(!this.Visible)
	{
		if (clickPlayer != null)
	{
		float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
		clickPlayer.PitchScale = randomPitch;
		clickPlayer.Play();
	}
		aButton.GrabFocus();
		
	this.Visible = true;
	//Settings needs to be not visible otwise it would grab focus if you would move upwards
	settings.Visible = false;
	}
	
}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionReleased("FrogJoin0")&& this.Visible)
		{
			if (clickPlayer != null)
	{
		float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
		clickPlayer.PitchScale = randomPitch;
		clickPlayer.Play();
	}

	this.Visible = false;
	settings.Visible = true;
	slider.GrabFocus();
	
		}

	}
		
	private void _on_a_button_button_up()
{
	if (clickPlayer != null)
	{
		float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
		clickPlayer.PitchScale = randomPitch;
		clickPlayer.Play();
	}
	
	this.Visible = false;
	settings.Visible = true;
slider.GrabFocus();
}
}





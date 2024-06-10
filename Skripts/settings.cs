using Godot;
using System;

public partial class settings : Control
{
	private AudioStreamPlayer clickPlayer;
	private readonly Random random = new Random();

	[Export] private String audioBusMusicName = "Music";

	[Export] private String audioBusEffectName = "Effect";
	string savePath = "user://file_name.res";
	private OptionButton windowMode;
	private BoxContainer selectMenu;
	private HSlider musicSlider;
	private HSlider effectSlider;
	private PopupMenu windowmode = new PopupMenu();
	private double stepTime = 0.5;
	private int steps = 10;
	private int firstWaitTime = 2;
	private double timeRight;
	private double timeRightEffects ;
	private double timeLeft ;
	private double timeLeftEffects ;
	ConfigFile configFile = new ConfigFile();

	private int busMusic;

	private int busEffect;
	public override void _Ready()
	{
		GetNodes();
		windowmode = windowMode.GetPopup();
		LoadSettings();
	}

	private void GetNodes()
	{
		timeRight = stepTime;
		timeRightEffects = stepTime;
		timeLeft = stepTime;
		timeLeftEffects = stepTime;
		busMusic = AudioServer.GetBusIndex(audioBusMusicName);
		busEffect = AudioServer.GetBusIndex(audioBusEffectName);
		windowMode = GetNode<OptionButton>("Panel/VBoxContainer/HBoxContainer/OptionButton");
		musicSlider = GetNode<HSlider>("Panel/VBoxContainer/HBoxContainer3/HSlider");
		effectSlider = GetNode<HSlider>("Panel/VBoxContainer/HBoxContainer4/HSliderEffects");
		clickPlayer = GetNode<AudioStreamPlayer>("../../../../../MainMenuSound/Click");
		selectMenu = GetNode<BoxContainer>("../../../SelectMenu");
		if (selectMenu == null)
		{
			GD.PrintErr("selectMenu not found #settings.cs");
		}
	}

	private void LoadSettings()
	{
	
		var error = configFile.Load(savePath);
		if (error != 0)
		{
			return;
		}
		
		
		foreach (String Settings in configFile.GetSections())
		{
			int index = (int)configFile.GetValue(Settings,"WindowMode");
			DisplayServer.WindowMode mode = (DisplayServer.WindowMode)index;
			DisplayServer.WindowSetMode(mode,0);
			if (index >= 1)
			{
				index -= 1;
			}
			windowMode.Selected =index;
			float effectValue = (float)configFile.GetValue(Settings,"EffectSlider");

				if (effectValue > -41)
				{
					AudioServer.SetBusVolumeDb(busEffect,(float)effectValue);
				}
				else
				{
					AudioServer.SetBusVolumeDb(busEffect,-81);
				}
			effectSlider.Value = effectValue;
			float musicValue = (float)configFile.GetValue(Settings,"MusicSlider");
			if (musicValue > -41)
			{
				AudioServer.SetBusVolumeDb(busMusic,(float)musicValue);
			}
			else
			{
				AudioServer.SetBusVolumeDb(busMusic,-81);
			}
			musicSlider.Value = musicValue;
			
			
		}
	}

public override void _Input(InputEvent @event)
{
	if (@event.IsActionPressed("MenuSettings") && !selectMenu.Visible)
	{
		if(clickPlayer != null)
		{
			float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
			clickPlayer.PitchScale = randomPitch;
			clickPlayer.Play();
		}
		this.Visible = !this.Visible;
		if (this.Visible)
		{
			windowMode.GrabFocus();
		}
		
	}
	if (@event.IsActionReleased("FrogQuit0")&& this.Visible)
	{
		this.Visible = false;
	}
}
//this is for the slider code because for some resons the slider dont move constantly with the controller in godot
//evento it works with the arrow keys
//TODO: needs better smoothing
public override void _PhysicsProcess(double delta)
{
	if (Input.IsActionPressed("ui_right"))
	{
		if (musicSlider.HasFocus())
		{
			timeRight -= delta;
		}
		if (timeRight <= 0)
		{
			timeRight = stepTime;
			musicSlider.Value += steps;
			_on_h_slider_value_changed(musicSlider.Value);
		}

		if (effectSlider.HasFocus())
		{
			timeRightEffects -= delta;
		}
		if (timeRightEffects <= 0)
		{
			timeRightEffects = stepTime;
			effectSlider.Value += steps;
			_on_h_slider_effects_value_changed(effectSlider.Value);
			clickPlayer.Play();
		}
	}
	else
	{
		timeRight = stepTime * firstWaitTime;
		timeRightEffects = stepTime * firstWaitTime;
	}


	if (Input.IsActionPressed("ui_left"))
	{
		if (musicSlider.HasFocus())
		{
			timeLeft -= delta;
		}
		if (timeLeft <= 0)
		{
			timeLeft = stepTime;
			musicSlider.Value -= steps;
			_on_h_slider_value_changed(musicSlider.Value);
		}
		if (effectSlider.HasFocus())
		{
			timeLeftEffects -= delta;
		}
		if (timeLeftEffects <= 0)
		{
			timeLeftEffects = stepTime;
			effectSlider.Value -= steps;
			_on_h_slider_effects_value_changed(effectSlider.Value);
			clickPlayer.Play();
		}
	}
	else
	{
		timeLeft = stepTime * firstWaitTime;
		timeLeftEffects = stepTime * firstWaitTime;
	}


}
	
private void _on_settings_button_up()
{
		if(clickPlayer != null){
 			 float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
 			 clickPlayer.PitchScale = randomPitch;
 			 clickPlayer.Play();
		}
		this.Visible = !this.Visible;
		if (this.Visible)
		{
			windowMode.GrabFocus();
		}
}
private void _on_option_button_item_selected(long index)
{
	if(clickPlayer != null){
				float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
				clickPlayer.PitchScale = randomPitch;
				clickPlayer.Play();
	}
	//WindowMode 1 does minimize the window this skips it
	if (index >= 1)
	{
		index +=1;
	}
	DisplayServer.WindowMode mode = (DisplayServer.WindowMode)index;
	DisplayServer.WindowSetMode(mode,0);
	configFile.SetValue("Settings","WindowMode", index);
	configFile.Save(savePath);

}
private void _on_h_slider_value_changed(double value)
{
	
	if (value > -41)
	{
		AudioServer.SetBusVolumeDb(busMusic,(float)value);
	}
	else
	{
		AudioServer.SetBusVolumeDb(busMusic,-81);
	}
	configFile.SetValue("Settings","MusicSlider", value);
	configFile.Save(savePath);
	// Replace with function body.
}


private void _on_h_slider_effects_value_changed(double value)
{
		if (value > -41)
		{
			AudioServer.SetBusVolumeDb(busEffect,(float)value);
		}
		else
		{
			AudioServer.SetBusVolumeDb(busEffect,-81);
		}
		configFile.SetValue("Settings","EffectSlider", value);
		configFile.Save(savePath);
	
		// Replace with function body.
}
private void _on_h_slider_effects_drag_ended(bool valueChanged)
{
	 clickPlayer.Play();
}
}



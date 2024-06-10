using Godot;
using System;

public partial class GameSettings : ColorRect
{
	
	
	
	/// Called when the node enters the scene tree for the first time.
	private OptionButton gameModeButton;
	private OptionButton pointsSettingsButton;
	private CheckBox tutorialButton;
	private int currentSettings ;
	/// <summary>
	/// bacause of some reason joystick behave differently than normal buttons i need to make this ugly code
	/// </summary>
	private double lastUpdate;
	//TODO: add the logic for the second Gamemode in the main Scene
	private  int[,] settingsPoints = new int[2,3];



	public override void _Ready()
	{
		settingsPoints[0, 0] = 60;
		settingsPoints[0, 1] = 120;
		settingsPoints[0, 2] = 180;
		settingsPoints[1, 0] = 50;
		settingsPoints[1, 1] = 100;
		settingsPoints[1, 2] = 150;
		GetNodes();
	}

	private void GetNodes()
	{
		gameModeButton = 
			GetNode<OptionButton>("PopUpMenu/VBoxContainer/Panel/VBoxContainer/HBoxContainer/OptionButton");
		pointsSettingsButton =
			GetNode<OptionButton>("PopUpMenu/VBoxContainer/Panel/VBoxContainer/HBoxContainer2/OptionButton2");
		tutorialButton = 
			GetNode<CheckBox>("PopUpMenu/VBoxContainer/Panel/VBoxContainer/HBoxContainer3/CheckBox");
	}

	public override void _Process(double delta)
	{
		lastUpdate += delta;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_left") && lastUpdate >= 0.2 )
		{
			lastUpdate = 0;
			if(gameModeButton.HasFocus())
			{
				ChangeSettings(-1);
			}
			if (pointsSettingsButton.HasFocus())
			{
				ChangePoints(-1);
			}
			
		}
		if (@event.IsActionPressed("ui_right") && lastUpdate >= 0.2)
		{
			lastUpdate = 0;
			if(gameModeButton.HasFocus())
			{
				ChangeSettings(+1);
			}
			if (pointsSettingsButton.HasFocus())
			{
				ChangePoints(+1);
			}
		}
	}
	//code to switch true how many Points you need to win and imidiatly change them in the Gloabal Value
	private void ChangeSettings(int value)
	{
		int currentSelected = gameModeButton.GetSelectedId();
		int index = (currentSelected + value + settingsPoints.GetLength(0)) % settingsPoints.GetLength(0);
		gameModeButton.Select(index);
		pointsSettingsButton.Select(index*settingsPoints.GetLength(1) + pointsSettingsButton.GetSelectedId()  % settingsPoints.GetLength(1));
		Global.gameMode = index;
		
		Global.playTime =settingsPoints[index,pointsSettingsButton.GetSelectedId()  % settingsPoints.GetLength(1)];
		
	}
	//code to switch true how many Points you need to win and imidiatly change them in the Gloabal Value
	private void ChangePoints(int value)
	{
			int currentSelected = pointsSettingsButton.GetSelectedId() % settingsPoints.GetLength(1);
			int index = (gameModeButton.GetSelectedId()* settingsPoints.GetLength(1))+((currentSelected + value+settingsPoints.GetLength(1) )% settingsPoints.GetLength(1));
			pointsSettingsButton.Select(index);
			Global.playTime =settingsPoints[gameModeButton.GetSelectedId(), index % settingsPoints.GetLength(1)];
	}



private void _on_texture_button_button_up()
{
	ChangeSettings(-1);
}


private void _on_texture_button_2_button_up()
{
	ChangeSettings(+1);
}
private void _on_texture_button_3_button_up()
{
	ChangePoints(-1);
}
private void _on_texture_button_4_button_up()
{
	ChangePoints(+1);
}
//code for when the popupmenu is used
private void _on_option_button_2_item_selected(long index)
{
	Global.playTime =settingsPoints[gameModeButton.GetSelectedId(), index % settingsPoints.GetLength(1)];
}


}

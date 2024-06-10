using Godot;
using System;
using System.Collections.Generic;

public partial class PauseMenu : Control
{
	private Control _pausePanel;
	private BoxContainer _settings;
	
	bool isPaused = false;
	bool isMenuing = false;
	int PauserID = 0;
	
	int optionCount = 0;
	int currentSelection = 0;
	
	private Node menuScreen;
	
	List<Button> buttons = new List<Button>();
	

	
	public override void _Ready(){
		_pausePanel = GetNodeOrNull<Control>("PausePanel");
		_pausePanel.Hide();
		
		VBoxContainer OptionsContainer = GetNode<VBoxContainer>("PausePanel/MarginContainer/VBoxContainer/OptionsContainer");
		
		foreach (Button child in OptionsContainer.GetChildren())
		{
			optionCount++;
			buttons.Add(child);
		}
		
		buttons[0].Modulate = new Color(1, 0, 0);
	}
	
	public override void _Process(double delta){
		
		for (int i = 0; i <=3; i++){
			if(Input.IsActionJustPressed($"Pause{i}")){
				if(isPaused && i == PauserID){
					UnPauseGame();
				}else{
					currentSelection = 0;
					PauseGame();
				}
				PauserID = i;
			}
		}
		
		if(isPaused){
			Vector2 direction = Input.GetVector($"FrogMovementLeft{PauserID}", $"FrogMovementRight{PauserID}", $"FrogMovementUp{PauserID}", $"FrogMovementDown{PauserID}");
			if(Math.Abs(direction.Y) > 0){
				if(!isMenuing){
					buttons[currentSelection].Modulate = new Color(1, 1, 1, 1);
					currentSelection += (direction.Y < 0 ? -1 : 1);
					if(currentSelection> buttons.Count-1){
						currentSelection = 0;
					}
					if(currentSelection<0){
						currentSelection = buttons.Count -1;
					}
						buttons[currentSelection].Modulate = new Color(1, 0, 0);
					}
					
					isMenuing = true;
			}else{
				isMenuing = false;
			}
			
			if(Input.IsActionPressed($"FrogJoin{PauserID}")){
				buttons[currentSelection].EmitSignal("pressed");
			}
		}
	}
	
	void PauseGame(){
		_pausePanel.Show();
		isPaused = true;
		GetTree().Paused = true;
	}
	void UnPauseGame(){
		_pausePanel.Hide();
		GetTree().Paused = false;
		isPaused = false;
	}
	void LoadMainMenu(){
		GetTree().Paused = false;
		_pausePanel.Hide();

		menuScreen = ResourceLoader.Load<PackedScene>("res://Scenes/Menu/main_menu.tscn").Instantiate();
		Node currentScene = GetTree().Root.GetChild(1);
		currentScene.QueueFree();
		GetTree().Root.AddChild(menuScreen);
	}
}

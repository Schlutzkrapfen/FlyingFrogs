using System;
using Godot;

namespace MMJ.Skripts;

public partial class Start : BoxContainer
{ 
	private Node simultaneousScene;
	private readonly Random random = new Random();
	private AudioStreamPlayer soundPlayer;
	private AudioStreamPlayer clickPlayer;
	private AudioStreamPlayer quitPlayer;
	private AudioStreamPlayer YouCantDoThisAlone;
	private AudioStreamPlayer load;

	private  AnimationPlayer[] animation = new AnimationPlayer[4];

	private Label[] text0Player = new Label[4];
	private Label[] text1Player = new Label[4];
	
	private TextureRect[] playerRect = new TextureRect[4];
	
	private Button[] bButton = new Button[4];
	
	private ColorRect gameSettings;
	private ColorRect botSettings;
	private OptionButton gameModeButton;
	private OptionButton pointsSettingsLabel;
	private OptionButton botSettingsLabel;
	int menuState = 0;
	private HBoxContainer whenPlayerIsAlone;
	private HBoxContainer normalPlayerAmount;
	private bool startPossible;
	private bool tutorial;
	
	private TextureRect tutorialRect;
	private Texture2D yButton;
	private Texture2D aButton;
	private settings settings;

	private string joinedTextPlayer0 = "To Play";

	double timeToStart = 3;
	bool timeStartBool ;

	private string joinedText = "Ready";
	private string quitText0 = "Press";
	private string quitText1 = "To Join";
	int joinedBotCount = 0;
	private Color white = new Godot.Color(255,255,255,255);

	public override void _Ready()
	{

		LoadNodeAndTexture();
		CheckIfImportantNodesAreHere();
		
		Global.globalPlayerList[0].isReady = false;
		Global.globalPlayerList[1].isReady = false;
		Global.globalPlayerList[2].isReady = false;
		Global.globalPlayerList[3].isReady = false;
	}

	private void LoadNodeAndTexture()
	{

		animation[0] = GetNode<AnimationPlayer>("HBoxContainer/PanelContainer/AnimationPlayer");
		animation[1] = GetNode<AnimationPlayer>("HBoxContainer/PanelContainer2/AnimationPlayer");
		animation[2] = GetNode<AnimationPlayer>("HBoxContainer/PanelContainer3/AnimationPlayer");
		animation[3] = GetNode<AnimationPlayer>("HBoxContainer/PanelContainer4/AnimationPlayer");
		
		text0Player[0] = GetNode<Label>("HBoxContainer/PanelContainer/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/Label");
		text1Player[0] = GetNode<Label>("HBoxContainer/PanelContainer/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/Label2");
		playerRect[0] = GetNode<TextureRect>("HBoxContainer/PanelContainer/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/TextureRect");
		bButton[0] = GetNode<Button>("HBoxContainer/PanelContainer/HBoxContainer/VBoxContainer/VSplitContainer/Button0");
		
		text0Player[1] = GetNode<Label>("HBoxContainer/PanelContainer2/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/Label");
		text1Player[1] = GetNode<Label>("HBoxContainer/PanelContainer2/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/Label2");
		playerRect[1] = GetNode<TextureRect>("HBoxContainer/PanelContainer2/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/TextureRect");
		bButton[1] = GetNode<Button>("HBoxContainer/PanelContainer2/HBoxContainer/VBoxContainer/VSplitContainer/Button1");
		
		text0Player[2] = GetNode<Label>("HBoxContainer/PanelContainer3/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/Label");
		text1Player[2] = GetNode<Label>("HBoxContainer/PanelContainer3/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/Label2");
		playerRect[2] = GetNode<TextureRect>("HBoxContainer/PanelContainer3/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/TextureRect");
		bButton[2] = GetNode<Button>("HBoxContainer/PanelContainer3/HBoxContainer/VBoxContainer/VSplitContainer/Button2");
		
		text0Player[3] = GetNode<Label>("HBoxContainer/PanelContainer4/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/Label");
		text1Player[3] = GetNode<Label>("HBoxContainer/PanelContainer4/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/Label2");
		playerRect[3] = GetNode<TextureRect>("HBoxContainer/PanelContainer4/HBoxContainer/VBoxContainer/VSplitContainer2/VBoxContainer/TextureRect");
		bButton[3] = GetNode<Button>("HBoxContainer/PanelContainer4/HBoxContainer/VBoxContainer/VSplitContainer/Button3");
		
		yButton = (Texture2D)GD.Load("res://Assets/Buttons/GJ_Knöpfe_Y.png");
		aButton = (Texture2D)GD.Load("res://Assets/Buttons/GJ_Knöpfe_A.png");
		soundPlayer = GetNode<AudioStreamPlayer>("Join");
		clickPlayer = GetNode<AudioStreamPlayer>("../../../MainMenuSound/Click");
		quitPlayer = GetNode<AudioStreamPlayer>("Leaf");
		YouCantDoThisAlone = GetNode<AudioStreamPlayer>("../../../MainMenuSound/YouCantDothisAlone");
		tutorialRect = GetNode<TextureRect>("../TextureRect2");
		load = GetNode<AudioStreamPlayer>("Load");

		gameSettings = GetNode<ColorRect>("../ColorRect");
		botSettings = GetNode<ColorRect>("../ColorRect/ColorRect");
		gameModeButton = GetNode<OptionButton>("../ColorRect/PopUpMenu/VBoxContainer/Panel/VBoxContainer/HBoxContainer/OptionButton");

		pointsSettingsLabel =
			GetNode<OptionButton>("../ColorRect/PopUpMenu/VBoxContainer/Panel/VBoxContainer/HBoxContainer2/OptionButton2");
		botSettingsLabel =
			GetNode<OptionButton>("../ColorRect/ColorRect/PopUpMenu/VBoxContainer/Panel/VBoxContainer/BotCount/OptionButton2");
		whenPlayerIsAlone =
			GetNode<HBoxContainer>("../ColorRect/PopUpMenu/VBoxContainer/Panel/VBoxContainer/HBoxContainer6");
		normalPlayerAmount =
			GetNode<HBoxContainer>("../ColorRect/PopUpMenu/VBoxContainer/Panel/VBoxContainer/HBoxContainer5");
		//I dont know how but i Created a weired new Type I dont know why this is happening here but 
		//it works? Please dont remove the GetNode if it suddenly doesn't Work we can test it here.
		var node = GetNode("../HBoxContainer/HSplitContainer4/Settings");
		//GD.Print(node);
			
		settings =  GetNode<settings>("../HBoxContainer/HSplitContainer4/Settings");
	}
	//checking if nodes that are needed for logic in code are here
	private void CheckIfImportantNodesAreHere()
	{
		if (gameSettings == null)
		{
			//GD.Print("gameSettings was not found #Start.cs");
		}
		if (settings == null)
		{
			//GD.Print("setting was not found #Start.cs");
		}

	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("FrogJoin0")&& !settings.Visible && !tutorialRect.Visible)
		{
			if (gameSettings.Visible)
			{
				return;
			}
			PlayerJoin(0);
		}
		if (@event.IsActionPressed("FrogJoin1"))
		{

			PlayerJoin(1);
			CheckIfEnoughPlayersJoined();
			
		}
		if (@event.IsActionPressed("FrogJoin2"))
		{
			PlayerJoin(2);
			CheckIfEnoughPlayersJoined();
		}
		if (@event.IsActionPressed("FrogJoin3"))
		{
			PlayerJoin(3);
			CheckIfEnoughPlayersJoined();

		}
		if (@event.IsActionPressed("FrogQuit0"))
		{
			if (gameSettings.Visible)
			{
				ChangeMenuState(-1);
				return;
			}
			PlayerQuit(0);
		}
		if (@event.IsActionPressed("FrogQuit1"))
		{
			PlayerQuit(1);
			CheckIfEnoughPlayersJoined();
		}
		if (@event.IsActionPressed("FrogQuit2"))
		{
			PlayerQuit(2);
			CheckIfEnoughPlayersJoined();
		}
		if (@event.IsActionPressed("FrogQuit3"))
		{
			PlayerQuit(3);
			CheckIfEnoughPlayersJoined();
		}
		if(@event.IsActionPressed("Start") && gameSettings.Visible)
		{
		
	
			YouCantDoThisAlone.Play();
		}
		if(@event.IsActionPressed("Start") && Global.globalPlayerList[0].isReady)
		{
			ChangeMenuState(1);
		}
		
		
	}
	private void _on_start_button_up()
	{
		if(clickPlayer != null)
		{
			clickPlayer.Play();
			
		}
		this.Visible = true;
		settings.Visible = false;
	}
	private void StartGame()
	{
		var packedScene = ResourceLoader.Load<PackedScene>(tutorial ? "res://Scenes/Menu/Tutorial.tscn" : "res://Scenes/Level1/Level1.tscn");

		var instance = packedScene.Instantiate();
		Node currentScene = GetTree().Root.GetChild(1);
		currentScene.QueueFree();
		GetTree().Root.AddChild(instance);
	}
	private void OpenGameSettings()
	{
		BotSettings botScript;
		botScript = botSettings as BotSettings;
		botScript.confirm();
		
		CheckIfEnoughPlayersJoined();
		gameSettings.Visible = true;
		pointsSettingsLabel.GrabFocus();
		gameSettings.GetNode<HBoxContainer>("PopUpMenu").Visible = true;
		botSettings.Visible = false;
	}
	private void OpenBotSettings()
	{
		gameSettings.GetNode<HBoxContainer>("PopUpMenu").Visible = false;
		gameSettings.Visible = true;
		botSettings.Visible = true;
		botSettingsLabel.GrabFocus();
		
		BotSettings botScript;
		botScript = botSettings as BotSettings;
		botScript.refresh();
	}
	private void PlayerJoin(int playerId)
	{
		//the first player has spacial treatment could be changed here if we don`t like that
		if(menuState != 0){
			return;
		}
		if (playerId == 0)
		{
			playerRect[playerId].Texture = yButton;
		 	this.Visible = true;
		 	bButton[playerId].Visible = true;
		 	text1Player[playerId].Text = joinedTextPlayer0;
		}
		else
		{
			text0Player[playerId].Text = joinedText;
			text1Player[playerId].Text = "";
			playerRect[playerId].Visible = false;
		}
		string playerStringAnimation = "Player" +playerId.ToString()+"Join/Join";
		animation[playerId].Play(playerStringAnimation);
		bButton[playerId].Visible = true;
		Global.globalPlayerList[playerId].isReady = true;
		Global.globalPlayerList[playerId].isPlayer = true;

		if(soundPlayer != null){
			float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
			soundPlayer.PitchScale = randomPitch;
			soundPlayer.Play();
		}
		
	}
	private void ChangeMenuState(int MenuStateChange){
		menuState += MenuStateChange;
		if(menuState < 0){
			menuState = 0;
		}
		if(menuState > 3){
			menuState = 3;
		}
		switch(menuState){
			case 0:
				gameSettings.Visible = false;
				break;
			case 1:
				OpenBotSettings();
				break;
			case 2: 
				OpenGameSettings();
				break;
			case 3: 
				StartGame();
				break;
		}
	}
	private void CheckIfEnoughPlayersJoined()
	{
		int i = 0;
				//The B buttons gets used for Controlling if enough players are Connected
				foreach (Button b in bButton)
				{
					if (b.Visible)
					{
						i++;
					}
				}
				//if (i >= 2)
				//{
					//startPossible = true;
					//whenPlayerIsAlone.Visible = false;
				   	//normalPlayerAmount.Visible = true;
				//}
				//else
				//{
					//startPossible = false;
					//whenPlayerIsAlone.Visible = true;
					//normalPlayerAmount.Visible = false;
				//}
	}

	private void PlayerQuit(int playerId)
	{
		if(menuState != 0){
			return;
		}
		
			if(this.Visible)
			{
				Global.globalPlayerList[playerId].isReady = false;
				string playerStringAnimation = "Player" +playerId.ToString()+"Join/Quit";
				animation[playerId].Play(playerStringAnimation);
				text0Player[playerId].Text = quitText0;
				text1Player[playerId].Text = quitText1;
				if(quitPlayer != null){
					quitPlayer.Play();
				}
				if (playerId == 0)
				{
					playerRect[playerId].Texture = aButton;
					this.Visible = false;
				}
				else
				{
					playerRect[playerId].Visible = true;
					bButton[playerId].Visible = false;
				}
			}
			

	}
	private void _on_button_0_button_up()
	{
		PlayerQuit(0);
	}
	private void _on_button_1_button_up()
	{
		PlayerQuit(1);
	}
	private void _on_button_2_button_up()
	{
		PlayerQuit(2);
	}
	private void _on_button_3_button_up()
	{
		PlayerQuit(3);
	}

private void _on_check_box_button_up()
{
	tutorial = !tutorial;
	// Replace with function body.
}
}









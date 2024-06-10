using System;
using Godot;

namespace MMJ.Skripts;

public partial class Winscreen : TextureRect
{
	private double[] score;
	private double scoreValue0;
	private double[] scorePercent = new double[4];
	private double[] scoreAdding = new double[4];
	private double scoreSum;
	//time Needed fot the points display the higher the number longer it takes to add the points.
	private double timeNeeded = 2;

	
	private Texture2D pinkFrog;
	private Texture2D blackFrog;
	private Texture2D yellowFrog;
	private Texture2D blueFrog;
	private ColorRect showStatistic;
	private Node menuScreen;
	private TextureProgressBar restartProgress;
	private TextureProgressBar mainMenuProgress;
	private AudioStreamPlayer chargeTime;
	private AudioStreamPlayer pointsCount;
	private VBoxContainer[] playerCounter = new VBoxContainer[4];
	private Label[] playerScoreCounter = new Label[4];
	private TextureProgressBar[] playerBar = new TextureProgressBar[4];
	
	
	private double time = 3;
	// counter for logic in code tracks how long the button is pressed
	private double restartCounter ;
	private double mainMenuCounter ;
	//the time the button needs to be pressed
	private double restartTime = 3;
	private double mainMenuTime = 3;
	private bool restartStartCounter;
	private bool mainMenuStartCounter;
	//private double[] testCase = {10,100,10,10} ;
	private bool countSoundStarted = false;

	public override void _Ready()
	{
		Global.Playeractive = -1;
		
		TestCase(Global.points);
		GetScores();
		LoadTextures();
		LoadNodes();
		

		for (int i = 0; i < Global.globalPlayerList.Length; i++)
		{
			if(!Global.globalPlayerList[i].isReady)
			{
				playerCounter[i].Visible = false;
			}
		}
	}
	private void GetScores()
	{
		
		score = Global.points;
		scoreSum = score[0]+score[1]+score[2]+score[3];
		for (int i = 0; i < scorePercent.Length; i++)
		{
			scorePercent[i] = score[i] / scoreSum;
		}
	}
	private void LoadTextures()
	{
		pinkFrog = (Texture2D)GD.Load("res://Assets/Frosche/GJ_WinnerScreen_Pink.png");
		blackFrog = (Texture2D)GD.Load("res://Assets/Frosche/GJ_WinnerScreen_Schwarz.png");
		yellowFrog = (Texture2D)GD.Load("res://Assets/Frosche/GJ_WinnerScreen_Gelb.png");
		blueFrog = (Texture2D)GD.Load("res://Assets/Frosche/GJ_WinnerScreen_Blau.png");
		if (score[0] >= score[1] && score[0] >= score[2] && score[0] >= score[3])
		{
			this.Texture = pinkFrog;
		}
		else if (score[1] >= score[2] && score[1] >= score[3])
		{
			this.Texture = blackFrog;
		}
		else if (score[2] >= score[3])
		{
			this.Texture = yellowFrog;
		}
		else
		{
			this.Texture = blueFrog;
		}
	}
	private void LoadNodes()
	{
		showStatistic = GetNode<ColorRect>("ColorRect");
		
		playerCounter[0] = GetNode<VBoxContainer>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer");
		playerCounter[1] = GetNode<VBoxContainer>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer2");
		playerCounter[2] = GetNode<VBoxContainer>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer3");
		playerCounter[3] = GetNode<VBoxContainer>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer4");
		playerScoreCounter[0] = GetNode<Label>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer/Label");
		playerScoreCounter[1] = GetNode<Label>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer2/Label");
		playerScoreCounter[2] = GetNode<Label>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer3/Label");
		playerScoreCounter[3] = GetNode<Label>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer4/Label");
		playerBar[0] = GetNode<TextureProgressBar>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer/TextureRect/TextureProgressBar");
		playerBar[1] = GetNode<TextureProgressBar>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer2/TextureRect/TextureProgressBar");
		playerBar[2] = GetNode<TextureProgressBar>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer3/TextureRect/TextureProgressBar");
		playerBar[3] = GetNode<TextureProgressBar>("ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/Control/Panel/HBoxContainer/VBoxContainer4/TextureRect/TextureProgressBar");
		restartProgress =
			GetNode<TextureProgressBar>(
				"ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer2/Restart/HBoxContainer/TextureRect/TextureProgressBar");
		mainMenuProgress =
			GetNode<TextureProgressBar>(
				"ColorRect/VBoxContainer/HBoxContainer/VBoxContainer/HBoxContainer2/Main Menu/HBoxContainer/TextureRect/TextureProgressBar");
		chargeTime = GetNode<AudioStreamPlayer>("../AudioStreamPlayer");
		pointsCount = GetNode<AudioStreamPlayer>("../pointscountin");
	}


	
	public override void _Process(double delta)
	{
		if (time <= 0)
		{
			AddPoints(delta/timeNeeded);
			showStatistic.Visible = true;
		}
		else
		{
			time -= delta;
		}

		if (restartStartCounter)
		{
			restartCounter += delta;
			if (restartCounter >= restartTime)
			{
				menuScreen = ResourceLoader.Load<PackedScene>("res://Scenes/Level1/Level1.tscn").Instantiate();
				Node currentScene = GetTree().Root.GetChild(1);
				currentScene.QueueFree();
				GetTree().Root.AddChild(menuScreen);
			}
		}
		else if (restartCounter >= 0)
		{
			restartCounter -= delta;
		}

		restartProgress.Value = restartCounter * 10;
		if (mainMenuStartCounter)
		{
			mainMenuCounter += delta;
			if (mainMenuCounter >= mainMenuTime)
			{
				menuScreen = ResourceLoader.Load<PackedScene>("res://Scenes/Menu/main_menu.tscn").Instantiate();
				Node currentScene = GetTree().Root.GetChild(1);
				currentScene.QueueFree();
				GetTree().Root.AddChild(menuScreen);
			}
		}
		else if (mainMenuCounter >= 0)
		{
			mainMenuCounter -= delta;
		}
		mainMenuProgress.Value = mainMenuCounter * 10;
	}


	private void _on_restart_button_up()
	{
		menuScreen = ResourceLoader.Load<PackedScene>("res://Scenes/Level1/Level1.tscn").Instantiate();
		Node currentScene = GetTree().Root.GetChild(1);
		currentScene.QueueFree();
		GetTree().Root.AddChild(menuScreen);
	}
	private void _on_main_menu_button_up()
	{
		menuScreen = ResourceLoader.Load<PackedScene>("res://Scenes/Menu/main_menu.tscn").Instantiate();
		Node currentScene = GetTree().Root.GetChild(1);
		currentScene.QueueFree();
		GetTree().Root.AddChild(menuScreen);
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("FrogJoin0"))
		{
			restartStartCounter = true;
			chargeTime.Play();
			if (time < 2)
			{
				time = 0;
			}
		}
		if (@event.IsActionReleased("FrogJoin0"))
		{
			restartStartCounter = false;
			chargeTime.Stop();
		}
	
		if (@event.IsActionPressed("FrogQuit0"))
		{
			mainMenuStartCounter = true;
			chargeTime.Play();
		}
		if (@event.IsActionReleased("FrogQuit0"))
		{
			mainMenuStartCounter = false;
			chargeTime.Stop();
		}
	}
/// <summary>
/// addPoints is used to change how far the progressbar and the Points are on the label
/// </summary>
/// <param name="delta"></param>
	private void AddPoints(double delta)
	{
		
		for (int i = 0; i < playerScoreCounter.Length; i++)
		{
			
			if (scorePercent[i] > scoreAdding[i])
			{
				scoreAdding[i] += delta;
				Godot.Vector2 changePosition = new Vector2(playerBar[i].Position.X,
					playerBar[i].Position.Y - (float)delta * playerBar[i].Size.Y);
				playerBar[i].SetPosition(changePosition) ; 
				playerBar[i].Value = scoreAdding[i]*100;
				double s = scoreAdding[i] * scoreSum;
				int scoreInt = (int)s;
				playerScoreCounter[i].Text = scoreInt.ToString();
				if (!countSoundStarted)
				{
					countSoundStarted = true;
					pointsCount.Play();
				}
				
			}
			else
			{
				playerBar[i].Value = scorePercent[i]*100;
				int s = (int)score[i];
				playerScoreCounter[i].Text = s.ToString();
			}
		}
		
	}

	/// <summary>
	/// Is used to test the script that every player gets points that you can decided
	/// </summary>
	/// <param name="playerScore"></param>
	private void TestCase(double[] playerScore)
	{
		for (int i = 0; i < playerScore.Length; i++)
		{
			if (playerScore[i] == 0)
			{
				//Global.globalPlayerList[i].isReady = false;
			}
			else
			{
				//Global.globalPlayerList[i].isReady = true;
				Global.points[i]= playerScore[i];
			}
			
		}
		
	}


}

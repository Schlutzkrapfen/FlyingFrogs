using Godot;
using System;
using System.Collections.Generic;
public partial class Ui : Control
{
	//gets overwritten with a Globel
	double timeLeft;
	Label timer;
	Label player0;
	Label player1;
	Label player2;
	Label player3;
	int playerWinning = 4;
	public Node WinScreen;
	public AnimationPlayer animation;
	double score0;
	double score1;
	double score2;
	double score3;

	public override void _Ready()
	{
		timeLeft = Global.playTime;
		animation = GetNode<AnimationPlayer>("AnimationPlayer");
		timer = GetNode<Label>("HBoxContainer/VBoxContainer/Timer2");
		player0 = GetNode<Label>("HBoxContainer/Player0");
		player1 = GetNode<Label>("HBoxContainer/Player1");
		player2 = GetNode<Label>("HBoxContainer/Player2");
		player3 = GetNode<Label>("HBoxContainer/Player3");
		animation.Play("Timer");
	}

	public override void _Process(double delta)
	{
		timeLeft -= delta;
		int minutes = (int)timeLeft / 60;
		int seconds = (int)timeLeft % 60;
		if (seconds >= 10)
		{
			timer.Text = "0" +minutes + ":" + seconds;
		}
		else
		{
			timer.Text = "0" +minutes + ":0" +  seconds;
		}
		if (timeLeft <= 0)
		{
		Global.points[0] = score0;
		Global.points[1] = score1;
		Global.points[2] = score2;
		Global.points[3] = score3;
		WinScreen = ResourceLoader.Load<PackedScene>("res://Scenes/EndScreen/Winscreen.tscn").Instantiate();
		Node currentScene = GetTree().Root.GetChild(1);
		currentScene.QueueFree();
		GetTree().Root.AddChild(WinScreen);

		}
		playerWinning = Global.Playeractive;
		switch (playerWinning)
		{
			case 0:
				score0 += delta;
				int score = (int)score0;
				player0.Text = FormatScore(score);
				break;
			case 1:
				score1 += delta;
				score = (int)score1;
				player1.Text = FormatScore(score);
				break;
			case 2:
				score2 += delta;
				score = (int)score2;
				player2.Text = FormatScore(score);
				break;
			case 3:
				score3 += delta;
				score = (int)score3;
				player3.Text = FormatScore(score);
				break;
		}
	}

	private string FormatScore(int score)
	{
		string scoreString = score.ToString();
		if (score <= 99)
		{
			if (score > 9)
			{
				scoreString = "0" + scoreString;
			}
			else
			{
				scoreString = "00" + scoreString;
			}
		}
		
		return scoreString;
	}
}


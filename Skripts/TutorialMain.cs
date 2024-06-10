using Godot;
using System;

public partial class TutorialMain : CanvasLayer
{
	private bool[] playerReady = new bool[4];
	private TextureRect[] ReadyPoint = new TextureRect[4];
	private readonly Random random = new Random();
	private AudioStreamPlayer clickPlayer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		
		GetNodes();
		CheckPlayer();
		CheckIfEveryOneIsReady();
	}

	private void CheckPlayer()
	{
		for(int i = 0; i <4; i++){
			if (!Global.globalPlayerList[i].isReady || !Global.globalPlayerList[i].isPlayer)
			{
				playerReady[i] = true;
			}
		}

	}
	private void GetNodes()
	{
		ReadyPoint[0] = GetNode<TextureRect>("TextureRect/Button/TextureRect");
		ReadyPoint[1] = GetNode<TextureRect>("TextureRect/Button/TextureRect2");
		ReadyPoint[2] = GetNode<TextureRect>("TextureRect/Button/TextureRect3");
		ReadyPoint[3] = GetNode<TextureRect>("TextureRect/Button/TextureRect4");
		clickPlayer = GetNode<AudioStreamPlayer>("TextureRect/Click");
		
	}
	private void CheckIfEveryOneIsReady()
	{

		foreach (bool ready in playerReady)
		{
			if (!ready)
			{
				return;
			}
		}
		StartTutorial();
		
	}

	private void StartTutorial()
	{
		var packedScene = ResourceLoader.Load<PackedScene>("res://Scenes/Level1/Level1.tscn");
		var instance = packedScene.Instantiate();
		Node currentScene = GetTree().Root.GetChild(1);
		currentScene.QueueFree();
		GetTree().Root.AddChild(instance);
	}
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("FrogJoin0") && Global.globalPlayerList[0].isReady)
		{
			
			playerReady[0] = !playerReady[0];
			ReadyPoint[0].Visible = playerReady[0];
			PlayClickSound();
			CheckIfEveryOneIsReady();
		}
		if (@event.IsActionPressed("FrogJoin1")&& Global.globalPlayerList[1].isReady)
		{
			
			playerReady[1] = !playerReady[1];
			ReadyPoint[1].Visible = playerReady[1];
			PlayClickSound();
			CheckIfEveryOneIsReady();
		}
		if (@event.IsActionPressed("FrogJoin2")&& Global.globalPlayerList[2].isReady)
		{
			
			playerReady[2] = !playerReady[2];
			ReadyPoint[2].Visible = playerReady[2];
			PlayClickSound();
			CheckIfEveryOneIsReady();
		}
		if (@event.IsActionPressed("FrogJoin3")&& Global.globalPlayerList[3].isReady)
		{
			
			playerReady[3] = !playerReady[3];
			ReadyPoint[3].Visible = playerReady[3];
			PlayClickSound();
			CheckIfEveryOneIsReady();
		}
	}

	private void PlayClickSound()
	{
		if(clickPlayer != null){
			float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
			clickPlayer.PitchScale = randomPitch;
			clickPlayer.Play();
		}
	}
}

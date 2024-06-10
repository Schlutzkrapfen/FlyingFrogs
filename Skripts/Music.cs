using Godot;
using System;

public partial class Music : Node2D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Test(60);
		if (Global.playTime == 60)
		{
			GetNode<AudioStreamPlayer>("BackgroundMusic1Min").Play();
		}
		else if(Global.playTime == 120)
		{
		  GetNode<AudioStreamPlayer>("BackgroundMusic2Min").Play();
		}
		else if(Global.playTime == 180)
		{
			
		  GetNode<AudioStreamPlayer>("BackgroundMusic3Min").Play();
		}
		  
	}

	public void Test(double time)
	{
		Global.playTime = time;
	}
}

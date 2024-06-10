using Godot;
using System;

public partial class ParticleEmitter : CpuParticles2D
{
	float dashTime;
	float passedTime;
	
	public override void _Ready(){
		Emitting = false;
		
		passedTime = 0;
	}
	public void SetDashTime(float time){
		dashTime = time;
	}
	public void Dash(Vector2 velocity){
		passedTime = 0;
		float randomX = ((float)GD.Randi() % 10)/30;
		float randomY = ((float)GD.Randi() % 10)/30;
		
		Direction = new Vector2(velocity.X * randomX, velocity.Y * randomY);
		SpeedScale = 1;
		Emitting = true;
		//Scale = new Vector2((float)0.001,(float)0.001);
	}
	public override void _PhysicsProcess(double delta){
		passedTime += (float)delta;
		
		if(passedTime > dashTime){
			Emitting = false;
		}
	}
	
	public void SetPlayerTexture(int PiD){
		Texture2D newTexture = (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_01.png");
		switch (PiD){
			case 0:
				newTexture = (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_Pink.png");
				break;
			case 1:
				newTexture = (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_Schwarz.png");
				break;
			case 2:
				newTexture = (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_Gelb.png");
				break;
			case 3:
				newTexture = (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_Blau.png");
				break;
		}
		   if (newTexture != null)
	{
		Texture = newTexture;
		Emitting = false;
		Restart();
	}
	}
}

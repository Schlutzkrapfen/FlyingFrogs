using Godot;
using System;

public partial class TransitionEffect : Node2D
{
	private float elapsedTime = 0;
	private float duration = 0.2f;
	
	private CpuParticles2D _SwapParticles;
	private CpuParticles2D _ParticleTrail;
	private CpuParticles2D _DisappearParticles;
	private CpuParticles2D _ReappearParticles;
	private ColorRect _ColRect;
	
	private Vector2 startPos;
	private Vector2 endPos;
	
	float maxRadius = 300;
	float minRadius = 10;
	
	float minReturnScale = 0.05f;
	float maxReturnScale = 0.1f;
	
	bool treePauseState = false;
	
	public override void _Ready(){
		_SwapParticles = GetNodeOrNull<CpuParticles2D>("SwapParticles");
		_ParticleTrail = GetNodeOrNull<CpuParticles2D>("SwapParticles/ParticleTrail");
		_DisappearParticles = GetNodeOrNull<CpuParticles2D>("DisappearParticles");
		_ReappearParticles = GetNodeOrNull<CpuParticles2D>("ReappearParticles");
		_ColRect = GetNodeOrNull<ColorRect>("ColorRect");
		
		_ParticleTrail.Hide();
		_SwapParticles.Hide();
		_DisappearParticles.Hide();
		_ReappearParticles.Hide();
		_ColRect.Hide();
		SetEnabled(false);
	}
	public override void _Process(double delta){
		if(elapsedTime > duration){
			EndTransition();

		}else
		{
			elapsedTime += (float)delta;
			DoParticleStuff();
		}
	}
	
	public void Transition(Vector2 FlyPos, Vector2 CaughtFrog, int CaughtFrogID, Vector2 ReturningFrog, int ReturningFrogID){
		_SwapParticles.Texture = LoadParticles(CaughtFrogID);
		_ParticleTrail.Texture = LoadParticles(CaughtFrogID);
		_DisappearParticles.Texture = LoadParticles(CaughtFrogID);
		_ReappearParticles.Texture =  LoadParticles(ReturningFrogID);
		//_ParticleTrail.SpeedScale = 5;
		_ParticleTrail.Amount = (int)(FlyPos-CaughtFrog).Length() / 25;
		_SwapParticles.Restart();
		_ParticleTrail.Restart();
		_DisappearParticles.Restart();
		_ReappearParticles.Restart();
		elapsedTime = 0;
		
		_ParticleTrail.Show();
		_SwapParticles.Show();
		_DisappearParticles.Show();
		if(ReturningFrogID>= 0){
			_ReappearParticles.Show();
			_ReappearParticles.Position = ReturningFrog;
		}
		
		//_ColRect.Show();
		
		endPos = FlyPos;
		startPos = CaughtFrog;
		_DisappearParticles.Position = startPos;
		_SwapParticles.Position = startPos;
		SetEnabled(true);
		//GetTree().Paused = true;
		DoPause(true);
	}
	private Texture2D LoadParticles(int id){
		switch (id){
			case 0:
				return (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_Pink.png");
			case 1:
				return (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_Schwarz.png");
			case 2:
				return (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_Gelb.png");
			case 3:
				return (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_Blau.png");
			default :
				return (Texture2D)GD.Load("res://FlyAssets/EmissionSprites/GJ_Staubkorn_01.png");
		}
	}
	public void EndTransition(){
		SetEnabled(false);
		_ParticleTrail.Hide();
		_SwapParticles.Hide();
		_ReappearParticles.Hide();
		_DisappearParticles.Hide();
		
		_ColRect.Hide();
		DoPause(false);

		//GetTree().Paused = false;
	}
	public void DoParticleStuff(){
		float t = (elapsedTime / duration);
		t = 1 - Mathf.Pow(1-t,2);
		
		Vector2 newPosition = startPos.Lerp(endPos, t);	
		_SwapParticles.Position = newPosition;
		
		float _radius = maxRadius + (minRadius - maxRadius) * t * 2;
		if(_radius < minRadius){
				_radius = minRadius;
		}
		
		_ParticleTrail.Set("emission_sphere_radius", _radius);
		
		Vector2 velocity = (startPos-endPos).Normalized() * 20;
			
		float randomX = ((float)GD.Randi() % 10)/30;
		float randomY = ((float)GD.Randi() % 10)/30;
	
		_ParticleTrail.Direction = new Vector2(-velocity.X * randomX, -velocity.Y * randomY);
		
		float _returnScale = maxReturnScale + (minReturnScale - maxReturnScale) * t;
		//_ReappearParticles.Scale = new Vector2(_returnScale,_returnScale);
	}
	
	private void SetEnabled(bool enabled){
		_ParticleTrail.Emitting = enabled;
		_SwapParticles.Emitting = enabled;
		_ReappearParticles.Emitting = enabled;
		_DisappearParticles.Emitting = enabled;
	}
	
	public void UpdateFlyPos(Vector2 pos){
		endPos = pos;
	}
	private void DoPause(bool pauseState){
		//GetTree().Paused = pauseState;
		//I dont like this :(
	}
}

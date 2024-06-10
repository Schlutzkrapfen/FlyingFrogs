using Godot;
using System;
using System.Collections.Generic;

public partial class FlyController : CharacterBody2D
{
	public const float Speed = 500.0f;
	public const float MaxSpeed = 500.0f;
	public List<List<Vector2>> frogPositions = new List<List<Vector2>>();
	double dashTime;
	float dashCoolDown = 0.9f;
	float dashDuration = 0.3f;
	float dashSpeed = 3000.0f;

	int playerID;

	private ParticleEmitter Particles;
	private FlySprite2D FlySprite;

	private Random random = new Random();
	
	private AudioStreamPlayer2D soundPlayer;
	bool isBot = false;
	byte difficulty = 0;
	Vector2 StartingPos;
	float DistanceEval = 10;
	
	float iframeTime = 0;
	float maxIframes = 0.2f;
	bool hasIframes = false;
	public override void _Ready()
	{
		dashTime = 100;
		playerID = -1;
		StartingPos = Position;
		Particles = GetNodeOrNull<ParticleEmitter>("ParticleEmitter");
		
		if (Particles == null)
		{
			//GD.Print("Error: ParticleEmitter node not found or ParticleEmitter script not attached.");
		}
		else
		{
			Particles.SetDashTime(dashDuration);
		}

		FlySprite = GetNodeOrNull<FlySprite2D>("FlySprite2D");

		if (FlySprite == null)
		{
			//GD.Print("Error: ParticleEmitter node not found or ParticleEmitter script not attached.");
		}
		else
		{
			FlySprite.LoadNewTexture("res://FlyAssets/FlySprites/GJ_Fliege_Basic_Spritesheet.png");
		}
		
		soundPlayer = GetNode<AudioStreamPlayer2D>("FlySound");
		Global.Playeractive = playerID;
	}

	public override void _PhysicsProcess(double delta)
	{
		if(isBot){BotUpdate(delta); }else{		PlayerUpdate(delta);}
		if(hasIframes){
			if(maxIframes < iframeTime){
				hasIframes = false;
			}
			iframeTime += (float)delta;
		}
	}
	private void PlayerUpdate(double delta){
		if(playerID >= 0){
			Vector2 direction = Input.GetVector($"FrogMovementLeft{playerID}", $"FrogMovementRight{playerID}", $"FrogMovementUp{playerID}", $"FrogMovementDown{playerID}");
			bool wantsToDash = Input.IsActionPressed($"FrogJoin{playerID}");
			MoveAndDash(direction,wantsToDash,delta);
		}
	}
	private void BotUpdate(double delta){
		List<Tuple<Vector2, float, float>> ToungeDodgeVals = new List<Tuple<Vector2, float, float>>(); //Ideal direction, length of vec, weight
		
		if(frogPositions.Count == 0){return;}
		int iterator = 0;
		foreach(List<Vector2> ToungeFrogPair in frogPositions){
			if(ToungeFrogPair.Count == 0|| playerID == iterator || !Global.globalPlayerList[iterator].isReady){
				iterator ++;
				continue;
			}
			
			Vector2 P = ToungeFrogPair[1];
			Vector2 X = Position;
			Vector2 V = P - ToungeFrogPair[0];
			Vector2 Q = X - P;  // Vector from P to X
			float dot = Q.Dot(V);
			float VdotV = V.Dot(V);
			Vector2 proj = (dot / VdotV) * V;  // Projection of Q onto V
			float distance = (X - proj).Length();
			Vector2 AvoidVec = new Vector2(-V.Y,V.X);
			
			float weight = 0f + 2 / (difficulty+1);
			if((X-P).Length() < (ToungeFrogPair[0]-P).Length() + 100){
				weight = 5;
			}
			ToungeDodgeVals.Add(Tuple.Create(AvoidVec.Normalized(), distance, weight));
			iterator ++;
		}
		Vector2 DistToCenter = Position - StartingPos;
		float distanceToCenterEval = 0.4f;
		if(DistToCenter.Length() > 250){
			ToungeDodgeVals.Add(Tuple.Create(DistToCenter.Normalized(), 1/DistToCenter.Length(), distanceToCenterEval));
		}

		Vector2 direction = new Vector2(0,0);
		bool wantsToDash = false;
		
		foreach(Tuple<Vector2, float, float> item in ToungeDodgeVals){
			direction += -item.Item1 * 1/(item.Item2 / DistanceEval) * item.Item3;

			if(item.Item2<400 && item.Item3>3){
				wantsToDash = true;
				if(dashTime <0.3){
					direction = item.Item1;
					direction += DistToCenter.Normalized() * distanceToCenterEval * 0.1f;
				}
			}
		}
		
		MoveAndDash(direction,wantsToDash,delta);
	}
	private void MoveAndDash(Vector2 direction, bool dash, double delta){
		direction = direction.Normalized();
		float deltaSpeed = Speed * (float)delta;
		float deltaDashSpeed = dashSpeed * (float)delta;

		dashTime += delta;
		Vector2 velocity = Velocity;
		
		if (direction != Vector2.Zero)
		{
			if (dashTime < dashDuration)
			{
				velocity.X = Mathf.MoveToward(Velocity.X, direction.X * dashSpeed, deltaDashSpeed);
				velocity.Y = Mathf.MoveToward(Velocity.Y, direction.Y * dashSpeed, deltaDashSpeed);
			}
			else if (velocity.X * velocity.X + velocity.Y * Velocity.Y < MaxSpeed * MaxSpeed)
			{
				velocity.Y += direction.Y * deltaSpeed;
				velocity.X += direction.X * deltaSpeed;
			}
			else
			{
				velocity += direction * deltaSpeed;
				velocity = velocity / (velocity.X * velocity.X + velocity.Y * velocity.Y) * MaxSpeed * MaxSpeed;
			}

			if (direction.X < 0)
			{
				FlySprite.SetMirrored();
			}
			else
			{
				FlySprite.SetNotMirrored();
			}
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, deltaSpeed / 10);
			velocity.Y = Mathf.MoveToward(Velocity.Y, 0, deltaSpeed / 10);
		}
		Velocity = velocity;
		if (dashTime >= dashCoolDown && dash)
			{
				dashTime = 0;
				Dash(direction);
			}

			MoveAndSlide();
	}
	public void SetPlayerID(int PiD)
	{
		if(!Global.globalPlayerList[PiD].isReady || hasIframes){
			return;
		}
		
		switch (PiD)
		{
			case 0:
				FlySprite.LoadNewTexture("res://FlyAssets/FlySprites/GJ_Fliege_Pink_Spritesheet.png");
				break;
			case 1:
				FlySprite.LoadNewTexture("res://FlyAssets/FlySprites/GJ_Fliege_Schwarz_Spritesheet.png");
				break;
			case 2:
				FlySprite.LoadNewTexture("res://FlyAssets/FlySprites/GJ_Fliege_Gelb_Spritesheet.png");
				break;
			case 3:
				FlySprite.LoadNewTexture("res://FlyAssets/FlySprites/GJ_Fliege_Blau_Spritesheet.png");
				break;
			default:
				//GD.Print("Not a valid Player");
				break;
		}
		Particles.SetPlayerTexture(PiD);
		dashTime = 100;

		// PiD switch and position reset
		SetPositionOldPlayer();
		Global.Playeractive = PiD;
		Global.playerFlyPos = new Vector2(Position.X, Position.Y);
		
		playerID = PiD;
		isBot = !Global.globalPlayerList[PiD].isPlayer;
		if(isBot){
			difficulty = Global.globalPlayerList[PiD].difficulty;
		}
		
		iframeTime = 0;
		hasIframes = true;
	}


	private void SetPositionOldPlayer()
	{
		if (Global.Playeractive != -1)
		{
			string path = $"../frogController/player_controller{Global.Playeractive}/player";
			CharacterBody2D player = GetNode<CharacterBody2D>(path);
			player.GlobalPosition = Global.playerFlyPos;
		}
	}



	void HandleWallCollisions(){
		
	}
	private void Dash(Vector2 velocity)
	{

		if (Particles != null)
		{
			Particles.Dash(velocity);
		}
		
		if(soundPlayer != null){
			float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;
	
			soundPlayer.PitchScale = randomPitch;
			soundPlayer.Play();
		}
	}
	public int GetCurrentPlayer(){
		
		return playerID;
	}
}


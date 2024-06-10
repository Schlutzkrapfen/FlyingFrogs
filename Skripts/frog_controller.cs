using Godot;
using System;

public partial class frog_controller : Node2D
{
	private  Node2D player0;
	private  Node2D player1;
	private  Node2D player2;
	private  Node2D player3;
	
	private CollisionShape2D[] playerCollisionBox = new CollisionShape2D[4];
	// Called when the node enters the scene tree for the first time.
	FlyController fly;
	
	public override void _Ready()
	{
		playerCollisionBox[0] = GetNode<CollisionShape2D>("../StaticBody2D2/CollisionShape2D");	
		playerCollisionBox[1] = GetNode<CollisionShape2D>("../StaticBody2D3/CollisionShape2D");	
		playerCollisionBox[2] = GetNode<CollisionShape2D>("../StaticBody2D5/CollisionShape2D");	
		playerCollisionBox[3] = GetNode<CollisionShape2D>("../StaticBody2D4/CollisionShape2D");	
	player0 = GetNode<Node2D>("player_controller0");
	player1 = GetNode<Node2D>("player_controller1");
	player2 = GetNode<Node2D>("player_controller2");
	player3 = GetNode<Node2D>("player_controller3");
	
	fly = GetNodeOrNull<FlyController>("Fly");
	
		DeaktivtedNotJoinedPlayers();
	}
	


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Global.Playeractive == 0)
		{
			player0.Visible = false;
			player1.Visible = true;
			player2.Visible = true;
			player3.Visible = true;
		}
		if (Global.Playeractive == 1)
		{
			player0.Visible = true;
			player1.Visible = false;
			player2.Visible = true;
			player3.Visible = true;
		}
		if (Global.Playeractive == 2)
		{
			player0.Visible = true;
			player1.Visible = true;
			player2.Visible = false;
			player3.Visible = true;
		}
		if (Global.Playeractive == 3)
		{
			player0.Visible = true;
			player1.Visible = true;
			player2.Visible = true;
			player3.Visible = false;
		}
		DeaktivtedNotJoinedPlayers();
	}
	public void DeaktivtedNotJoinedPlayers()
	{
		if(!Global.globalPlayerList[0].isReady)
		{
			playerCollisionBox[0].Disabled = true;
			player0.Visible = false;
		}
		if(!Global.globalPlayerList[1].isReady)
		{
			playerCollisionBox[1].Disabled = true;
			player1.Visible = false;
		}
		if(!Global.globalPlayerList[2].isReady)
		{
			playerCollisionBox[2].Disabled = true;
			player2.Visible = false;
		}
		if(!Global.globalPlayerList[3].isReady)
		{
			playerCollisionBox[3].Disabled = true;
			player3.Visible = false;
		}
	}
}

using Godot;
using System;
using System.Collections.Generic;
using MMJ.Skripts;

public partial class ObserverPosition : Node2D
{
	private CharacterBody2D Fly;
	private FlyController FlyScript;
	private TransitionEffect TransitionScript;
	
	private List<List<CharacterBody2D>> FrogPlayerPos = new List<List<CharacterBody2D>>();
	private int lastFlyID = -1;
	
	private Vector2 returningFrog = new Vector2(0,0);
	private Vector2 transitioningFrog = new Vector2(0,0);
	
	public override void _Ready(){
		Fly = GetNodeOrNull<CharacterBody2D>("Fly");
		FlyScript = Fly as FlyController;
		Node2D FrogContainer = GetNode<Node2D>("frogController");
		
		Node2D TransitionEffectScene = GetNode<Node2D>("TransitionEffect");
		TransitionScript = TransitionEffectScene as TransitionEffect;
		 
		foreach (Node2D child in FrogContainer.GetChildren())
		{
			List<CharacterBody2D> t_list = new List<CharacterBody2D>();
			foreach(Node item in child.GetChildren()){
				if(item is CharacterBody2D charBod){
					t_list.Add(charBod);
				}
			}
			
			FrogPlayerPos.Add(t_list);
			
		
		}
		int iterator = 0;
		foreach(List<CharacterBody2D> item in FrogPlayerPos){
			if(Global.globalPlayerList[iterator].isReady){
			item[0].Call("SetIsBot", Global.globalPlayerList[iterator].isPlayer);
			item[0].Call("SetDifficulty", (float)Global.globalPlayerList[iterator].difficulty);
			iterator++;
			}
		}
	}
	public override void _Process(double delta){
		
		List<List<Vector2>> t_posList = new List<List<Vector2>>();

		foreach(List<CharacterBody2D> t_list in FrogPlayerPos){
			List<Vector2> t_singlepos = new List<Vector2>();
			if(t_list.Count > 0){
				t_singlepos.Add(t_list[0].GlobalPosition); //Crosshair
				t_singlepos.Add(t_list[1].GlobalPosition); //FrogCar

			}
			t_posList.Add(t_singlepos);
		}
		FlyScript.frogPositions = t_posList;
		
		foreach(List<CharacterBody2D> t_list in FrogPlayerPos){
			t_list[0].Call("SetFlyPosition", Fly.Position);
		}
		
		if(lastFlyID != FlyScript.GetCurrentPlayer()){
			int currentPlayer = FlyScript.GetCurrentPlayer();
			int iterator = 0;
			
			
			foreach(List<CharacterBody2D> item in FrogPlayerPos){
				
				if(Global.globalPlayerList[iterator].isReady){
					if(currentPlayer == iterator){
						item[0].Call("SetIsFly", true);
						transitioningFrog = item[1].GlobalPosition;
					}
					else if(lastFlyID == iterator){
							returningFrog = item[1].GlobalPosition;
							item[0].Call("SetIsFly", false);
					}
					iterator ++;
				}
			}
			
			TransitionScript.Transition(Fly.Position, transitioningFrog, currentPlayer, returningFrog, lastFlyID);
			lastFlyID = currentPlayer;
			}
			TransitionScript.UpdateFlyPos(Fly.Position);
	}
}

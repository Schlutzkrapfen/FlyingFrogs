using Godot;
using System;

public partial class tongueColls : Area2D
{
	int player_id;
	private Random random = new Random();
	
	private AudioStreamPlayer2D soundPlayer;
	public override void _Ready(){
		soundPlayer = GetNode<AudioStreamPlayer2D>("CatchFly");
	}
	
	private void _on_area_entered(Area2D area)
	{
		if(area.Name == "FlyArea2D"){
			Node parent = area.GetParent();

			if (parent != null)
			{
				string parentClass = parent.GetType().Name;

				switch (parentClass)
				{
					case "FlyController":
						((FlyController)parent).SetPlayerID(player_id);
				 	break;
				}
			}
			if(soundPlayer != null){
				float randomPitch = (float)random.NextDouble() * 1.5f + 0.5f;

				soundPlayer.PitchScale = randomPitch;
				soundPlayer.Play();
			}

		}

	}

}






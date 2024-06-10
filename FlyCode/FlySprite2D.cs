using Godot;
using System;

public partial class FlySprite2D : Sprite2D
{
	bool mirrored = false;
	public void SetMirrored(){
		if(!mirrored){
			Scale = new Vector2(-Scale.X, Scale.Y);
		}
		mirrored = true;
	}
	public void SetNotMirrored(){
		if(mirrored){
			Scale = new Vector2(-Scale.X, Scale.Y);
		}
		mirrored = false;
	}
	public void LoadNewTexture(string texturePath)
	{
		Texture2D newTexture = GD.Load<Texture2D>(texturePath);

		Texture = newTexture;
	}
}

using Godot;
using System;
using MMJ.Skripts;

public partial class Global : Node
{

	public static GlobalPlayerList[] globalPlayerList = new GlobalPlayerList[4] ;
	static Global()
	{
		for (int i = 0; i < globalPlayerList.Length; i++)
		{
			globalPlayerList[i] = new GlobalPlayerList();
		}
	}
	public static Vector2 playerFlyPos = new(0, 0);
	public static int Playeractive = -1;
	public static double[] points = new double[4];
	
	public static double playTime = 120;
	public static int gameMode = 0;
}

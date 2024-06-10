using Godot;
using System;
using MMJ.Skripts;

public partial class BotSettings : ColorRect
{
	private OptionButton BotCount;
	private OptionButton DifficultySettings;
	private Panel WarningLabel;
	
	private int currentSettings ;
	double lastUpdate = 0;

	int currentSelectedBotCount = 0;
	int maxBots = 3;
	int minBots = 0;
	
	int currentSelectedDifficulty = 1;
	int maxDifficultyOptions = 3;
	public override void _Ready()
	{
		GetNodes();
	}

	private void GetNodes()
	{
		BotCount = 
			GetNode<OptionButton>("PopUpMenu/VBoxContainer/Panel/VBoxContainer/BotCount/OptionButton2");
		DifficultySettings =
			GetNode<OptionButton>("PopUpMenu/VBoxContainer/Panel/VBoxContainer/Difficulty/OptionButton2");
		WarningLabel =
			GetNode<Panel>("PopUpMenu/VBoxContainer/Control/WarningLabel");
	}

	public override void _Process(double delta)
	{
		lastUpdate += delta;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_left") && lastUpdate >= 0.2 )
		{
			lastUpdate = 0;
			if(BotCount.HasFocus())
			{
				ChangeBotCount(-1);
			}
			if (DifficultySettings.HasFocus())
			{
				ChangeDifficulty(-1);
			}
			
		}
		if (@event.IsActionPressed("ui_right") && lastUpdate >= 0.2)
		{
			lastUpdate = 0;
			if(BotCount.HasFocus())
			{
				ChangeBotCount(+1);
			}
			if (DifficultySettings.HasFocus())
			{
				ChangeDifficulty(+1);
			}
		}
	}

	private void ChangeBotCount(int value)
	{
		currentSelectedBotCount += value;
		if(currentSelectedBotCount < minBots){
			currentSelectedBotCount = maxBots;
		}
		if(currentSelectedBotCount > maxBots){
			currentSelectedBotCount = minBots;
		}
		BotCount.Select(currentSelectedBotCount);

	}

	private void ChangeDifficulty(int value)
	{
		currentSelectedDifficulty += value;
		if(currentSelectedDifficulty < 0){
			currentSelectedDifficulty = maxDifficultyOptions-1;
		}
		if(currentSelectedDifficulty > maxDifficultyOptions-1){
			currentSelectedDifficulty = 0;
		}
		DifficultySettings.Select(currentSelectedDifficulty);
	}

public void refresh(){
	maxBots = 0;
	minBots = 0;
	foreach(GlobalPlayerList item in Global.globalPlayerList){
		if(!item.isReady || !item.isPlayer){
			maxBots++;
		}
	}
	if(maxBots >= 3){
		maxBots = 3;
		minBots = 1;
		currentSelectedBotCount = 1;
		BotCount.SetItemDisabled(0, true);
		BotCount.Select(currentSelectedBotCount);
		WarningLabel.Show();
	}else{
		currentSelectedBotCount = 0;
		BotCount.Select(currentSelectedBotCount);
		BotCount.SetItemDisabled(0, false);
		WarningLabel.Hide();
	}
	for (int i = 3; i>maxBots; i--){
		BotCount.SetItemDisabled(i, true);
	}
}
public void confirm(){
	int iterator = 0;
	foreach(GlobalPlayerList item in Global.globalPlayerList){
		if(!item.isReady || !item.isPlayer){
			iterator ++;
			item.isReady = true;
			item.isPlayer = false;
			item.difficulty = (byte)currentSelectedDifficulty;
		}
		if (iterator >= currentSelectedBotCount){
			return;
		}
	}
}
private void _on_texture_button_button_up()
{
	ChangeBotCount(-1);
}

private void _on_texture_button_2_button_up()
{
	ChangeBotCount(+1);
}
private void _on_texture_button_3_button_up()
{
	ChangeDifficulty(-1);
}
private void _on_texture_button_4_button_up()
{
	ChangeDifficulty(+1);
}

public void OnBotSelected(int index){
	currentSelectedBotCount = index;
}
public void OnDifficultySelected(int index){
	currentSelectedBotCount = index;
}



}


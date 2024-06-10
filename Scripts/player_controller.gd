extends Node2D

@export var thisplayer_id : int = 0

var child_self : CharacterBody2D = null
var child_sprite : Sprite2D = null 

func _ready() -> void:
	
	#print(thisplayer_id)
	$player.player_id = thisplayer_id
	#print($player.player_id)
	$Frogcar.player_id = thisplayer_id
	
				
func set_sprite_color() -> void:
	if child_sprite != null:
		child_sprite.frame = thisplayer_id

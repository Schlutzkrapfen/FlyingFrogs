extends CharacterBody2D

var player_id : int 

var setup := true

func _process(_d2elta: float) -> void:
	if setup:
		setupMovements()
	rotateToPlayer()

func setupMovements() -> void:
	setup = false
	$Sprite2D.frame = player_id
	$tongue.player_id = player_id
	$tongue/Area2D.player_id = player_id
func rotateToPlayer():
	var player = get_node("../player")  
	look_at(player.global_transform.origin)
	rotate(PI/2)

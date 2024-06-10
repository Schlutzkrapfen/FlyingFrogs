extends CharacterBody2D

var player_id : int  # hier wird die id gesetzt

var move_right_action := ""
var move_left_action := ""
var move_down_action := ""
var move_up_action := ""
var trigger_attack := ""

var speed := 800.0
var direction := Vector2.ZERO
var _velocity := Vector2.ZERO
var _can_attack := false  
var _original_scale := Vector2.ONE  
var _attack_timer := 3.0  
var _start_timer:= 4.0
var _setup := true
var _movementAllowed:= false

var start = true
var startMoving
var velocitys :=0  
var amountBeforTheAnimationCanPlay := 10
var waitForMoveTutorial = 0
var waitForMoveTutorialTime = 1

var global = preload("res://Skripts/Global.cs").new()

var _isBot = false
var _difficulty = 0
var _reachedFly = false
var _additionalBotCooldown = 1.5
var _additionalBotCooldownTimer = 0
var _isAttacking = false
var _can_tongue_attack = false
var origin := Vector2.ZERO
var FlyPos := Vector2.ZERO
var LastFlyPos := Vector2.ZERO
var FrogPos := Vector2.ZERO

var isFly = false;
var tongue_instance: Node

func _ready():
	tongue_instance = get_node("../Frogcar/tongue")
	#print(_isBot)
	return;

func _physics_process(delta : float)-> void:
	_can_tongue_attack = tongue_instance.can_attack()

	if !isFly :
		if _setup:
			setupMovements()
		
		if !_isBot:
			playerProcess(delta)
		else :
			botProcess(delta)

func playerProcess(delta : float) -> void:
	update_direction()
	#_velocity = Vector2.ZERO
	_velocity = direction.normalized() * speed * (1 - (_attack_timer * 2)) * delta
	
		
	if _attack_timer <= 0.5:
		if _movementAllowed:
			if velocitys < amountBeforTheAnimationCanPlay:
				velocitys += (_velocity.x*_velocity.x )+(velocity.y*velocity.y)
				waitForMoveTutorial += delta
				if waitForMoveTutorial > waitForMoveTutorialTime:
					$"../AnimationPlayer".play("MoveStick")
					$"../TextureRect".visible = true 
			else:
				$"../TextureRect".visible = false
			translate(_velocity)
			move_and_slide()
			
	#_velocity = Vector2.ZERO
	
	if Input.is_action_pressed(trigger_attack) and _can_attack && _can_tongue_attack:
		attack()
	
	if !_can_attack:
		_attack_timer -= delta
		if _attack_timer <= 0.5:
			if _attack_timer > 0.05:
				scale = _original_scale * (1.0 - _attack_timer)
			else:
				scale = _original_scale
		if _attack_timer <= 0.0:
			_can_attack = true
	if start:
		_start_timer -= delta
	if _start_timer <= -1 && start:
		$"../AnimationPlayer".play("PressA")
		
func botProcess(delta : float) -> void:	
	_additionalBotCooldownTimer += delta
	updateBotDirection(delta)
	
	if _reachedFly and _can_attack and _additionalBotCooldown<_additionalBotCooldownTimer && _can_tongue_attack:
		botattack()
		_velocity = Vector2.ZERO
		_reachedFly = false
	
	if direction.length() > 1 :
		_velocity += direction.normalized() * speed * delta
	else : 
		_velocity += direction * speed * delta
				
	if _velocity.length() > speed *delta:
		_velocity = _velocity.normalized() * speed * delta
	
	if _attack_timer <= 0.3:
		if _movementAllowed:
			translate(_velocity)
			move_and_slide()
			

	if !_can_attack:
		_attack_timer -= delta
		if _attack_timer <= 0.5:
			if _attack_timer > 0.05:
				scale = _original_scale * (1.0 - _attack_timer)
			else:
				Input.action_release(trigger_attack) #required incase last frame was attacked
				_isAttacking = false;
				scale = _original_scale
		if _attack_timer <= 0.0:
			_can_attack = true
	if start:
		_start_timer -= delta
	if _start_timer <= 0 and start:
		botattack()
	
	
func update_direction() -> void:
	direction = Vector2.ZERO
	direction.x = Input.get_action_strength(move_right_action) - Input.get_action_strength(move_left_action)
	direction.y = Input.get_action_strength(move_down_action) - Input.get_action_strength(move_up_action)
	
func updateBotDirection(delta : float) -> void:
	if _isAttacking:
		return
	#do bot stuff, add stuff for _difficulty
	var PredictedPos = Vector2.ZERO
	var currentPos = global_position

	if _difficulty == 2:
			PredictedPos = FlyPos + (FlyPos - LastFlyPos) / delta * 0.3
	elif _difficulty == 1:
			PredictedPos = FlyPos + (FlyPos - LastFlyPos) / delta * 0.8
	elif _difficulty == 0:
			PredictedPos = FlyPos + (FlyPos - LastFlyPos) / delta
			
			
	direction = (PredictedPos - currentPos)
	
	var moving = true
	
	if abs((LastFlyPos - FlyPos).length()) < 0.01 :
		moving = false;
		
	if abs(direction.length()) < 30 && moving || abs(direction.length()) < 15:
		direction = Vector2.ZERO
		_velocity = velocity - velocity/2
		_reachedFly = true
	
	LastFlyPos = FlyPos
	
func setupMovements() -> void:
	var player = get_node(".")
	var playerNode = player as Node2D
	FrogPos = playerNode.position
	
	_setup = false
	move_right_action = "FrogMovementRight" + str(player_id)
	move_left_action = "FrogMovementLeft" + str(player_id)
	move_down_action = "FrogMovementDown" + str(player_id)
	move_up_action = "FrogMovementUp" + str(player_id)
	trigger_attack = "FrogJoin" + str(player_id)
	$Sprite2D.frame = player_id
	setSpawnpoint()
	
func botattack() -> void:
	_isAttacking = true
	_velocity = Vector2.ZERO
	Input.action_press(trigger_attack)
	attack()
	_additionalBotCooldownTimer = 0;
	_additionalBotCooldown = 1 / (_difficulty + 1)
	#_attack_timer += _additionalBotCooldown
	
func attack() -> void:
	scale *= 0.5
	_velocity = Vector2.ZERO
	_can_attack = false
	_movementAllowed = true
	_attack_timer = 1.0
	start = false
	$"../AnimationPlayer".stop()
	$"../TextureRect".visible = false
	
	
func setSpawnpoint() -> void:
	match player_id:
		0:
			global_position = Vector2(900, 250)
		1:
			global_position = Vector2(250, 250)
		2:
			global_position = Vector2(250, 450)
		3:
			global_position = Vector2(900, 450)
	
	if origin == Vector2.ZERO:
		origin = global_position



func isInBoundingBoxNew(value : Vector2) -> bool:
	if Global.GetGlobalPlayer(0) && global_position.x > 592 && global_position.y < 350:
		# this is rechts oben (0)
		# kleiner als 350 y höhe und größer als 592 x breite und spieler existiert
		#print("top right")
		return false
		
	if Global.GetGlobalPlayer(1) && global_position.x < 592 && global_position.y < 350: 
		# this is links oben (1)
		# kleiner als 350 y höhe und kleiner als 592 x breite und spieler existiert
		#print("top left")
		return false
		
	if Global.GetGlobalPlayer(2) && global_position.x < 592 && global_position.y > 350: 
		# this is links unten (2)
		# größer als 350 y höhe und kleiner als 592 x breite und spieler existiert
		#print("bottom left")
		return false
		
	if Global.GetGlobalPlayer(3) && global_position.x > 592 && global_position.y > 350: 
		# this is rechts unten (3)
		# größer als 350 y höhe und größer als 592 x breite und spieler existiert
		#print("bottom right")
		return false
		
	return false
	
func SetFlyPosition(value: Vector2) -> void:
	FlyPos = value
	
func SetIsBot(value : bool) -> void:
	_isBot = !value

func SetDifficulty(value : float) -> void:
	_difficulty = value

func SetIsFly(value : bool) -> void:
	if value :
		Input.action_release(trigger_attack) 
	if isFly && !value:
		tongue_instance.reset_after_fly()
		global_position = origin
	
	tongue_instance.SetIsFly(value)
	isFly = value


# als erstes box checks für die spielfeldgrenzen
# dann box checks für die spezialfelder

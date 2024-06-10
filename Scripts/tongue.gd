extends CharacterBody2D

var player_id : int 
signal player_idChanged(Id)
var trigger_attack := ""

var setup := true
var _can_attack := true   
var _attack_timer := 0.0  

var moveToPlayer : bool = false
var moveFromPlayer : bool = false

var player : CharacterBody2D
var originalPosition : Vector2 

var _start_timer:= 4.0
var _after_cap_cooldown := 1.0
var _after_cap_timer := 0.0

var start = true
var isFly := false

var soundPlayer : AudioStreamPlayer2D
func _ready():
	soundPlayer = $toungeSound
	originalPosition = global_position;
	
func _process(delta: float) -> void:
		
	if start :
		_start_timer -= delta;
		if _start_timer <= 1 :
			start = false
	
	if setup:
		setupMovements()
	if Input.is_action_pressed(trigger_attack) and _can_attack and !start and !isFly:
		shootToPlayer()
		play_random_pitch_sound()
		
	if !_can_attack:
		_attack_timer -= delta
		if _attack_timer <= 0.0:
			_can_attack = true
			global_position = originalPosition
			
	if _after_cap_timer < _after_cap_cooldown:
		_can_attack = false
		_after_cap_timer += delta
		
	if moveFromPlayer || moveToPlayer:
		if moveToPlayer:
			var distanceToPlayer = lerp(global_position,player.global_position, 0.4 * (delta * 60))
			if abs(player.global_position.x - global_position.x) < 5 && abs(player.global_position.y - global_position.y) < 5:
				moveToPlayer = false
				moveFromPlayer = true
			else: 
				global_position = distanceToPlayer
				
		if moveFromPlayer:
			var distanceToHome = lerp(global_position, originalPosition, 0.25* (delta * 60))
			if abs(originalPosition.x - global_position.x) < 5 && abs(originalPosition.y - global_position.y) < 5:
				moveToPlayer = false
				moveFromPlayer = false
				global_position = originalPosition
			else: 
				global_position = distanceToHome


func setupMovements() -> void:
	setup = false
	trigger_attack = "FrogJoin" + str(player_id)


func shootToPlayer():
	_can_attack = false
	_attack_timer = 1.0
	moveToPlayer = true
	player = get_node("../../player") 
	#originalPosition = global_position
	
func getPlayerId() -> int:
	return player_id
	
func play_random_pitch_sound():
	if soundPlayer != null and soundPlayer.stream != null:
		# Generate a random pitch value between 0.5 and 2.0
		var random_pitch = randf_range(0.5, 2.0)

		# Set the pitch scale of the sound player
		soundPlayer.pitch_scale = random_pitch

		# Play the sound
		soundPlayer.play()
	#else:
		#print("Sound player or sound stream is missing")
		
func can_attack() -> bool:
	return _can_attack

func reset_after_fly() -> void:
	global_position = originalPosition;
	_after_cap_timer = 0;

func SetIsFly(value : bool) -> void:
	isFly = value;



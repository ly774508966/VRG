using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GridTile
{
	public Vector3 location;
	public List<GameObject> presentGameObjects;

	public GridTile (Vector3 locationArg)
	{
		location = locationArg;
	}
}

public class ScriptGameMaster : MonoBehaviour
{
		
	//Phases - Actions resolve in execution phase; players issue orders in command phase
	enum Phase
	{
		Execution,
		Command
	}
	
	//Modes - Play alternates between movement and engagement modes while in execution phase
	enum Mode
	{
		Movement,
		Command,
		Engagement
	}

	//Game states
	Phase gamePhase;
	Mode gameMode;
	
	//Interface
	public float damageDisplayDepth = -1;
	public string inputButtonName = "";
	public GameObject damageDisplay;
	ScriptInterface scriptInterface;
	ScriptCycleDisplay scriptCycleDisplay;

	//Characters
	public CharacterSheet selectedSheet;
	public CharacterSheet opposingSheet;
	public GameObject characterGameObjectTemplate;
	public GameObject containerCharacter;
	public List<CharacterSheet> charactersInPlay = new List<CharacterSheet> ();
	//List<ScriptCharacterSheet> activeCharacters = new List<ScriptCharacterSheet>();
	int nextCharacterID = 0;
	
	//Items
	public List<Item> itemsInPlay = new List<Item> ();
	
	//Space
	public Transform spawn00;
	public Transform spawn01;
	public GridTile[ , , ] gameGrid;
	//public int gridSizeX = 10;
	//public int gridSizeY = 20;
	//public int gridSizeZ = 1;
	public Vector3 gridSpacing = new Vector3 (1, 1, 1);
	public GameObject gridTilePrefab;
	public GameObject gridContainer;
	public GameObject containerPrefab;

	
	//Time
	public int cycle = -1;
	public float cycleTimer;
	public float cycleLength = 10;
	public float timerConstant = 1;
	public int spawnDelayCycles = 3;
	int spawn00Time = -1;
	int spawn01Time = -1;

	//Cameras
	Camera overviewCamera;
		
	//Debug
	//public GameObject testCharacter;
	//public GameObject[] testArray;
	//public List<GameObject> tempCharactersInPlay = new List<GameObject>();

	private static ScriptGameMaster _instance;

	public static ScriptGameMaster Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<ScriptGameMaster> ();
			}
			return _instance;
		}
	}
	
	void Awake ()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start ()
	{	
		//Create grid
		gameGrid = NewGameGrid (100, 100, 1);
		//Debug.Log(gameGrid[5, 11, 0].location);

		//Debug.Log(gameGrid[0, 0, 0].location);



		//Acquire scripts
		GameObject interfaceController = GameObject.Find ("ControllerInterface");
		scriptInterface = interfaceController.GetComponent<ScriptInterface> ();
		scriptCycleDisplay = interfaceController.transform.FindChild ("PanelCycle").GetComponent<ScriptCycleDisplay> ();
		
		//Acquire camera
		overviewCamera = Camera.main;
		
		//Acquire controllers
		//containerCharacter = GameObject.Find ("ControllerCharacter");
		
		//Register each character object in the scene
		foreach (GameObject character in GameObject.FindGameObjectsWithTag("Character")) {
			CharacterSheet hotSheet = character.GetComponent<CharacterSheet> ();
			print ("hotSheet = " + hotSheet.ToString());
			//Set first character as active
			if (selectedSheet == null) {				
				SetAsSelected (RegisterCharacter (hotSheet));
			} else {
				RegisterCharacter (hotSheet);	
			}
		}
		
		//Spawn a random character on the left and right spawnpoints and give a random item
		GiveCharacterItem (SetAsSelected (RegisterCharacter (RandomizeCharacterValues (NewCharacter (spawn00)))), CreateRandomItem ());
		GiveCharacterItem (RegisterCharacter (RandomizeCharacterValues (NewCharacter (spawn01))), CreateRandomItem ());

		//Begin first turn
		RolloverCycle ();


	}
	
	// Update is called once per frame
	void Update ()
	{

		//Debug
		//if(Input.GetKeyDown(KeyCode.N)){
		//	RandomizeCharacterValues(RegisterCharacter(NewCharacter()));
		//}
		//if(Input.GetKeyDown(KeyCode.C)){
		//foreach(GameObject character in charactersInPlay)
		//	{
		//	character.transform.FindChild("ObjectCharacterModel").SendMessage("ColorCharacter");	
		//	}
		//}
		//if(Input.GetKeyDown(KeyCode.P)){
		//	CreatePlayerCharacter();	
		//}

		if (inputButtonName != "") {
			ButtonHandler ();
		}

		//In movement mode, run turn for cycle length, then allow characters to act in engagement mode
		if (gameMode == Mode.Movement) {
			cycleTimer += Time.deltaTime * timerConstant;
			if (cycleTimer >= cycleLength) {
				//Uncomment next line to restore normal resolution
				SetToEngagementMode ();
			}
		}
	}

//BEGIN CUSTOM METHODS
	
//CHARACTER CREATION

	//Spawn position transform -> unregistered character sheet
	CharacterSheet NewCharacter (Transform spawnTransform)
	{
		//Create character at spawn point and initialize variables
		GameObject hotChar = Instantiate (characterGameObjectTemplate, spawnTransform.position, spawnTransform.rotation) as GameObject;
		CharacterSheet hotSheet = hotChar.GetComponent<CharacterSheet> ();
			
		//Place character in character container
		hotChar.transform.parent = containerCharacter.transform;		
			
		//Assign left character as selected and right as opposing; assign position objective
		if (spawnTransform == spawn01) {
			hotSheet.positionObjective = new Vector3 (-3.75F, hotChar.transform.position.y, hotChar.transform.position.z);
			opposingSheet = hotSheet;
		} else if (spawnTransform == spawn00) {		
			hotSheet.positionObjective = new Vector3 (-5.25F, hotChar.transform.position.y, hotChar.transform.position.z);
			selectedSheet = hotSheet;	
		} else {
			Debug.Log ("Invalid spawn position");	
		}
		return hotSheet;
	}
		
	//Unregistered character sheet -> registered character sheet
	CharacterSheet RegisterCharacter (CharacterSheet hotSheet)
	{

		//print ("register character: " + hotSheet.fullName);

		//Set character as kinematic
		PhysicsController.Instance.Unragdollify (hotSheet.gameObject);

		//Assign and increment character id
		hotSheet.characterID = nextCharacterID;
		nextCharacterID += 1;

		//Add to list of active characters
		charactersInPlay.Add (hotSheet);

		//Set display name and assign to gameobject
		hotSheet.stringID = string.Format ("{0:00} {1}", hotSheet.characterID, hotSheet.fullName);
		hotSheet.gameObject.name = hotSheet.stringID;
		
		//Set color
		hotSheet.transform.FindChild ("ObjectCharacterModel").GetComponent<ScriptModelController>().ColorCharacter();
//		hotSheet.transform.FindChild ("ObjectCharacterModel").SendMessage ("ColorCharacter");
		
		//Set derived stats
		hotSheet.maxHitProfile = new CharacterHitProfile (2 + hotSheet.toughness, 5 + hotSheet.toughness, 
		                                                 3 + hotSheet.toughness, 3 + hotSheet.toughness,
		                                                 4 + hotSheet.toughness, 4 + hotSheet.toughness);
		hotSheet.currentHitProfile = new CharacterHitProfile (hotSheet.maxHitProfile);
		hotSheet.unarmedDamage = hotSheet.baseMuscle / 2;
		
		//Set default item and tactics
		if (hotSheet.activeItem == null) {
			hotSheet.activeItem = ScriptDatabase.Instance.unarmed;
		}
		hotSheet.activeTactics.Add (ScriptDatabase.Instance.tacticsLookup ["Basic Shot"]);

		return hotSheet;
	}
		
	//character sheet -> selected character sheet
	CharacterSheet SetAsSelected (CharacterSheet hotSheet)
	{
		selectedSheet = hotSheet;
		return hotSheet;	
	}

	//character sheet -> randomized character sheet
	CharacterSheet RandomizeCharacterValues (CharacterSheet hotSheet)
	{
		
		//Assign random name
		hotSheet.firstName = ScriptDatabase.Instance.firstNames [(int)Mathf.Floor (Random.value * ScriptDatabase.Instance.firstNames.Count)];
		hotSheet.lastName = ScriptDatabase.Instance.lastNames [(int)Mathf.Floor (Random.value * ScriptDatabase.Instance.lastNames.Count)];
		hotSheet.fullName = hotSheet.firstName + " " + hotSheet.lastName;

		//Assign random colors
		hotSheet.primaryColor = GetRandomColor ();
		hotSheet.secondaryColor = GetRandomColor ();
		
		//Assign Stats
		hotSheet.toughness = GetRandom1ToN (5);
		hotSheet.currentFocus = GetRandom1ToN (10);
		hotSheet.baseMuscle = GetRandom1ToN (10);
		hotSheet.baseBrawl = GetRandom1ToN (10);
		hotSheet.baseMelee = GetRandom1ToN (10);
		hotSheet.baseShot = GetRandom1ToN (10);
		hotSheet.baseEvasion = GetRandom1ToN (10);
		hotSheet.baseIntelligence = GetRandom1ToN (10);
		hotSheet.basePresence = GetRandom1ToN (10);
	
		return hotSheet;	
	}

	//unregistered character sheet, character sheet template -> unregistered character sheet of template
	CharacterSheet SetCharacterValues (CharacterSheet hotSheet, CharacterTemplate hotTemplate)
	{
		//Set name and update game object
		hotSheet.fullName = hotTemplate.fullName;
		hotSheet.stringID = string.Format ("{0:00} {1}", hotSheet.characterID, hotSheet.fullName);
		hotSheet.gameObject.name = hotSheet.stringID;
		
		//Assign Colors
		hotSheet.primaryColor = hotTemplate.primaryColor;
		hotSheet.secondaryColor = hotTemplate.secondaryColor;
		hotSheet.skinColor = hotTemplate.skinColor;
		
		//Assign Stats
		hotSheet.toughness = hotTemplate.characterStatProfile.toughness;
		hotSheet.currentFocus = hotTemplate.characterStatProfile.focus;
		hotSheet.baseMuscle = hotTemplate.characterStatProfile.muscle;
		hotSheet.baseBrawl = hotTemplate.characterStatProfile.brawl;
		hotSheet.baseMelee = hotTemplate.characterStatProfile.muscle;
		hotSheet.baseShot = hotTemplate.characterStatProfile.shot;
		hotSheet.baseEvasion = hotTemplate.characterStatProfile.evasion;
		hotSheet.baseIntelligence = hotTemplate.characterStatProfile.intelligence;
		hotSheet.basePresence = hotTemplate.characterStatProfile.presence;

		return hotSheet;
	}

//TURN PROGRESSION

	//Increment cycle
	void RolloverCycle ()
	{
		
		cycle += 1;
		
		//Begin Command Phase
		SetToCommandPhase ();
		
		//Reduce all characters' wait time by 1
		foreach (CharacterSheet hotSheet in charactersInPlay) {
			if (hotSheet.waitTime > 0) {
				hotSheet.waitTime -= 1;
			}
		}
		
		//Cycle timer begins at 0
		cycleTimer = 0.0F;
		//Log new Cycle
		scriptCycleDisplay.SendMessage ("UpdateCycle", cycle);
		
		//Ensure all characters have valid targets
		UpdateCharacterValues ();
	}

	//Progress to next event
	void ResolveEngagements ()
	{
		
		if (gameMode == Mode.Engagement) {
			//Update character states
			UpdateCharacterValues ();

			//Get characters in queue to act
			List<CharacterSheet> activeCharacters = GetActiveCharacters ();

			//If there any characters left to act for this Cycle, then execute next action
			/* UNCOMMENT TO RESTORE NORMAL FUNCTIONALITY
			if(activeCharacters.Count > 0)
			{
				//1. Determine character order
				List<ScriptCharacterSheet> sortedCharacters = SortActiveCharacters(activeCharacters);
				
				//2. Exceute next action in queue
				ExecuteAction(sortedCharacters[0]);
				
				//3. If more than one character and characters are tied for priority, both actions resolve before registering new states
				//Warning: It seems like this will not work for more than one tied charcter
				if(activeCharacters.Count > 1 && activeCharacters[0].readyPriority == activeCharacters[1].readyPriority)
				{
				ExecuteAction(activeCharacters[1]);	
				}
				
				//4. Update character states and run recursively
				UpdateCharacterValues();
				ResolveEngagements ();
			} 
			else 
			{
*/
			//If there are no characters left to act, end cycle
			RolloverCycle ();
			//	}
		} else {
			Debug.Log ("Error: Attempt to resolve engagement outside of engagement mode");
		}
	}

//MODE TOGGLE
	
	void SetToExecutionPhase ()
	{
		
		gamePhase = Phase.Execution;
		
		//Spawn characters if necessary
		if (spawn00Time == cycle) {
			GiveCharacterItem (RegisterCharacter (RandomizeCharacterValues (NewCharacter (spawn00))), CreateRandomItem ());
		} 
		
		if (spawn01Time == cycle) {
			GiveCharacterItem (RegisterCharacter (RandomizeCharacterValues (NewCharacter (spawn01))), CreateRandomItem ());
		}
		
		//Begin in movement mode
		SetToMovementMode ();
	}
	
	void SetToCommandPhase ()
	{
		
		gamePhase = Phase.Command;
		gameMode = Mode.Command;
		
		//Skip Command Phase
		SetToExecutionPhase ();
	}
	
	void SetToMovementMode ()
	{
		gameMode = Mode.Movement;
		
		//Start movement towards destination
		foreach (CharacterSheet hotSheet in charactersInPlay) {
			ScriptCharacterController hotScript = hotSheet.GetComponent<ScriptCharacterController> ();
			hotScript.greenLight = true;
			hotScript.startLerp = true;
		}
	}
	
	void SetToEngagementMode ()
	{
		gameMode = Mode.Engagement;
		
		//Stop character movement
		StartCoroutine ("RedLight");
	}

//ACTION RESOLUTION

	//Resolve character's action- default overload
//	void ExecuteAction (ScriptCharacterSheet hotSheet)
//	{
//		//Deprecated?
//
//		//New result
//		Result result = null;
//	
//		if (hotSheet.target) {
//			ScriptCharacterSheet targetSheet = hotSheet.target;
//			
//			//Get result of attempted action
//			result = GetActionResult (hotSheet, targetSheet);
//			
//			//Change states
//			if (result.success) {
//				//Reduce health
//				SumHitProfiles (result.targetCharacter.currentHitProfile, result.targetNetHitProfile);
//			}
//			
//			//Apply damage results
//			if (targetSheet.currentHitProfile.head <= 0 || targetSheet.currentHitProfile.body <= 0) {
//				KillCharacter (targetSheet);	
//				
//				//Start encounter cam on dead character
//				RunCinematicCamera (result.targetCharacter.GetComponent<ScriptCharacterController> ());
//			}
//			
//			//Initiate visual and audio effects
//			scriptPhysicsController.SendMessage ("InitiateActionEffect", result);
//			
//		} else {
//			Debug.Log (result.actingCharacter.ToString () + " attacks nothing.");
//		}
//		
//		//Reset Wait Time to delay
//		hotSheet.waitTime = hotSheet.activeItem.itemStatProfile.cooldownAspect; 
//		
//		//Log action to console
//		string hotLine = string.Format ("{0} attacks {1} ({2:00} ATT vs {3:00} DEF: {4:00}%). Roll: {5}", 
//		                                new object[] {
//			result.actingCharacter.fullName,
//			result.targetCharacter.fullName,
//			result.actingAttack,
//			result.targetDefense,
//			result.hitPercentage,
//			result.roll
//		});
//		if (result.success) {
//			hotLine += string.Format (" > {0}. {1} shoots {2} for {3} {4} damage.", 
//			                          new object[]{result.successNumber, result.actingCharacter.fullName, 
//				result.targetCharacter.fullName, result.grossDamage, result.damageType});
//		} else {
//			hotLine += string.Format (" <= {0}. {1} misses.", result.successNumber, result.actingCharacter.fullName);
//		}
//		ConsoleAddLine (hotLine);
//		
//		//Display damage
//		GameObject currentDamageDisplay = Instantiate (
//			damageDisplay, new Vector3 (result.targetCharacter.gameObject.transform.position.x,
//		                           result.targetCharacter.gameObject.transform.position.y, damageDisplayDepth), Quaternion.identity) as GameObject;
//		currentDamageDisplay.GetComponent<ScriptDisplayContainer> ().camera00 = overviewCamera;
//		TextMesh statusChangeText = currentDamageDisplay.GetComponentInChildren<TextMesh> ();
//		if (result.success) {
//			statusChangeText.text = "-" + result.grossDamage + "HP";
//		} else {
//			statusChangeText.text = "Miss";
//		}
//	}

	//Resolve player input shot- 2nd overload
	public void ExecuteAction (PlayerShotInfo playerShotInfo)
	{

		//Debug.Log("Execute action ray " + playerShotInfo.shotRay.ToString());

		//New result
		Result result = null;
		CharacterSheet hotSheet = playerShotInfo.shooter;
		CharacterSheet targetSheet = playerShotInfo.target;

		//Add playershotinfo to result and resolve action
		result = GetActionResult (playerShotInfo);
//		print ("player shot info target: " + playerShotInfo.target.fullName);
		if (playerShotInfo.target) {
			print ("Found target from PSI");
			//Change states
			if (result.success) {
				//Reduce health
				print ("result target hit profile: " + result.targetNetHitProfile.head + ", " + result.targetNetHitProfile.body + ", " + result.targetNetHitProfile.leftArm);
				SumHitProfiles (result.targetCharacter.currentHitProfile, result.targetNetHitProfile);

				//Apply damage results
				print ("Applying damage results: " + targetSheet.currentHitProfile.head + ", " + targetSheet.currentHitProfile.body);
				if (targetSheet.currentHitProfile.head <= 0 || targetSheet.currentHitProfile.body <= 0) {
					//Debug.Log (targetSheet.fullName + "Should be dead");
					KillCharacter (targetSheet);	
				
					//Start encounter cam on dead character
					RunCinematicCamera (result.targetCharacter.GetComponent<ScriptCharacterController> ());
				}
			}

			//Set selector cooldown?
			//hotSheet.waitTime = hotSheet.activeItem.itemStatProfile.cooldownAspect; 
		
			//Log action to console
			string hotLine = string.Format ("{0} attacks {1} ({2:00} ATT vs {3:00} DEF: {4:00}%). Roll: {5}", 
		                                new object[] {
				result.actingCharacter.fullName,
				result.targetCharacter.fullName,
				result.actingAttack,
				result.targetDefense,
				result.hitPercentage,
				result.roll
			});
			if (result.success) {
				hotLine += string.Format (" > {0}. {1} shoots {2} for {3} {4} damage.", 
			                          new object[]{result.successNumber, result.actingCharacter.fullName, 
				result.targetCharacter.fullName, result.grossDamage, result.damageType});
			} else {
				hotLine += string.Format (" <= {0}. {1} misses.", result.successNumber, result.actingCharacter.fullName);
			}
			ConsoleAddLine (hotLine);
		
			//Display damage
			GameObject currentDamageDisplay = Instantiate (
			damageDisplay, new Vector3 (result.targetCharacter.gameObject.transform.position.x,
		                           result.targetCharacter.gameObject.transform.position.y, damageDisplayDepth), Quaternion.identity) as GameObject;
			currentDamageDisplay.GetComponent<ScriptDisplayContainer> ().camera00 = overviewCamera;
			TextMesh statusChangeText = currentDamageDisplay.GetComponentInChildren<TextMesh> ();
			if (result.success) {
				statusChangeText.text = "-" + result.grossDamage + "HP";
			} else {
				statusChangeText.text = "Miss";
			}
		}
		//Initiate visual and audio effects
		PhysicsController.Instance.InitiateActionEffect(result);
	}

	//attacking character, defending character -> attack result. Default overload
	Result GetActionResult (CharacterSheet actingCharacter, CharacterSheet targetCharacter)
	{


		
		Result result = new Result (actingCharacter);
		result.targetCharacter = targetCharacter;
		
		//Get attack stats
		result.actingAttack = actingCharacter.readyAttack;
		result.targetDefense = targetCharacter.readyDefense;
		
		//Calculate success number
		result.hitPercentage = GetHitPercentage (result.actingAttack, result.targetDefense);
		result.successNumber = 100 - result.hitPercentage;
		
		//Roll d100	
		result.roll = GetRandom1ToN (100);
		
		//If roll is greater than the success number, attack succeeds
		result.rollExcess = result.roll - result.successNumber;
		print ("Roll excess: " + result.rollExcess);
		if (result.rollExcess >= 1) {
			result.success = true;
			
			//Set damage properties
			result.grossDamage = actingCharacter.readyDamage;
			result.damageType = actingCharacter.activeItem.damageType;
			result.targetGrossHitProfile = GetGrossHitProfile (result);
			
			//Apply armor (pending)
			result.targetNetHitProfile = SumHitProfiles (result.targetGrossHitProfile, result.targetCharacter.resistanceHitProfile);
		} else {
			result.success = false;
		}
		return result;
	}

	Result GetActionResult (PlayerShotInfo playerShotInfo)
	{
		CharacterSheet actingCharacter = playerShotInfo.shooter;
		Result result = new Result (actingCharacter);
		result.playerShotInfo = playerShotInfo;

		//Debug.Log ("Action result init: " + playerShotInfo.shotRay.ToString ());
//		print ("player shot info target" + playerShotInfo.target.name);
		if (playerShotInfo.target) {

			result.targetCharacter = playerShotInfo.target;
			result.hitLocation = playerShotInfo.shotLocation;
			//Debug.Log (hitLocation.name);
		
			//Get attack stats
			result.actingAttack = actingCharacter.readyAttack;
			result.targetDefense = result.targetCharacter.readyDefense;
		
			//Calculate success number
			result.hitPercentage = GetHitPercentage (result.actingAttack, result.targetDefense);
			result.successNumber = 100 - result.hitPercentage;
		
			//Roll d100	
			result.roll = GetRandom1ToN (100);
		
			//If roll is greater than the success number, attack succeeds
			result.rollExcess = result.roll - result.successNumber;
//			print ("acting attack, target defense" + );
			if (result.rollExcess >= 1) {
				result.success = true;
			
				//Set damage properties
				result.grossDamage = actingCharacter.readyDamage;
				result.damageType = actingCharacter.activeItem.damageType;
				result.targetGrossHitProfile = GetGrossHitProfile (result);
			
				//Apply armor (pending)
				result.targetNetHitProfile = SumHitProfiles (result.targetGrossHitProfile, result.targetCharacter.resistanceHitProfile);
			} else {
				result.success = false;
			}
		} else {
			result.success = false;
		}
		return result;
	}

//CHARACTER MANAGEMENT
	
	//Remove character from play
	void KillCharacter (CharacterSheet hotSheet)
	{
		if (!hotSheet.inPlay) return; //Stop, he's already dead!...

		ScriptCharacterController scriptCharacterController = hotSheet.GetComponent<ScriptCharacterController> ();

		//Remove character from characters in play 
		int hotIndex = GetCharactersInPlayIndex (hotSheet);
		//Debug.Log (hotIndex.ToString ());
		charactersInPlay.RemoveAt (hotIndex);
		
		//Set character's inPlay to false
		hotSheet.inPlay = false;

		//Remove character as a valid target
		foreach (CharacterSheet otherHotSheet in charactersInPlay) {
			if (otherHotSheet.target == hotSheet) {
				otherHotSheet.target = null;
			}
		}

		//Stop character
		scriptCharacterController.greenLight = false;

		//Disable oscillator
		ScriptPlayerTargeting scriptPlayerTargeting = hotSheet.GetComponentInChildren<ScriptPlayerTargeting> ();
		if (scriptPlayerTargeting != null) {
			scriptPlayerTargeting.DisableOscillation (scriptCharacterController);
		}

		
		//Set new character to spawn
		if (hotSheet.gameObject.transform.rotation.y == 0) {	
			spawn00Time = cycle + spawnDelayCycles;
		} else {
			spawn01Time = cycle + spawnDelayCycles;
		}
	}

	void UpdateCharacterValues ()
	{	
		//Update targets
		foreach (CharacterSheet hotSheet in charactersInPlay) {	
			//For every character in play, if there is one other character
			if (hotSheet.target == null && charactersInPlay.Count > 1) {
				//Choose random character
				int randomCharacterIndex = (int)Mathf.Floor (Random.value * charactersInPlay.Count);
				CharacterSheet otherCharacter = charactersInPlay [randomCharacterIndex];
				
				//If random character is not original character, assign as target (no character can be a target of him/herself)
				if (otherCharacter != hotSheet) {
					hotSheet.target = otherCharacter;
					//assigningNewTarget = false;
				} else {
					//If it is original character, use next character in line
					hotSheet.target = charactersInPlay [((randomCharacterIndex + 1) % (charactersInPlay.Count))];
				}
			}

			//Update contextual position
			hotSheet.isInActingPosition = hotSheet.GetComponentInChildren<ScriptControllerTargeting> ().GetActingPosition (hotSheet);

			//Update Equipment modifiers
			hotSheet.netEquipmentAttack = hotSheet.activeItem.itemStatProfile.attackModifier;
			hotSheet.netEquipmentDamage = hotSheet.activeItem.itemStatProfile.damageModifier;
			hotSheet.netEquipmentPriority = hotSheet.activeItem.itemStatProfile.priorityModifier;
			hotSheet.netEquipmentRange = hotSheet.activeItem.itemStatProfile.maxRangeAspect;
		
			//Update Tactic modifiers--only works for first tactic at the moment
			hotSheet.netTacticsAttack = hotSheet.activeTactics [0].tacticStatProfile.attack;
			hotSheet.netTacticsDamage = hotSheet.activeTactics [0].tacticStatProfile.damage;
			hotSheet.netTacticsDefense = hotSheet.activeTactics [0].tacticStatProfile.defense;
			hotSheet.netTacticsPriority = hotSheet.activeTactics [0].tacticStatProfile.priority;
			hotSheet.netTacticsRange = hotSheet.activeTactics [0].tacticStatProfile.maxRange;
			
			//Update ready stats

			//Determine attack stat
			int baseAttack;
			switch (hotSheet.activeItem.attackType) {
			case AttackType.Brawl:
				baseAttack = hotSheet.baseBrawl;
				break;
			case AttackType.Melee:
				baseAttack = hotSheet.baseMelee;
				break;
			case AttackType.Shot:
				baseAttack = hotSheet.baseShot;
				break;
			default:
				Debug.Log ("Invalid Attack Type " + hotSheet.activeItem.attackType.ToString ());
				baseAttack = -9999;
				break;
			}

			//print ("ready attack = " + baseAttack + ", " + hotSheet.netEquipmentAttack + ", " + hotSheet.netTacticsAttack);
			hotSheet.readyAttack = baseAttack + hotSheet.netEquipmentAttack + hotSheet.netTacticsAttack;		
			hotSheet.readyDefense = hotSheet.baseEvasion + hotSheet.netTacticsDefense;
			hotSheet.readyPriority = hotSheet.currentFocus + hotSheet.netEquipmentPriority + hotSheet.netTacticsPriority;
			hotSheet.readyRange = hotSheet.netEquipmentRange + hotSheet.netTacticsRange;
			
			//Add muscle only if close combat
			if (hotSheet.activeItem.attackType == AttackType.Shot) {
				hotSheet.readyDamage = hotSheet.netEquipmentDamage + hotSheet.netTacticsDamage;
			} else if (hotSheet.activeItem.attackType == AttackType.Brawl || hotSheet.activeItem.attackType == AttackType.Melee) {
				hotSheet.readyDamage = hotSheet.baseMuscle + hotSheet.netEquipmentDamage + hotSheet.netTacticsDamage;
			} else {
				Debug.Log ("Invalid Attack Type: " + hotSheet.activeItem.attackType);
			}

			//Update currentHitChance (pending)
		}						
	}

//ITEMS

	//Add item to character's inventory
	public void GiveCharacterItem (CharacterSheet character, Item hotItem)
	{
		//print ("Give character item: " + character.name + ", "+ hotItem.fullName);
		//If item is unowned
		if (hotItem.owner == null) {
			//Add to equipped items and set character as new owner
			character.equippedItems.Add (hotItem);
			character.activeItem = hotItem;
			hotItem.owner = character;
			print ("active item: " + hotItem.fullName);
		} else {
			Debug.Log ("Item is already owned");
		}
	}

	//Returns random item
	public Item CreateRandomItem ()
	{
		Item hotItem = ScriptDatabase.Instance.GetRandomItem ();
		itemsInPlay.Add (hotItem);
		return hotItem;
	}

//CAMERA

	//Switch camera to cinematic
	void RunCinematicCamera (ScriptCharacterController character)
	{
		Camera hotCam = character.characterCameras [2]; //magic number
		overviewCamera.enabled = false;
		hotCam.enabled = true;
		
		StartCoroutine ("StopCinematicCamera", hotCam);
	}
	
	//Revert to normal view
	IEnumerator StopCinematicCamera (Camera hotCam)
	{
		yield return new WaitForSeconds (2.0F);
		
		hotCam.enabled = false;
		overviewCamera.enabled = true;
	}

//HANDLERS

	void ButtonHandler ()
	{
		if (charactersInPlay.Count >= 1) {

			string hotButton = inputButtonName;
			//playerPrompt = false;
			inputButtonName = null;
			switch (hotButton) {	
			case "Melee":
					//selectedSheet.engageInMelee = true;
					//selectedSheet.engageAtRange = false;
				break;
			case "Ranged":
					//selectedSheet.engageInMelee = false;
					//selectedSheet.engageAtRange = true;
				break;
			case "Next":
					//NextStep();
				SetToExecutionPhase ();
				break;
			case null:
				break;
			default:
				Debug.Log ("Invalid button name: " + hotButton);
				break;
			}	
		}
	}
		
//HELPERS

	//Return queue of characters ready to act
	List<CharacterSheet> GetActiveCharacters ()
	{	
		List<CharacterSheet> activeCharacters = new List<CharacterSheet> ();
		foreach (CharacterSheet currentSheet in charactersInPlay) {
			if (currentSheet.waitTime == 0 && currentSheet.isInActingPosition) {
				activeCharacters.Add (currentSheet);
			}
		}
		return activeCharacters;
	}
	
	//Unsorted queue -> sorted queue
	List<CharacterSheet> SortActiveCharacters (List<CharacterSheet> activeCharacters)
	{
		int initialCount = activeCharacters.Count;
		List<CharacterSheet> tempList = new List<CharacterSheet> ();
		while (tempList.Count < initialCount) {	
			//Determine highest priority of remaining active characters
			float maxPriority = 0.0F;
			for (int i = 0; i < activeCharacters.Count; i++) {
				float currentPriority = activeCharacters [i].readyPriority;
				if (currentPriority > maxPriority)
					maxPriority = currentPriority;
			}	
			
			//Add highest priority character to temporary list and remove from active characters
			bool findingNextCharacter = true;
			int j = 0;
			while (findingNextCharacter) {	
				if (activeCharacters [j].readyPriority == maxPriority) {
					tempList.Add (activeCharacters [j]);
					findingNextCharacter = false;
					activeCharacters.RemoveAt (j);
				} else {
					j++;
				}
			}
		}
		return tempList;
	}
	
	//character sheet -> index in characters in play list
	int GetCharactersInPlayIndex (CharacterSheet hotSheet)
	{
		for (int i = 0; i < charactersInPlay.Count; i++) {
			if (hotSheet == charactersInPlay [i]) {
				return i;	
			}
		}
		Debug.LogError ("Character Index not found.");
		return -1;
	}

	void ConsoleAddLine (string line)
	{
		scriptInterface.SendMessage ("AddNewLine", line);	
	}
	
	bool GetRandomBool ()
	{
		if (Random.value >= .5) {
			return true;	
		} else {
			return false;
		}
			
	}

	//n -> random integer between 1 and n
	int GetRandom1ToN (int n)
	{
		return (int)Mathf.Floor (Random.value * n + 1);
	}

	//Return random color
	Color GetRandomColor ()
	{
		//Color test = new Color(
		return new Color (
			Random.value,
			Random.value,
			Random.value,
			255);
	}
	
	//Wait for every character to finish their frame of movement, then stop all characters and resolve actions
	IEnumerator RedLight ()
	{
		yield return 0;
		foreach (CharacterSheet hotSheet in charactersInPlay) {
			ScriptCharacterController hotScript = hotSheet.GetComponent<ScriptCharacterController> ();
			hotScript.greenLight = false;
		}
		if (gameMode == Mode.Engagement) {
			ResolveEngagements ();
		}
	}

	//attack stat, defense stat -> number to beat to hit target
	int GetHitPercentage (int actingAttack, int targetDefense)
	{
		return (10 + actingAttack - targetDefense) * 5;	
	}

	//action result -> damage profile before armor
	CharacterHitProfile GetGrossHitProfile (Result result)
	{
		//Convert damage into net change in HP
		int hitModifier = -result.grossDamage;


		//If hit location is specified, return restricted random
		if (result.hitLocation) {

			if (result.hitLocation.name == "ColliderHigh" || result.hitLocation.name.Contains("head")) {
				return new CharacterHitProfile (hitModifier, 0, 0, 0, 0, 0);
			} else if (result.hitLocation.name == "ColliderMedium" || result.hitLocation.name.Contains("Arm") || result.hitLocation.name.Contains("spine") || result.hitLocation.name.Contains("gun")) {
				int bodyPartNumber = GetRandom1ToN (3);
				if (bodyPartNumber == 1) {
					return new CharacterHitProfile (0, hitModifier, 0, 0, 0, 0);
				} else if (bodyPartNumber == 2) {
					return new CharacterHitProfile (0, 0, hitModifier, 0, 0, 0);
				} else if (bodyPartNumber == 3) {
					return new CharacterHitProfile (0, 0, 0, hitModifier, 0, 0);
				} else {
					Debug.Log ("Broken random number");
					return new CharacterHitProfile ();
				}
			} else if (result.hitLocation.name == "ColliderLow" || result.hitLocation.name.Contains("Leg")) {
				if (Random.value >= 0.05) {
					return new CharacterHitProfile (0, 0, 0, 0, hitModifier, 0);
				} else {
					return new CharacterHitProfile (0, 0, 0, 0, 0, hitModifier);
				}
			} else {
				Debug.LogError ("Invalid hit location: " + result.hitLocation.name);
				return new CharacterHitProfile ();
			}
		} else {
			Debug.LogError ("Invalid hit location");
			return new CharacterHitProfile ();

			//If hit location is unspecified, return open random


			/*
		int bodyPartNumber = GetRandom1ToN(6);

		switch(bodyPartNumber)
		{
		case 1:
			return new CharacterHitProfile(hitModifier, 0, 0, 0, 0, 0);
		case 2:
			return new CharacterHitProfile(0, hitModifier, 0, 0, 0, 0);
		case 3:
			return new CharacterHitProfile(0, 0, hitModifier, 0, 0, 0);
		case 4:
			return new CharacterHitProfile(0, 0, 0, hitModifier, 0, 0);
		case 5:
			return new CharacterHitProfile(0, 0, 0, 0, hitModifier, 0);
		case 6:
			return new CharacterHitProfile(0, 0, 0, 0, 0, hitModifier);
		default:
			Debug.Log ("Invalid Body Part Number" + bodyPartNumber.ToString());
			return new CharacterHitProfile();	
		}
		*/
		}
	}

	//hit profile 0, hit profile 1 -> sum hit profile
	CharacterHitProfile SumHitProfiles (CharacterHitProfile hotProfile, CharacterHitProfile otherProfile)
	{
		hotProfile.head += otherProfile.head;
		hotProfile.body += otherProfile.body;
		hotProfile.leftArm += otherProfile.leftArm;
		hotProfile.rightArm += otherProfile.rightArm;
		hotProfile.leftLeg += otherProfile.leftLeg;
		hotProfile.rightLeg += otherProfile.rightLeg;
		
		return hotProfile;
	}
	
//QUERIES

	//If every character has finished moving, return true; otherwise, false
	bool MovementIsOver ()
	{
		foreach (CharacterSheet hotSheet in charactersInPlay) {
			if (hotSheet.GetComponent<ScriptCharacterController> ().atDestination == false) {
				return false;
			}
		}
		return true;
	}

	GridTile[ , , ] NewGameGrid (int gridSizeX, int gridSizeY, int gridSizeZ)
	{
		GridTile[ , ,] hotGrid = new GridTile[gridSizeX, gridSizeY, gridSizeZ];

		for (int i = 0; i < gridSizeX; i++) {

			//	GameObject xContainer = Instantiate(containerPrefab, transform.position, transform.rotation) as GameObject;
			//	xContainer.transform.parent = gridContainer.transform;
			//	xContainer.name = string.Format("X{0:00}", i);

			//Debug.Log(xContainer.name);

			for (int j = 0; j < gridSizeY; j++) {
				//		GameObject yContainer = Instantiate(containerPrefab, transform.position, transform.rotation) as GameObject;
				//		yContainer.transform.parent = xContainer.transform;
				//		yContainer.name = string.Format("Y{0:00}", j);
				
				for (int k = 0; k < gridSizeZ; k++) {
					Vector3 tileLocation = new Vector3 ((float)i * gridSpacing.x, (float)j * gridSpacing.y, (float)k * gridSpacing.z);

					hotGrid [i, j, k] = new GridTile (tileLocation);
					//Debug.Log(gameGrid[i, j, k].location.ToString());
					//Debug.Log(tileLocation);
					//			GameObject hotTile = Instantiate(gridTilePrefab, tileLocation, Quaternion.identity) as GameObject;
					//			hotTile.transform.parent = yContainer.transform;
					
				}
			}
		}

		return hotGrid;
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class ScriptGameMaster : MonoBehaviour {
	

	
	/*NOTES
	 * 
	 * 
	 * 
	 * */
	
	
	//Phases - Actions resolve in execution phase; players issue orders in command phase
	public enum Phase 
	{
		Execution,
		Command
	}
	
	//Modes - Play alternates between movement and engagement modes while in execution phase
	public enum Mode
	{
		Movement,
		Command,
		Engagement
	}

	public Phase gamePhase;
	public Mode gameMode;
	
	//Modes
	public bool movementMode = false;
	public bool engagementMode = false;
	
	//Interface
	GameObject interfaceController;
	ScriptInterface scriptInterface;
	public string inputButtonName = "";
	ScriptCycleDisplay scriptCycleDisplay;
	public GameObject damageDisplay;
	public float damageDisplayDepth = -1;
	
	//Characters
	public GameObject characterGameObjectTemplate;
	public List<ScriptCharacterSheet> charactersInPlay = new List<ScriptCharacterSheet>();
	public int nextCharacterID = 0;
	public List<ScriptCharacterSheet> activeCharacters = new List<ScriptCharacterSheet>();
	public int spawn00Time = -1;
	public int spawn01Time = -1;
	public GameObject conCharacter;
	public ScriptCharacterSheet selectedSheet;
	public ScriptCharacterSheet opposingSheet;
	
	//Items
	public List<Item> itemsInPlay = new List<Item>();
	
	//Space
	public Transform spawn00;
	public Transform spawn01;
	
	//Time
	public int cycle = -1;
	public float cycleTimer;
	public float cycleLength = 10;
	public float timerConstant = 1;
	
	//Mechanics
	
		//Tactics
	//public int aggressiveFirePriorityBonus = 10;
	
	//Camera
	Camera overviewCamera;
	
	//Physics
	ScriptPhysicsController scriptPhysicsController;
	
	//Effects
	//public GameObject energyBall;
	
	//Records
		//Database
	ScriptDatabase scriptDatabase;
		//Names
	List<string> firstNames = new List<string>(new string[] {"Jumbo", "Ham", "Tassik", 
		"Marinn", "Rose", "Joseph", "Dash", "Jaedon", "Argot", "Tau", "Rachel", "Julien", "Lily", "Larry", 
		"Maynard", "Leo", "Ota", "Gulliver", "Megan", "Freck", "Korder", "Lincoln"});
	List<string> lastNames = new List<string>(new string[] {"Baloney", "Jehosephat", "Kayla", 
		"Dillon", "Reynolds", "Wild", "Rendar", "Casio", "Veis", "Ceti", "Vega", "Pavec", "Puncture", 
		"Jello", "Thatcher", "Marshall", "Stockholm", "Retri", "Freck", "Korder", "Lincoln"});
	
	//Debug
	//public GameObject testCharacter;
	//public GameObject[] testArray;
	//public List<GameObject> tempCharactersInPlay = new List<GameObject>();
	
	// Use this for initialization
	void Start () {
		
		//Acquire scripts
		interfaceController = GameObject.Find ("ControllerInterface");
		scriptInterface = interfaceController.GetComponent<ScriptInterface>();
		scriptCycleDisplay = interfaceController.transform.FindChild("PanelCycle").GetComponent<ScriptCycleDisplay>();
		scriptPhysicsController = GameObject.Find ("ControllerPhysics").GetComponent<ScriptPhysicsController>();
		scriptDatabase = GetComponent<ScriptDatabase>();
		
		//Acquire camera
		overviewCamera = Camera.main;
		
		//Acquire controllers
		conCharacter = GameObject.Find ("ConCharacter");
		
		//Register each character object in the scene
		foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character")){
			ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			if(selectedSheet == null){				
				SetAsSelected(RegisterCharacter(hotSheet));
			} else {
				//ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
				//int output = hotSheet.GetCharacterPriority(0);
				//Debug.Log (output.ToString());
				RegisterCharacter(hotSheet);	
			}
		}
		
		//Spawn a random character on the left and right spawnpoints and give a random item
		//GiveCharacterItem(RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn00))), CreateRandomItem());
		
		//	RegisterCharacter(SetCharacterValues(NewCharacter(spawn00), scriptDatabase.coppermouth));
		
		GiveCharacterItem(RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn00))), CreateRandomItem());
		GiveCharacterItem(RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn01))), CreateRandomItem());
		
		RolloverCycle();
		
		//Debug
		//Debug.Log (charactersInPlay[0].GetComponent<ScriptCharacterSheet>().unequippedItems[0].netStatProfile.attackModifier);
	}
	
	// Update is called once per frame
	void Update () {
		
		
		//if(Input.GetKeyDown(KeyCode.N)){
		//	;
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
		if(inputButtonName != ""){
			ButtonHandler();
		}
		
		if(movementMode){
		cycleTimer += Time.deltaTime * timerConstant;
			if(cycleTimer >= cycleLength){
			SetToEngagementMode();
			//RolloverCycle();	
			}
		}
		
		
	}
	
	//BEGIN FUNCTIONS
	
	//Character Management
	ScriptCharacterSheet NewCharacter(Transform spawnTransform){
	
		//Create character at spawn point
			GameObject hotChar = Instantiate(characterGameObjectTemplate, spawnTransform.position, spawnTransform.rotation) as GameObject;
			ScriptCharacterSheet hotSheet = hotChar.GetComponent<ScriptCharacterSheet>();
			
			//Place in character container
			hotChar.transform.parent = conCharacter.transform;		
			
		//Assign left character as selected and right as opposing; assign position objective
		if(spawnTransform == spawn01){
			hotSheet.positionObjective = new Vector3(-3.75F, hotChar.transform.position.y, hotChar.transform.position.z);
			opposingSheet = hotSheet;
		} else if(spawnTransform == spawn00){		
			hotSheet.positionObjective = new Vector3(-5.25F, hotChar.transform.position.y, hotChar.transform.position.z);
		selectedSheet = hotSheet;	
		} else {
			Debug.Log ("Invalid spawn position");	
			}
			
			return hotSheet;
		}
		
	ScriptCharacterSheet RegisterCharacter(ScriptCharacterSheet hotSheet){
		
		scriptPhysicsController.SendMessage("Unragdollify", hotSheet.gameObject);
		
		hotSheet.characterID = nextCharacterID;
		nextCharacterID += 1;
		charactersInPlay.Add (hotSheet);
		hotSheet.stringID = hotSheet.characterID.ToString() + hotSheet.fullName;
		//hotSheet.fullName = hotSheet.stringID;
		
		//Assign object character name
		hotSheet.gameObject.name = hotSheet.stringID;
		
		//Set color
		GameObject hotModel = hotSheet.gameObject.transform.FindChild("ObjectCharacterModel").gameObject;
		hotModel.SendMessage("InitializeModel");
		hotModel.SendMessage("ColorCharacter");
		
		//Set Derived Stats
		hotSheet.maxHitProfile = new CharacterHitProfile(2 + hotSheet.toughness, 5 + hotSheet.toughness, 
		                                                 3 + hotSheet.toughness, 3 + hotSheet.toughness,
		                                                 4 + hotSheet.toughness, 4 + hotSheet.toughness);
		hotSheet.currentHitProfile = new CharacterHitProfile(hotSheet.maxHitProfile);

		hotSheet.unarmedDamage = hotSheet.baseMuscle / 2;
		
		//Set as unarmed
		hotSheet.activeItem = scriptDatabase.unarmed;
		
		//Set Tactics
		hotSheet.activeTactics.Add(scriptDatabase.tacticsLookup["Basic Shot"]);
		
		
	
		return hotSheet;
	}
		
	ScriptCharacterSheet SetAsSelected(ScriptCharacterSheet hotSheet){
		selectedSheet = hotSheet;
		return hotSheet;	
	}
	
	ScriptCharacterSheet RandomizeCharacterValues(ScriptCharacterSheet hotSheet){
		
		//Set name and update game object
		hotSheet.firstName = firstNames[(int)Mathf.Floor(Random.value*firstNames.Count)];
		hotSheet.lastName = lastNames[(int)Mathf.Floor(Random.value*lastNames.Count)];
		hotSheet.fullName = hotSheet.firstName+ " " + hotSheet.lastName;
		//hotSheet.stringID = hotSheet.characterID.ToString() + hotSheet.firstName.ToString() + hotSheet.lastName.ToString();
		//hotSheet.name = hotSheet.stringID;
		
		//Assign Colors
		hotSheet.primaryColor = GetRandomColor();
		hotSheet.secondaryColor = GetRandomColor();
		
		//Assign Stats
		//hotSheet.frameSize = GetRandomFrameSize();

		hotSheet.toughness = GetRandom1ToN(5);

		hotSheet.currentFocus = GetRandom1ToN(10);
		hotSheet.baseMuscle = GetRandom1ToN(10);
		hotSheet.baseBrawl = GetRandom1ToN(10);
		hotSheet.baseMelee = GetRandom1ToN(10);
		hotSheet.baseShot = GetRandom1ToN(10);
		hotSheet.baseEvasion = GetRandom1ToN(10);
		hotSheet.baseIntelligence = GetRandom1ToN(10);
		hotSheet.basePresence = GetRandom1ToN(10);
		/*
		//hotSheet.baseAttack = GetRandom1ToN(10);
		//hotSheet.baseDefense = GetRandom1ToN(10);
		//hotSheet.unarmedRange = GetRandom1ToN(6);
		
		//Assign Tactics
		//hotSheet.targetReassess = GetRandomBool();
		//if(GetRandomBool()){
		
		//	hotSheet.engageAtRange = true;
		//	hotSheet.engageInMelee = false;
		
		//} else {
		//	hotSheet.engageAtRange = false;
		//	hotSheet.engageInMelee = true;
		//}
		
		//Assign Derived Stats
		
		//hotSheet.priority = hotSheet.focus;

		//hotSheet.weaponCooldown = GetRandom1ToN(3);
		//hotSheet.weaponRange = 1;
		//resistance
		*/
		return hotSheet;
		
	}
	
	ScriptCharacterSheet SetCharacterValues (ScriptCharacterSheet hotSheet, CharacterTemplate hotTemplate)
	{
		//Set name and update game object
		//hotSheet.firstName = firstNames[(int)Mathf.Floor(Random.value*firstNames.Count)];
		//hotSheet.lastName = lastNames[(int)Mathf.Floor(Random.value*lastNames.Count)];
		hotSheet.fullName = hotTemplate.fullName;
		hotSheet.stringID = hotSheet.characterID.ToString() + hotSheet.fullName;
		hotSheet.name = hotSheet.stringID;
		
		//Assign Colors
		hotSheet.primaryColor = hotTemplate.primaryColor;
		hotSheet.secondaryColor = hotTemplate.secondaryColor;
		hotSheet.skinColor = hotTemplate.skinColor;
		
		//Assign Stats
		hotSheet.frameSize = hotTemplate.frameSize;
		//hotSheet.baseToughness = hotTemplate.characterStatProfile.toughness;
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
	
	int GetCharactersInPlayIndex(ScriptCharacterSheet hotSheet){
		for(int i = 0; i < charactersInPlay.Count; i++){
			if(hotSheet == charactersInPlay[i])
			{
			return i;	
			}
		}
		
		Debug.Log ("Character Index not found.");
		return -9999;
	}
	
	void KillCharacter(ScriptCharacterSheet hotSheet){

		//Remove dead character from characters in play 
		charactersInPlay.RemoveAt(GetCharactersInPlayIndex(hotSheet));
		
		//Set character's inPlay to false
		hotSheet.inPlay = false;
		
		//Disable character model's face
		//GameObject hotFace = hotSheet.gameObject.GetComponentInChildren<ScriptModelController>().face;
		//hotFace.SetActive(false);
		//transform.Find ("ObjectCharacterObjectCharacterModelheadfacefaceImage").gameObject.SetActive(false);
		
		//Remove character as an valid target
		foreach(ScriptCharacterSheet otherHotSheet in charactersInPlay){
			//ScriptCharacterSheet otherHotSheet = character.GetComponent<ScriptCharacterSheet>();
			if(otherHotSheet.target == hotSheet){
				otherHotSheet.target = null;
			}
		}
		//Log death
		scriptInterface.SendMessage("AddNewLine", hotSheet.fullName + " dies.");
		
		//Death physics
		//ScriptCharacterSheet lastAttackerSheet = hotSheet.lastAttacker.GetComponent<ScriptCharacterSheet>();
		//	scriptPhysicsController.propel = lastAttackerSheet.propel;
		//scriptPhysicsController.blowUpHead = lastAttackerSheet.blowUpHead;
			
		
		//scriptPhysicsController.SendMessage("ExecuteCharacter", hotSheet.gameObject);
		
		//Set new character spawn time
		if(hotSheet.gameObject.transform.rotation.y == 0){
			
					spawn00Time = cycle + 3;
		} else
		//if(hotSheet.gameObject.transform.rotation.y == 180)
		{
					spawn01Time = cycle + 3;
		}
		//} else {
		//Debug.Log ("Unexpected");	
		//}
}
	
	
	//Progress to next event
	void ResolveEngagements(){
		
		if(engagementMode)
		{
			//Set activeCharacters
			UpdateCharacterValues();
			GetActiveCharacters();
		//Debug.Log (activeCharacters.Count.ToString());
			
			//If there any characters left to act for this Cycle, then execute next action
			if(activeCharacters.Count >= 1)
			{
				//1. Determine character order
				SortActiveCharacters();
				
				//2. Exceute next action in queue
				ExecuteAction(activeCharacters[0]);
				
				//3. If more than one character and characters are tied for priority, both actions resolve before registering new states
				//Warning: It seems like this will not work for more than one tied charcter
				if(activeCharacters.Count > 1 && activeCharacters[0].readyPriority == activeCharacters[1].readyPriority)
				{
				ExecuteAction(activeCharacters[1]);	
				}

				//CharacterCleanup();
				
				//4. Update character target, destination, stats
				UpdateCharacterValues();
				ResolveEngagements ();
			} 
			else 
			{
				RolloverCycle();
			}
		} else {
			Debug.Log ("Error: Attempt to resolve engagement outside of engagement mode");
		}
	}
	void ExecuteAction(ScriptCharacterSheet hotSheet){
		
		Result result = null;
		if(hotSheet.target)
		{
			ScriptCharacterSheet targetSheet = hotSheet.target;

			//Get action result
			result = GetActionResult(hotSheet, targetSheet);

			//Change states
			if(result.success)
			{
				//Reduce health
				SumHitProfiles(result.targetCharacter.currentHitProfile, result.targetNetHitProfile);
			}
			
			//Apply damage results
			if(targetSheet.currentHitProfile.head <= 0 || targetSheet.currentHitProfile.body <= 0)
			{
				KillCharacter(targetSheet);	

				//Start encounter cam on dead character
				RunCinematicCamera(result.targetCharacter.GetComponent<ScriptCharacterController>());
			}

			//Initiate effect
			scriptPhysicsController.SendMessage("InitiateActionEffect", result);

		} else {
			//Character attacks nothing
			scriptInterface.SendMessage("AddNewLine",hotSheet.fullName + " attacks... nothing.");
		}

		//Reset Wait Time to Delay
		hotSheet.waitTime = 1; //Magic number

			

			

		//Log action
			string hotLine = result.actingCharacter.fullName +
			" attacks " +
			result.targetCharacter.fullName +
			" (" +
			result.actingAttack.ToString() +
			" ATT vs " +
			result.targetDefense.ToString() +
			" DEF: " +
			result.hitPercentage.ToString() +
			"%). Roll: " +
			result.roll.ToString();
			
			if(result.success)
		{
			hotLine += " > " +
			result.successNumber.ToString() + 
			". " +
			result.actingCharacter.fullName + 
			" shoots " +
			result.targetCharacter.fullName + 
			" for " + 
			result.grossDamage.ToString() +
			" " +
			result.damageType +
			" damage.";	
		}
		else
		{
			hotLine += " <= " +
			result.successNumber.ToString() + 
			". " +
			result.actingCharacter.fullName +
					" misses.";
		}
		ConsoleAddLine(hotLine);
		
		//Display damage
			GameObject currentDamageDisplay = Instantiate(damageDisplay, new Vector3(result.targetCharacter.gameObject.transform.position.x,
					result.targetCharacter.gameObject.transform.position.y,damageDisplayDepth), Quaternion.identity) as GameObject;
				 TextMesh statusChangeText = currentDamageDisplay.GetComponentInChildren<TextMesh>();
				
				if(result.success)
				{
					statusChangeText.text = "-" + result.grossDamage + "HP";
				}
				else
				{
					statusChangeText.text = "Miss";
				}
		
	}
	
	//Change cycle
	void RolloverCycle(){
		
				//Begin new Cycle
		cycle += 1;
		//Begin Command Phase
		SetToCommandMode();
		//Stop all movement
		//StartCoroutine("RedLight");
		
		//Reduce all characters' wait time by 1
		foreach(ScriptCharacterSheet hotSheet in charactersInPlay){
			//ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			if(hotSheet.waitTime > 0)
			{
			hotSheet.waitTime -= 1;
			}
		}

		//Begin cycle timer at zero
		cycleTimer = 0.0F;
		//Log new Cycle
		scriptCycleDisplay.SendMessage("UpdateCycle",cycle);
		//scriptInterface.SendMessage("AddNewLine", "Cycle " + cycle.ToString());
		
		//Ensure all characters have valid targets
		UpdateCharacterValues();
		
		
	}

	
	//Prepare queue
	void GetActiveCharacters(){
		
		activeCharacters = new List<ScriptCharacterSheet>();
		foreach(ScriptCharacterSheet currentSheet in charactersInPlay){
			//ScriptCharacterSheet currentSheet = character.GetComponent<ScriptCharacterSheet>();
			if(currentSheet.waitTime == 0 && currentSheet.isInActingPosition){
				activeCharacters.Add (currentSheet);
			}
		}
		
	}
	void SortActiveCharacters(){
		int initialCount = activeCharacters.Count;
		List<ScriptCharacterSheet> tempList = new List<ScriptCharacterSheet>();
		while(tempList.Count < initialCount){	
			//Determine highest priority of remaining active characters
			float maxPriority = 0.0F;
			for(int i = 0; i < activeCharacters.Count; i++){
			float currentPriority = activeCharacters[i].readyPriority;
			if(currentPriority > maxPriority)
				maxPriority = currentPriority;
			}	
		
			//Add highest priority character to temporary list and remove from active characters
			bool findingNextCharacter = true;
			int j = 0;
			while(findingNextCharacter){	
				if(activeCharacters[j].readyPriority == maxPriority){
					tempList.Add (activeCharacters[j]);
					findingNextCharacter = false;
					activeCharacters.RemoveAt (j);
				} else {
					j++;
				}
			}
		}
		
		activeCharacters = tempList;
		
	}
	
	//Maintenence
	//void CharacterCleanup(){
		//Debug.Break ();
		//List<ScriptCharacterSheet> tempCharactersInPlay = new List<ScriptCharacterSheet>(charactersInPlay);
		//Debug.Log ("temp contains " + tempCharactersInPlay.Count);
	//}
	void UpdateCharacterValues(){
		
		//Update targets
		foreach(ScriptCharacterSheet hotSheet in charactersInPlay){
			//ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			
			if(hotSheet.target == null){
				//For all charactersInPlay without Targets:
				//bool assigningNewTarget = true;
				
				if(charactersInPlay.Count > 1){
				//Choose random character
				
				//int i = (int)Mathf.Floor(Random.value*charactersInPlay.Count);
				//while(assigningNewTarget){
				int randomCharacterIndex = (int)Mathf.Floor(Random.value*charactersInPlay.Count);
				ScriptCharacterSheet otherCharacter = charactersInPlay[randomCharacterIndex];
				
				//If random character is not first character, assign as target (no character can be a target of him/herself
				if(otherCharacter != hotSheet){
						hotSheet.target = otherCharacter;
						//assigningNewTarget = false;
				} else {
					//If it is first character, use next character in line
						hotSheet.target = charactersInPlay[((randomCharacterIndex+1)%(charactersInPlay.Count))];
				}
				}
			}
			
			//Update Equipment modifiers
			hotSheet.netEquipmentAttack = hotSheet.activeItem.itemStatProfile.attackModifier;
			hotSheet.netEquipmentDamage = hotSheet.activeItem.itemStatProfile.damageModifier;
			//hotSheet.netEquipmentDefense = hotSheet.activeItem.netStatProfile.
			hotSheet.netEquipmentPriority = hotSheet.activeItem.itemStatProfile.priorityModifier;
			hotSheet.netEquipmentRange = hotSheet.activeItem.itemStatProfile.maxRangeAspect;
		

			
			//Update Tactic modifiers --only works for first tactic at the moment
			hotSheet.netTacticsAttack = hotSheet.activeTactics[0].tacticStatProfile.attack;
			hotSheet.netTacticsDamage = hotSheet.activeTactics[0].tacticStatProfile.damage;
			hotSheet.netTacticsDefense = hotSheet.activeTactics[0].tacticStatProfile.defense;
			hotSheet.netTacticsPriority = hotSheet.activeTactics[0].tacticStatProfile.priority;
			hotSheet.netTacticsRange = hotSheet.activeTactics[0].tacticStatProfile.maxRange;
			
			//Update ready stats
			
				//Update ready attack
			int baseAttack;
			switch(hotSheet.activeItem.attackType)
			{
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
				Debug.Log ("Invalid Attack Type " + hotSheet.activeItem.attackType.ToString());
				baseAttack = -9999;
				break;
			}
			
			hotSheet.readyAttack = baseAttack + hotSheet.netEquipmentAttack + hotSheet.netTacticsAttack;		
			hotSheet.readyDefense = hotSheet.baseEvasion + hotSheet.netTacticsDefense;
			hotSheet.readyPriority = hotSheet.currentFocus + hotSheet.netEquipmentPriority + hotSheet.netTacticsPriority;
			hotSheet.readyRange = hotSheet.netEquipmentRange + hotSheet.netTacticsRange;
			
			//Add muscle only if close combat
			if(hotSheet.activeItem.attackType == AttackType.Shot)
			{
			hotSheet.readyDamage = hotSheet.netEquipmentDamage + hotSheet.netTacticsDamage;
			} 
			else if(hotSheet.activeItem.attackType == AttackType.Brawl || hotSheet.activeItem.attackType == AttackType.Melee)
			{
				hotSheet.readyDamage = hotSheet.baseMuscle + hotSheet.netEquipmentDamage + hotSheet.netTacticsDamage;
			}
			else
			{
				Debug.Log ("Invalid Attack Type: " + hotSheet.activeItem.attackType);
			}
			//Update currentHitChance
			
		}
		
		//Update isInActingPosition
		foreach(ScriptCharacterSheet hotSheet in charactersInPlay)
		{
			//ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			hotSheet.isInActingPosition = hotSheet.GetComponentInChildren<ScriptControllerTargeting>().GetActingPosition(hotSheet);
		}
		
		
		
						
	}
	
	//void ExecuteRangedAttack(ScriptCharacterSheet hotSheet){
		
	/*Needs revision
	void ExecuteMeleeAttack(ScriptCharacterSheet hotSheet){
		ScriptCharacterSheet targetSheet = hotSheet.target.GetComponent<ScriptCharacterSheet>();
		
						scriptInterface.SendMessage("AddNewLine",hotSheet.fullName
			+ " attacks " + targetSheet.fullName + "! " + 
				hotSheet.melee.ToString() + " Melee vs. " + targetSheet.evasion.ToString() + " Evasion");
			//Compare attacker's Melee to target's Defense
			if(hotSheet.melee > targetSheet.evasion){
				targetSheet.health -= hotSheet.damage;
				scriptInterface.SendMessage("AddNewLine",hotSheet.fullName
				+ " deals " + hotSheet.damage.ToString() + " damage to "+ targetSheet.fullName
				+ ". " + targetSheet.health.ToString() + " Health remaining.");
			} else {
				scriptInterface.SendMessage("AddNewLine",hotSheet.fullName + " misses!");
		}
	}
	*/
	//HANDLER FUNCTIONS

	void ButtonHandler(){
			if(charactersInPlay.Count >= 1){
			//ScriptCharacterSheet selectedSheet = charactersInPlay[0].GetComponent<ScriptCharacterSheet>();
					
			string hotButton = inputButtonName;
			//playerPrompt = false;
			inputButtonName = null;
			switch(hotButton){	
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
					SetToExecutionPhase();
					break;
			case null:
				break;
				default:
					Debug.Log ("Error 002: Button name " + hotButton + " is invalid.");
					break;
			}	
		}
		}
		
	//HELPER FUNCTIONS
	
	void ConsoleAddLine(string line)
	{
	scriptInterface.SendMessage("AddNewLine", line);	
	}
	
	bool GetRandomBool(){
		if(Random.value >= .5){
		return true;	
		} else {
		return false;
		}
			
	}
	
	int GetRandom1ToN(int n){
		return (int)Mathf.Floor(Random.value*n + 1);
	}
	
	//int GetRandom1To10(){
	//	return (int)Mathf.Floor(Random.value*10 + 1);
	//}
	
	//int GetRandom1to255(){
	//	int hotValue = (int)Mathf.Floor (Random.value * 255 + 1);
		//Debug.Log (hotValue.ToString());
	//		return hotValue;
	//}
	
	Color GetRandomColor(){
		//Color test = new Color(
	    return new Color(
			Random.value,
			Random.value,
			Random.value,
			255);
			
	}
	
	//int GetCharacterPriority(ScriptCharacterSheet hotSheet){
	//	return hotSheet.nerve;
	//}
	
	FrameSize GetRandomFrameSize()
	{
		int hotInt = GetRandom1ToN(3);
		switch (hotInt)
		{
		case 1:
			return FrameSize.Small;
		case 2:
			return FrameSize.Medium;
		case 3: 
			return FrameSize.Large;
		default:
			Debug.Log ("Invalid Frame Number " + hotInt.ToString());
			return FrameSize.None;
		}
	}

	/*
	int GetFrameNumber(ScriptCharacterSheet hotSheet)
	{
		switch(hotSheet.frameSize)
		{
		case FrameSize.Small:
			return 1;
		case FrameSize.Medium:
			return 2;
		case FrameSize.Large:
			return 3;
		default:
			Debug.Log ("Invalid Frame Size: " + hotSheet.frameSize.ToString());
			return -9999;
		}	
	}
	*/
	//MODE TOGGLE
	
	void SetToExecutionPhase(){
	//Debug.Log ("Execution Phase");
		
	gamePhase = Phase.Execution;
		
		//Adjust Stats according to Tactics
		//ApplyTactics();
		
		
		//Spawn characters if necessary
		if(spawn00Time == cycle){
		GiveCharacterItem(RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn00))), CreateRandomItem());	;
		//ScriptCharacterSheet hotSheet = NewCharacter(spawn00);
		//RegisterCharacter(RandomizeCharacterValues(hotSheet));
			//hotSheet.activeItem = null;
		//	Debug.Log (hotSheet.activeItem.currentAmmo.ToString());
		
			//Debug.Log ("RespawnLeft");
		} 
		if(spawn01Time == cycle){
		GiveCharacterItem(RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn01))), CreateRandomItem());	;
			//Debug.Log ("RespawnRight");
		}
		
		//SetToEngagementMode();
		SetToMovementMode();
	}
	
	
	
	void SetToCommandMode(){
		//Debug.Log ("CommandPhase");
		gamePhase = Phase.Command;
		gameMode = Mode.Command;
		//engagementMode = false;
		//movementMode = false;
		//executionPhase = false;
		//commandPhase = true;
		
		
		//Skip Command Phase
		SetToExecutionPhase();
	}
	
	void SetToMovementMode(){
		//Debug.Log ("MovementMode");
		//commandPhase = false;
		engagementMode = false;
		movementMode = true;
		foreach(ScriptCharacterSheet hotSheet in charactersInPlay){
		ScriptCharacterController hotScript = hotSheet.GetComponent<ScriptCharacterController>();
			hotScript.greenLight = true;
			hotScript.startLerp = true;
		}
		
	
	}
	
	void SetToEngagementMode(){
		//Debug.Log ("EngagementMode");
		//playerPrompt = false;
		movementMode = false;
		engagementMode = true;
			StartCoroutine("RedLight");
	}
	
	//Wait for every character to finish their frame of movement, then stop all characters
	IEnumerator RedLight(){
		//Debug.Log ("RedLight");
		yield return 0;
		foreach(ScriptCharacterSheet hotSheet in charactersInPlay){
		ScriptCharacterController hotScript = hotSheet.GetComponent<ScriptCharacterController>();
			hotScript.greenLight = false;
		}
		if(engagementMode){
		ResolveEngagements ();
		}
		//}
		
	}
	
	//Queries
	bool MovementIsOver(){
		foreach(ScriptCharacterSheet hotSheet in charactersInPlay){
			if(hotSheet.GetComponent<ScriptCharacterController>().atDestination == false){
				//Debug.Log (character.GetComponent<ScriptCharacterMove>().fracJourney);
				return false;
		}
		}
		return true;
		
	}
	
	Result GetActionResult(ScriptCharacterSheet actingCharacter, ScriptCharacterSheet targetCharacter)
	{


		Result result = new Result(actingCharacter);
		result.targetCharacter = targetCharacter;

	

		result.actingAttack = actingCharacter.readyAttack;
		result.targetDefense = targetCharacter.readyDefense;
		
		//Calculate success number
		result.hitPercentage = GetHitPercentage(result.actingAttack, result.targetDefense);
		result.successNumber = 100 - result.hitPercentage;
		
		//Roll d100	
		result.roll = GetRandom1ToN(100);
			
		//If roll is greater than or equal to the success number, attack succeeds
		result.rollExcess = result.roll - result.successNumber;
		if(result.rollExcess >= 1)
		{
			result.success = true;

			//Record damage
			result.grossDamage = actingCharacter.readyDamage;
			result.damageType = actingCharacter.activeItem.damageType;
			result.targetGrossHitProfile = GetGrossHitProfile(result);

			//Debug.Log(result.targetGrossHitProfile.head.ToString() + result.targetGrossHitProfile.body.ToString() + 
			  //        result.targetGrossHitProfile.leftArm.ToString() + result.targetGrossHitProfile.rightArm.ToString() + 
			    //      result.targetGrossHitProfile.leftLeg.ToString() + result.targetGrossHitProfile.rightLeg.ToString());


			//Debug.Log(result.grossDamage);


			//Apply armor
			
			result.targetNetHitProfile = SumHitProfiles(result.targetGrossHitProfile, result.targetCharacter.resistanceHitProfile);


			//result.hitLocation = GetHitLocation(result.rollExcess);

			
			//Debug.Log (result.damageAmount.ToString() + " damage to " + result.hitLocation.ToString());
		}
		else
		{
			result.success = false;
		}
		return result;
	}
	
	int GetHitPercentage(int actingAttack, int targetDefense)
	{
		return (10+actingAttack-targetDefense)*5;	
	}
	
	void GiveCharacterItem (ScriptCharacterSheet character, Item hotItem)
	{
		//If item is unowned
		if(hotItem.owner == null)
		{
			//Add to equipped items and set character as new owner
	character.equippedItems.Add (hotItem);
			character.activeItem = hotItem;
			hotItem.owner = character;
		}
		else
		{
			Debug.Log("Item is already owned");
		}
	}
	
	CharacterHitProfile GetGrossHitProfile(Result result)
	{
		//List<BodyPart> hitLocations = new List<BodyPart>();
		int bodyPartNumber = GetRandom1ToN(6);

		int hitModifier = -result.grossDamage;

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
	}
	
	Item CreateRandomItem()
		{
		Item hotItem = scriptDatabase.GetRandomItem();
		itemsInPlay.Add(hotItem);
		return hotItem;
		}
		
	void RunCinematicCamera (ScriptCharacterController character)
	{
		Camera hotCam = character.characterCameras[2];
			/*
		if(Random.value >= 0.5)
		{
			//character.cinematicCamera0.enabled = true;
			hotCam = character.cinematicCamera0;
		}
		else
		{
			//character.cinematicCamera1.enabled = true;
			hotCam = character.cinematicCamera1;
		}
		//Debug.Log (hotCam.ToString());

*/

		overviewCamera.enabled = false;
		hotCam.enabled = true;
		//Debug.Break();
		//StartCoroutine("StopCinematicCamera", hotCam);
		//yield return new WaitForSeconds(2.0);
	}
	
	IEnumerator StopCinematicCamera(Camera hotCam)
	{
		yield return new WaitForSeconds(2.0F);
		
		hotCam.enabled = false;
		overviewCamera.enabled = true;
	}

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

}

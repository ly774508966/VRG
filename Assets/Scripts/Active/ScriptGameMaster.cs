using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptGameMaster : MonoBehaviour {
	

	
	/*NOTES
	 * 
	 * 
	 * 
	 * */
	
	//Phases
	public bool executionPhase = false;
	public bool commandPhase = false;
	
	//Modes
	public bool movementMode = false;
	public bool engagementMode = false;
	//public bool endOfCycle = false;
	
	
	//Interface
	GameObject interfaceMain;
	ScriptInterface scriptInterface;
	public string inputButtonName = "";
	
	ScriptCycleDisplay scriptCycleDisplay;
	public GameObject damageDisplay;
	public float damageDisplayDepth = -1;
	
	//Characters
	public GameObject characterTemplate;
	public List<GameObject> charactersInPlay = new List<GameObject>();
	public int nextCharacterID = 0;
	public List<GameObject> activeCharacters = new List<GameObject>();
	public int spawn00Time = -1;
	public int spawn01Time = -1;
	public GameObject conCharacter;
	public ScriptCharacterSheet selectedSheet;
	public ScriptCharacterSheet opposingSheet;
	
	//Space
	public Transform spawn00;
	public Transform spawn01;
	
	//Time
	public int cycle = -1;
	public float cycleTimer;
	public float cycleLength = 10;
	public float timerConstant = 1;
	
	//Gameplay
	
		//Tactics
	//public int aggressiveFirePriorityBonus = 10;
	
	
	//Physics
	ScriptPhysicsController scriptPhysicsController;
	
	//Effects
	public GameObject energyBall;
	
	//Database
	public List<string> firstNames = new List<string>(new string[] {"Jumbo", "Ham", "Tassik", 
		"Marinn", "Rose", "Joseph", "Dash", "Jaedon", "Argot", "Tau", "Rachel", "Julien", "Lily", "Larry", 
		"Maynard", "Leo", "Ota", "Gulliver", "Megan", "Freck", "Korder", "Lincoln"});
	public List<string> lastNames = new List<string>(new string[] {"Baloney", "Jehosephat", "Kayla", 
		"Dillon", "Reynolds", "Wild", "Rendar", "Casio", "Veis", "Ceti", "Vega", "Pavec", "Puncture", 
		"Jello", "Thatcher", "Marshall", "Stockholm", "Retri", "Freck", "Korder", "Lincoln"});
	
	//Debug
	//public GameObject testCharacter;
	//public GameObject[] testArray;
	
	// Use this for initialization
	void Start () {
		
		
		
		//Acquire scripts
		interfaceMain = GameObject.Find ("InterfaceMain");
		scriptInterface = interfaceMain.GetComponent<ScriptInterface>();
		scriptCycleDisplay = interfaceMain.transform.FindChild("PanelCycle").GetComponent<ScriptCycleDisplay>();
		scriptPhysicsController = GameObject.Find ("ControllerPhysics").GetComponent<ScriptPhysicsController>();
		
		//Acquire controllers
		conCharacter = GameObject.Find ("ConCharacter");
		
		//Register each character object in the scene
		foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character")){
			if(selectedSheet == null){				
				SetAsSelected(RegisterCharacter(character));
			} else {
				//ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
				//int output = hotSheet.GetCharacterPriority(0);
				//Debug.Log (output.ToString());
				RegisterCharacter(character);	
			}
		}
		
		//Spawn a random character on the left and right spawnpoints
		RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn00)));
		RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn01)));
		
		//NextStep ();
		RolloverCycle();
		
		//RegisterCharacter(testCharacter);	
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
		//if(Input.GetKeyDown(KeyCode.N)){
		//	;
		//	RandomizeCharacterValues(RegisterCharacter(NewCharacter()));
		//}
		
		if(Input.GetKeyDown(KeyCode.C)){
		foreach(GameObject character in charactersInPlay)
			{
			character.transform.FindChild("ObjectCharacterModel").SendMessage("ColorCharacter");	
			}
		}
		
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
	GameObject NewCharacter(Transform spawnTransform){
	//Create character object at spawn position
		//if(spawnPosition == 0)
		//{
	
			
		//Create character at spawn point
			GameObject hotChar = Instantiate(characterTemplate, spawnTransform.position, spawnTransform.rotation) as GameObject;
			ScriptCharacterSheet hotSheet = hotChar.GetComponent<ScriptCharacterSheet>();
			
			//Place in character container
			hotChar.transform.parent = conCharacter.transform;		
			
		//Assign left character as selected and right as opposing; assign position objective
		if(spawnTransform == spawn00){
			hotSheet.positionObjective = new Vector3(-1.75F, hotChar.transform.position.y, hotChar.transform.position.z);
			selectedSheet = hotSheet;
		} else if(spawnTransform == spawn01){		
			hotSheet.positionObjective = new Vector3(-3.25F, hotChar.transform.position.y, hotChar.transform.position.z);
		opposingSheet = hotSheet;	
		} else {
			Debug.Log ("Invalid spawn position");	
			}
			
			
			
			
			return hotChar;
		//}
		//else if(spawnPosition == 1)
		//{
		//	GameObject hotChar = Instantiate(characterTemplate, spawn01.position, spawn01.rotation) as GameObject;
		//	ScriptCharacterSheet hotSheet = hotChar.GetComponent<ScriptCharacterSheet>();
		//	hotSheet.positionObjective = new Vector3(-3.25F, hotChar.transform.position.y, hotChar.transform.position.z);
		//	return hotChar;
			
		//}
		//else 
		//{
		//	Debug.Log ("Invalid Spawn Position");
		//	return null;
		
			
		}
		
		//return hotChar;
		//return Instantiate(characterTemplate, spawnTransform.position, spawnTransform.rotation) as GameObject;
			
			//new Vector3(spawnPosition.x + nextCharacterID * 2, spawnPosition.y, spawnPosition.z), transform.rotation) as GameObject;
	
	GameObject RegisterCharacter(GameObject character){
		ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
		
		scriptPhysicsController.SendMessage("Unragdollify", character);
		
		hotSheet.characterID = nextCharacterID;
		nextCharacterID += 1;
		charactersInPlay.Add (character);
		hotSheet.stringID = hotSheet.characterID.ToString() + hotSheet.firstName + hotSheet.lastName;
		hotSheet.fullName = hotSheet.stringID;
		
		//Assign object character name
		character.name = hotSheet.stringID;
		
		//Set color
		GameObject hotModel = hotSheet.gameObject.transform.FindChild("ObjectCharacterModel").gameObject;
		hotModel.SendMessage("InitializeModel");
		hotModel.SendMessage("ColorCharacter");
		
		
		return character;
	}
	
	GameObject SetAsSelected(GameObject character){
				//If first character, assign as selected
		
			selectedSheet = character.GetComponent<ScriptCharacterSheet>();
		return character;
		
	}
	GameObject RandomizeCharacterValues(GameObject character){
		ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
		
		//Set name and update game object
		hotSheet.firstName = firstNames[(int)Mathf.Floor(Random.value*firstNames.Count)];
		hotSheet.lastName = lastNames[(int)Mathf.Floor(Random.value*lastNames.Count)];
		hotSheet.fullName = hotSheet.firstName+ " " + hotSheet.lastName;
		hotSheet.stringID = hotSheet.characterID.ToString() + hotSheet.firstName.ToString() + hotSheet.lastName.ToString();
		hotSheet.name = hotSheet.stringID;
		
		//Assign Colors
		hotSheet.primaryColor = GetRandomColor();
		hotSheet.secondaryColor = GetRandomColor();
		
		//Assign Stats
		hotSheet.health = GetRandom1To10();
		hotSheet.focus = GetRandom1To10();
		hotSheet.damage = GetRandom1To10();
		hotSheet.speed = GetRandom1To10();
		hotSheet.accuracy = GetRandom1To10();
		hotSheet.evasion = GetRandom1To10()/2;
		hotSheet.armor = 0;
		hotSheet.melee = 0;
		
		//Assign Tactics
		//hotSheet.targetReassess = GetRandomBool();
		//if(GetRandomBool()){
			hotSheet.engageAtRange = true;
			hotSheet.engageInMelee = false;
		//} else {
		//	hotSheet.engageAtRange = false;
		//	hotSheet.engageInMelee = true;
		//}
		
		//Assign Derived Stats
		
		hotSheet.delay = 1;
		//hotSheet.priority = hotSheet.focus;
		
		
		
		return (character);
		
	}

	void KillCharacter(ScriptCharacterSheet hotSheet, int characterIndex){
						
		//Remove dead character from characters in play 
		charactersInPlay.RemoveAt(characterIndex);
		//Set character's inPlay to false
		hotSheet.inPlay = false;
		
		//Disable character model's face
		//GameObject hotFace = hotSheet.gameObject.GetComponentInChildren<ScriptModelController>().face;
		//hotFace.SetActive(false);
		
		//transform.Find ("ObjectCharacterObjectCharacterModelheadfacefaceImage").gameObject.SetActive(false);
		
		//Remove character as an valid target
		foreach(GameObject character in charactersInPlay){
			ScriptCharacterSheet otherHotSheet = character.GetComponent<ScriptCharacterSheet>();
			if(otherHotSheet.target == hotSheet.gameObject){
				otherHotSheet.target = null;
			}
		}
		//Log death
		scriptInterface.SendMessage("AddNewLine", hotSheet.fullName + " dies.");
		
		//Death physics
	ScriptCharacterSheet lastAttackerSheet = hotSheet.lastAttacker.GetComponent<ScriptCharacterSheet>();
			scriptPhysicsController.propel = lastAttackerSheet.propel;
		scriptPhysicsController.blowUpHead = lastAttackerSheet.blowUpHead;
			
		
		scriptPhysicsController.SendMessage("ExecuteCharacter", hotSheet.gameObject);
		
		//Set new character spawn time
		if(hotSheet.gameObject.transform.rotation.y == 0){
			
					spawn01Time = cycle + 3;
		} else
		//if(hotSheet.gameObject.transform.rotation.y == 180)
		{
					spawn00Time = cycle + 3;
		}
		//} else {
		//Debug.Log ("Unexpected");	
		//}
}
	
	
	//Progress to next event
	void ResolveEngagement(){
		
		if(engagementMode){
			//CharacterCleanup();
			//UpdateTargets();
			//Set activeCharacters
			UpdateCharacterValues();
			GetActiveCharacters();
		//Debug.Log (activeCharacters.Count.ToString());
			
			//If there any characters left to act for this Cycle, then execute next action
			if(activeCharacters.Count >= 1)
			{
				//1. Determine character order
				SortActiveCharacters();
				
				//Exceute next action in queue
				ExecuteNextAction();
				
				//Kill necessary characters
				CharacterCleanup();
				
				//Update character target, destination, stats
				UpdateCharacterValues();
				ResolveEngagement ();
			} 
			else 
			{
			//Debug.Log (MovementIsOver());
			//if(MovementIsOver()){
			//	RolloverCycle();
			//} else {
				//SetToMovementMode();
				RolloverCycle();
			//}
			}
		} else {
			Debug.Log ("Error: Attempt to resolve engagement outside of engagement mode");
		}
	}
	void ExecuteNextAction(){
		
			//Get 1st character in queue and its target
		ScriptCharacterSheet hotSheet = activeCharacters[0].GetComponent<ScriptCharacterSheet>();
		if(hotSheet.target){
			
			hotSheet.target.GetComponent<ScriptCharacterSheet>().lastAttacker = hotSheet.gameObject;
			
			//Execute appropriate action function
			if(hotSheet.engageAtRange){
				ScriptCharacterSheet targetSheet = hotSheet.target.GetComponent<ScriptCharacterSheet>();
	
		//Start weapon effect
				hotSheet.gameObject.GetComponentInChildren<ScriptModelController>().SendMessage("WeaponEffect");
		
				scriptInterface.SendMessage("AddNewLine",hotSheet.fullName
			+ " attacks " + targetSheet.fullName + "! " + 
				hotSheet.accuracy.ToString() + " Accuracy vs. " + targetSheet.evasion.ToString() + " Evasion");
			
				//Compare attacker's Attack to target's Defense
				//bool missed;
			if(hotSheet.accuracy > targetSheet.evasion){
					//missed = false;
					targetSheet.health -= hotSheet.damage;
				
		
			
			//Log damage
				scriptInterface.SendMessage("AddNewLine",hotSheet.fullName
				+ " deals " + hotSheet.damage.ToString() + " damage to "+ targetSheet.fullName
				+ ". " + targetSheet.health.ToString() + " Health remaining.");
			
				//Launch energy ball
			//Transform projectileOrigin = hotSheet.gameObject.transform.FindChild("TraEmitter").transform;
			//GameObject hotBall = Instantiate(energyBall, projectileOrigin.position, projectileOrigin.rotation) as GameObject;
			//Rigidbody ballRigid = hotBall.GetComponent<Rigidbody>();
			//hotBall.GetComponent<Rigidbody>().AddForce(new Vector3(-2500,0,0));
			
			} else {
				scriptInterface.SendMessage("AddNewLine",hotSheet.fullName + " misses!");
				//targetReassess Tactic
				//if(hotSheet.targetReassess){...}	
			}
	
					
			//Damage display - Ideally, this section would log the actual properties of the attack, rather than the character's stats
			GameObject currentDamageDisplay = Instantiate(damageDisplay, new Vector3(targetSheet.gameObject.transform.position.x,
					targetSheet.gameObject.transform.position.y,damageDisplayDepth), Quaternion.identity) as GameObject;
				 TextMesh statusChangeText = currentDamageDisplay.GetComponentInChildren<TextMesh>();
				
				if(hotSheet.accuracy > targetSheet.evasion)
				{
					statusChangeText.text = "-" + hotSheet.damage + "HP";
				}
				else
				{
					statusChangeText.text = "Miss";
				}
			
			
			
			
			//	ExecuteRangedAttack(hotSheet);	
			} else if (hotSheet.engageInMelee){
				Debug.Log ("Melee feature pending");
				//ExecuteMeleeAttack(hotSheet);
			} else {
			scriptInterface.SendMessage("AddNewLine",hotSheet.fullName + " does zero things.");	
			}
			

		} else {
			//Character attacks nothing
			//scriptInterface.SendMessage("AddNewLine",hotSheet.fullName + " attacks... nothing.");
		}
		//Reset Wait Time to Delay
		hotSheet.waitTime = hotSheet.delay;
	}
	
	//Change cycle
	void RolloverCycle(){
		
				//Begin new Cycle
		cycle += 1;
		//Begin Command Phase
		SetToCommandMode();
		//Stop all movement
		//StartCoroutine("RedLight");
		
		//Reduce all characters' wait time to zero
		foreach(GameObject character in charactersInPlay){
			ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			hotSheet.waitTime = 0;
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
		

		activeCharacters = new List<GameObject>();
		foreach(GameObject character in charactersInPlay){
			ScriptCharacterSheet currentSheet = character.GetComponent<ScriptCharacterSheet>();
			if(currentSheet.waitTime == 0 && currentSheet.isInActingPosition){
				activeCharacters.Add (character);
			}
		}
		
	}
	void SortActiveCharacters(){
		int initialCount = activeCharacters.Count;
		List<GameObject> tempList = new List<GameObject>();
		while(tempList.Count < initialCount){	
			//Determine highest priority of remaining active characters
			float maxPriority = 0.0F;
			for(int i = 0; i < activeCharacters.Count; i++){
			float currentPriority = GetCharacterPriority(activeCharacters[i]);
			if(currentPriority > maxPriority)
				maxPriority = currentPriority;
			}	
		
			//Add highest priority character to temporary list and remove from active characters
			bool findingNextCharacter = true;
			int j = 0;
			while(findingNextCharacter){	
				if(GetCharacterPriority(activeCharacters[j]) == maxPriority){
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
	void CharacterCleanup(){
	
		for(int i = 0; i < charactersInPlay.Count; i++){
			ScriptCharacterSheet hotSheet = charactersInPlay[i].GetComponent<ScriptCharacterSheet>();
			if(hotSheet.health <= 0){
				KillCharacter(hotSheet, i);

			}
			
			//Error-checking
			if(hotSheet.waitTime < 0){
			Debug.Log ("Error 001: " + hotSheet.characterID.ToString () + "'s waitTime is " + hotSheet.waitTime);	
			}
		}
	}
	void UpdateCharacterValues(){
		
		//Update targets
		foreach(GameObject character in charactersInPlay){
			ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			
			if(hotSheet.target == null){
				//For all charactersInPlay without Targets:
				//bool assigningNewTarget = true;
				
				if(charactersInPlay.Count > 1){
				 	
				
				//Choose random character
				
				//int i = (int)Mathf.Floor(Random.value*charactersInPlay.Count);
				//while(assigningNewTarget){
				int randomCharacterIndex = (int)Mathf.Floor(Random.value*charactersInPlay.Count);
				GameObject otherCharacter = charactersInPlay[randomCharacterIndex];
				
				//If random character is not first character, assign as target (no character can be a target of him/herself
				if(otherCharacter != hotSheet.gameObject){
						hotSheet.target = otherCharacter;
						//assigningNewTarget = false;
				} else {
					//If it is first character, use next character in line
						hotSheet.target = charactersInPlay[((randomCharacterIndex+1)%(charactersInPlay.Count))];
				}
				}
			}
		}
		
		//Update isInActingPosition
		foreach(GameObject character in charactersInPlay)
		{
			ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			hotSheet.isInActingPosition = character.GetComponentInChildren<ScriptControllerTargeting>().IsInActingPosition(hotSheet);
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
			ScriptCharacterSheet selectedSheet = charactersInPlay[0].GetComponent<ScriptCharacterSheet>();
					
			string hotButton = inputButtonName;
			//playerPrompt = false;
			inputButtonName = null;
			switch(hotButton){	
				case "Melee":
					selectedSheet.engageInMelee = true;
			selectedSheet.engageAtRange = false;
					break;
				case "Ranged":
					selectedSheet.engageInMelee = false;
			selectedSheet.engageAtRange = true;
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
	
	bool GetRandomBool(){
		if(Random.value >= .5){
		return true;	
		} else {
		return false;
		}
			
	}
	
	int GetRandom1To100(){
		return (int)Mathf.Floor(Random.value*100 + 1);
	}
	
	int GetRandom1To10(){
		return (int)Mathf.Floor(Random.value*10 + 1);
	}
	
	int GetRandom1to255(){
		int hotValue = (int)Mathf.Floor (Random.value * 255 + 1);
		//Debug.Log (hotValue.ToString());
			return hotValue;
	}
	
	Color GetRandomColor(){
		//Color test = new Color(
	    return new Color(
			Random.value,
			Random.value,
			Random.value,
			255);
			
	}
	
	int GetCharacterPriority(GameObject character){
		ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
		//Start with character focus and add/ subtract due to conditions
		int total = hotSheet.focus;
		//if(hotSheet.aggressiveFire){
		//	total += aggressiveFirePriorityBonus;
		//}
		
		
		return total;
	}
	
	
	//MODE TOGGLE
	
	void SetToExecutionPhase(){
	//Debug.Log ("Execution Phase");
	    commandPhase = false;
		//movementMode = true;
		executionPhase = true;
		//engagementMode = true;
	
		
		//Adjust Stats according to Tactics
		ApplyTactics();
		
		
		//Spawn characters if necessary
		if(spawn00Time == cycle){
		RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn00)))	;
			Debug.Log ("RespawnLeft");
		} else if(spawn01Time == cycle){
		RegisterCharacter(RandomizeCharacterValues(NewCharacter(spawn01)))	;
			Debug.Log ("RespawnRight");
		} else {
			
		}
		
		//SetToEngagementMode();
		SetToMovementMode();
	}
	
	
	
	void SetToCommandMode(){
		//Debug.Log ("CommandPhase");
	engagementMode = false;
		movementMode = false;
		executionPhase = false;
		commandPhase = true;
		
		
		//Skip Command Phase
		SetToExecutionPhase();
	}
	
	void SetToMovementMode(){
		//Debug.Log ("MovementMode");
		//commandPhase = false;
		engagementMode = false;
		movementMode = true;
		foreach(GameObject character in charactersInPlay){
		ScriptCharacterMove hotScript = character.GetComponent<ScriptCharacterMove>();
			hotScript.greenLight = true;
			hotScript.startLerp = true;
		}
		
	
	}
	
	void SetToEngagementMode(){
		Debug.Log ("EngagementMode");
		//playerPrompt = false;
		movementMode = false;
		engagementMode = true;
			StartCoroutine("RedLight");
	}
	
	//Wait for every character to finish their frame of movement, then stop all characters
	IEnumerator RedLight(){
		//Debug.Log ("RedLight");
		yield return 0;
		foreach(GameObject character in charactersInPlay){
		ScriptCharacterMove hotScript = character.GetComponent<ScriptCharacterMove>();
			hotScript.greenLight = false;
		}
		if(engagementMode){
		ResolveEngagement ();
		}
		//}
		
	}
	
	//Queries
	bool MovementIsOver(){
		foreach(GameObject character in charactersInPlay){
			if(character.GetComponent<ScriptCharacterMove>().atDestination == false){
				//Debug.Log (character.GetComponent<ScriptCharacterMove>().fracJourney);
				return false;
		}
		}
		return true;
		
	}
	
	
	void ApplyTactics(){
		foreach(GameObject character in charactersInPlay){
		ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			
			
			//Apply firing mode
			if(hotSheet.aggressiveFire){
				//hotSheet.priority
			} else if(hotSheet.blindFire){
			
			} else if(hotSheet.aimedFire){
				
			}
		}
	}
	
}

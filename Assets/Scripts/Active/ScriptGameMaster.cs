using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptGameMaster : MonoBehaviour {
	

	
	/*NOTES
	 * 
	 * Players choose Tactics for multiple Cycles at a time
	 * 
	 * */
	
	//Interface
	ScriptInterface scriptInterface;
	public bool playerPrompt = false;
	public string inputButtonName = "";
	public ScriptCharacterSheet selectedSheet;
	
	//Characters
	public GameObject characterTemplate;
	public List<GameObject> charactersInPlay = new List<GameObject>();
	public int nextCharacterID = 0;
	public List<GameObject> activeCharacters = new List<GameObject>();
	
	//Space
	public Vector3 spawnPosition;
	
	//Time
	public int cycle = -1;
	
	//Physics
	ScriptPhysicsController scriptPhysicsController;
	
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
		scriptInterface = GameObject.Find ("InterfaceMain").GetComponent<ScriptInterface>();
		scriptPhysicsController = GameObject.Find ("ControllerPhysics").GetComponent<ScriptPhysicsController>();
		
		//Register each character object in the scene
		foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character")){
			
		RegisterCharacter(character);	
		}
		
		//RegisterCharacter(testCharacter);	
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		//if(!playerPrompt && Input.GetMouseButtonDown(0)){
		//	NextStep();
		//}
		
		if(Input.GetKeyDown(KeyCode.N)){
			;
			RandomizeCharacterValues(RegisterCharacter(NewCharacter()));
		}
		
		//if(Input.GetKeyDown(KeyCode.P)){
		//	CreatePlayerCharacter();	
		//}
		if(inputButtonName != ""){
			ButtonHandler();
		}
	}
	
	
	
	
	//BEGIN FUNCTIONS
	
	//Character Management
	GameObject NewCharacter(){
	//Create character object at spawn position
		return Instantiate(characterTemplate, new Vector3(spawnPosition.x + nextCharacterID * 2, spawnPosition.y, spawnPosition.z), transform.rotation) as GameObject;
	
	}
	GameObject RegisterCharacter(GameObject character){
		ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
		
		hotSheet.characterID = nextCharacterID;
		nextCharacterID += 1;
		charactersInPlay.Add (character);
		
		//Assign object character name
		character.name = hotSheet.stringID;
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
		
		//Assign Stats
		hotSheet.health = GetRandom1To100();
		hotSheet.focus = GetRandom1To100();
		hotSheet.damage = GetRandom1To100();
		hotSheet.speed = GetRandom1To100();
		hotSheet.accuracy = GetRandom1To100();
		hotSheet.evasion = GetRandom1To100()/2;
		hotSheet.armor = GetRandom1To100()/4;
		hotSheet.melee = GetRandom1To100();
		
		//Assign Tactics
		hotSheet.targetReassess = GetRandomBool();
		if(GetRandomBool()){
		hotSheet.engageAtRange = true;
			hotSheet.engageInMelee = false;
		} else {
			hotSheet.engageAtRange = false;
			hotSheet.engageInMelee = true;
		}
		
		//Assign Derived Stats
		
		hotSheet.delay = 1;
		hotSheet.priority = hotSheet.focus;
		
		
		
		return (character);
		
	}

	void KillCharacter(ScriptCharacterSheet hotSheet, int characterIndex){
						
				//Remove dead character from characters in play 
				charactersInPlay.RemoveAt(characterIndex);
				//Set character's inPlay to false
				hotSheet.inPlay = false;
				//Remove character as an active target
				foreach(GameObject character in charactersInPlay){
					ScriptCharacterSheet otherHotSheet = character.GetComponent<ScriptCharacterSheet>();
					if(otherHotSheet.target == hotSheet.gameObject){
						otherHotSheet.target = null;
					}
				}
				//Log death
				scriptInterface.SendMessage("AddNewLine", hotSheet.fullName + " dies.");
		
				//Enable ragdoll
		scriptPhysicsController.SendMessage("PropelChunk", hotSheet.gameObject);
}
	
	
	//Progress to next event
	void NextStep(){
		//if(playerSheet.inPlay){
			CharacterCleanup();
			UpdateTargets();
			//Set activeCharacters
			GetActiveCharacters();

			//If there any characters left to act for this Cycle, then execute next action
			if(activeCharacters.Count > 0){
				//Sort activeCharacters by descending Priority
				SortActiveCharacters();
				ExecuteNextAction();
				CharacterCleanup();
			} else {
				EndCycle();
			}
		//} else {
		//	scriptInterface.SendMessage("AddNewLine", "Character " + playerSheet.characterID.ToString() + " died.");
		//}
	}
	void ExecuteNextAction(){
		
			
		ScriptCharacterSheet hotSheet = activeCharacters[0].GetComponent<ScriptCharacterSheet>();
		if(hotSheet.target){
			
			//Execute appropriate action function
			if(hotSheet.engageAtRange){
				ExecuteRangedAttack(hotSheet);	
			}
			if(hotSheet.engageInMelee){
				ExecuteMeleeAttack(hotSheet);
			}
			

		} else {
			scriptInterface.SendMessage("AddNewLine",hotSheet.fullName + " attacks... nothing.");
		}
		//Reset Wait Time to Delay
		hotSheet.waitTime = hotSheet.delay;
	}
	
	//Change cycle
	void BeginCycle(){
		

		//Begin new Cycle
		cycle += 1;
		//Log new Cycle
		scriptInterface.SendMessage("AddNewLine", "Cycle " + cycle.ToString());
		
		//Begin Command Phase
		playerPrompt = true;

	}
	void EndCycle(){
		//Update characters' wait time (decrement by one)
		foreach(GameObject character in charactersInPlay){
			ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			hotSheet.waitTime -= 1;
		}
		BeginCycle();
	}
	
	//Prepare queue
	void GetActiveCharacters(){
		activeCharacters = new List<GameObject>();
		foreach(GameObject character in charactersInPlay){
			ScriptCharacterSheet currentSheet = character.GetComponent<ScriptCharacterSheet>();
			if(currentSheet.waitTime == 0 && currentSheet.isInPosition){
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
			float currentPriority = activeCharacters[i].GetComponent<ScriptCharacterSheet>().priority;
			if(currentPriority > maxPriority)
				maxPriority = currentPriority;
			}	
		
			//Add highest priority character to temporary list and remove from active characters
			bool findingNextCharacter = true;
			int j = 0;
			while(findingNextCharacter){	
				if(activeCharacters[j].GetComponent<ScriptCharacterSheet>().priority == maxPriority){
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
	void UpdateTargets(){
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
						
	}
	
	void ExecuteRangedAttack(ScriptCharacterSheet hotSheet){
		ScriptCharacterSheet targetSheet = hotSheet.target.GetComponent<ScriptCharacterSheet>();
	
				scriptInterface.SendMessage("AddNewLine",hotSheet.fullName
			+ " attacks " + targetSheet.fullName + "! " + 
				hotSheet.accuracy.ToString() + " Accuracy vs. " + targetSheet.evasion.ToString() + " Evasion");
			//Compare attacker's Accuracy to target's Defense
			if(hotSheet.accuracy > targetSheet.evasion){
				targetSheet.health -= hotSheet.damage;
				scriptInterface.SendMessage("AddNewLine",hotSheet.fullName
				+ " deals " + hotSheet.damage.ToString() + " damage to "+ targetSheet.fullName
				+ ". " + targetSheet.health.ToString() + " Health remaining.");
			} else {
				scriptInterface.SendMessage("AddNewLine",hotSheet.fullName + " misses!");
				//targetReassess Tactic
				//if(hotSheet.targetReassess){...}
				
			}
	}
	
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
					NextStep();
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
	
	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptGameMaster : MonoBehaviour {
	

	
	/*NOTES
	 * 
	 * Players choose Tactics for multiple Cycles at a time
	 * 
	 * */
	
	//Player
	public bool playerPrompt = false;
	public string inputButtonName = null;
	public GameObject playerControlledCharacter = null;
	public ScriptCharacterSheet playerSheet = null;
	
	//Characters
	public GameObject characterTemplate = null;
	public List<GameObject> charactersInPlay = new List<GameObject>();
	public int nextCharacterID = 0;
	public List<GameObject> activeCharacters = new List<GameObject>();
	
	//Time
	public int cycle = -1;
	
	//Local variables
	//Resolve Action Function
	//public List<float> priority = new List<float>();
	//public List<float> characterPriority = new List<float>();
	
	//Scripts
	public ScriptInterface scriptInterface;
	
	//Strings
	public List<string> firstNames = new List<string>(new string[] {"Jumbo", "Ham", "Tassik", 
		"Marinn", "Rose", "Joseph", "Dash", "Jaedon", "Argot", "Tau", "Rachel", "Julien", "Lily", "Larry", 
		"Maynard", "Leo", "Ota", "Gulliver", "Megan", "Freck", "Korder", "Lincoln"});
	public List<string> lastNames = new List<string>(new string[] {"Baloney", "Jehosephat", "Kayla", 
		"Dillon", "Reynolds", "Wild", "Rendar", "Casio", "Veis", "Ceti", "Vega", "Pavec", "Puncture", 
		"Jello", "Thatcher", "Marshall", "Stockholm", "Retri", "Freck", "Korder", "Lincoln"});
	
	// Use this for initialization
	void Start () {
		
		scriptInterface = GetComponentInChildren<ScriptInterface>();
		//Spawn characters
		//...
		
		//Register characters in play by assigning ID and adding to list
		/*
		foreach (GameObject character in GameObject.FindGameObjectsWithTag("Character")){
			ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
			if(hotSheet.inPlay){
				hotSheet.characterID = nextCharacterID;
				nextCharacterID += 1;
				charactersInPlay.Add (character);
			}
		}
		
		//Set character with ID 0 to player-controlled
		for(int i = 0; i < charactersInPlay.Count; i++){
			ScriptCharacterSheet hotSheet = charactersInPlay[i].GetComponent<ScriptCharacterSheet>();
			if(hotSheet.characterID == 0){
				playerControlledCharacter = charactersInPlay[i];
				playerSheet = hotSheet;
				playerSheet.allegiance = "Player";
			}
		}*/
		
		//Begin Cycle 0
		//BeginCycle();
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!playerPrompt && Input.GetMouseButtonDown(0)){
			NextStep();
		}
		
		if(Input.GetKeyDown(KeyCode.N)){
			CreateNPC();	
		}
		
		if(Input.GetKeyDown(KeyCode.P)){
			CreatePlayerCharacter();	
		}
		
		
		if(inputButtonName != null){
			string hotButton = inputButtonName;
			playerPrompt = false;
			inputButtonName = null;
			switch(hotButton){	
				case "Fight":
					//Fight
					break;
				case "Flight":
					//Run away
					break;
			}	
		}
	}
	
	
	
	
	//BEGIN FUNCTIONS
	
	//New Character
	void CreatePlayerCharacter(){
		playerControlledCharacter = NewRandomCharacter();
		playerSheet = playerControlledCharacter.GetComponent<ScriptCharacterSheet>();
		playerSheet.control = "P1";
	}
	
	void CreateNPC(){
		NewRandomCharacter().GetComponent<ScriptCharacterSheet>().control = "AI";
	}
	
	GameObject NewRandomCharacter(){
		GameObject hotChar = Instantiate(characterTemplate) as GameObject;
		ScriptCharacterSheet hotSheet = hotChar.GetComponent<ScriptCharacterSheet>();
		
		//Register character
		hotSheet.characterID = nextCharacterID;
		nextCharacterID += 1;
		charactersInPlay.Add (hotChar);
		
		hotSheet.firstName = firstNames[(int)Mathf.Floor(Random.value*firstNames.Count)];
		hotSheet.lastName = lastNames[(int)Mathf.Floor(Random.value*lastNames.Count)];
		hotSheet.fullName = hotSheet.firstName+ " " + hotSheet.lastName;
		hotSheet.name = hotSheet.characterID.ToString()+hotSheet.firstName+hotSheet.lastName;
		
		hotSheet.health = (int)Mathf.Floor(Random.value*100 + 1);
		hotSheet.focus = (int)Mathf.Floor(Random.value*100 + 1);
		hotSheet.damage = (int)Mathf.Floor(Random.value*100 + 1);
		hotSheet.speed = (int)Mathf.Floor(Random.value*100 + 1);
		hotSheet.accuracy = (int)Mathf.Floor(Random.value*100 + 1);
		hotSheet.evasion = (int)Mathf.Floor((Random.value*100 + 1)/2);
		hotSheet.armor = (int)Mathf.Floor(Random.value*100 + 1);
		hotSheet.priority = (int)Mathf.Floor(Random.value*100 + 1);
		hotSheet.melee = (int)Mathf.Floor(Random.value*100 + 1);
		hotSheet.delay = 1;
		
		return (hotChar);
		
		
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

			//Determine if attack hits and deal damage; log results to console
			ScriptCharacterSheet targetSheet = hotSheet.target.GetComponent<ScriptCharacterSheet>();
			//Log attack
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
				//Remove dead character from characters in play 
				charactersInPlay.RemoveAt(i);
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
						hotSheet.target = charactersInPlay[(randomCharacterIndex+1)%(charactersInPlay.Count-1)];
					
				}
				//}
			}
		}
						
	}
	
	
}

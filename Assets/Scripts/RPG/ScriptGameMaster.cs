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
	public List<GameObject> charactersInPlay = new List<GameObject>();
	public int nextCharacterID = 0;
	public List<GameObject> activeCharacters = new List<GameObject>();
	
	//Time
	public int cycle = -1;
	
	//Local variables
	//Resolve Action Function
	public List<float> priority = new List<float>();
	public List<float> characterPriority = new List<float>();
	
	//Scripts
	public ScriptInterface scriptInterface;
	
	// Use this for initialization
	void Start () {
		
		scriptInterface = GetComponentInChildren<ScriptInterface>();
		//Spawn characters
		//...
		
		//Register characters in play by assigning ID and adding to list
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
		}
		
		//Begin Cycle 0
		//BeginCycle();
		
	}
	
	// Update is called once per frame
	void Update () {
			if(!playerPrompt && Input.GetMouseButtonDown(0)){
		NextStep();
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
	
	void NextStep(){
		if(playerSheet.inPlay){
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
		} else {
			scriptInterface.SendMessage("AddNewLine", "Character " + playerSheet.characterID.ToString() + " died.");
		}
	}
	
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
	

	
	
	
	
	void ExecuteNextAction(){
		
			
		ScriptCharacterSheet hotSheet = activeCharacters[0].GetComponent<ScriptCharacterSheet>();
		if(hotSheet.target){

			//Determine if attack hits and deal damage; log results to console
			ScriptCharacterSheet targetSheet = hotSheet.target.GetComponent<ScriptCharacterSheet>();
			//Log attack
			scriptInterface.SendMessage("AddNewLine","Character " + hotSheet.characterID.ToString()
			+ " attacks Character " + targetSheet.characterID.ToString () + "!");
			//Compare attacker's Accuracy to target's Defense
			if(hotSheet.accuracy > targetSheet.evasion){
				targetSheet.health -= hotSheet.damage;
				scriptInterface.SendMessage("AddNewLine","Character " + hotSheet.characterID.ToString()
				+ " deals " + hotSheet.damage.ToString () + " damage to Character " + targetSheet.characterID.ToString ());
			} else {
				scriptInterface.SendMessage("AddNewLine","Character " + hotSheet.characterID.ToString() + " misses!");
			}
		} else {
			scriptInterface.SendMessage("AddNewLine","Character " + hotSheet.characterID.ToString () + " attacks... nothing.");
		}
		//Reset Wait Time to Delay
		hotSheet.waitTime = hotSheet.delay;
	}
	
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
				scriptInterface.SendMessage("AddNewLine", "Character " + hotSheet.characterID.ToString() + " dies.");
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
				bool assigningNewTarget = true;
				int i = 0;
				while(assigningNewTarget){
					GameObject otherCharacter = charactersInPlay[i];
					if(otherCharacter != hotSheet.gameObject){
						hotSheet.target = otherCharacter;
						assigningNewTarget = false;
					} else {
						i++;
						if(i >= charactersInPlay.Count){
							assigningNewTarget = false;
						}
					}
				}
			}
		}
						
	}
	
	
}

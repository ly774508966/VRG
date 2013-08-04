using UnityEngine;
using System.Collections;

public class ScriptTargetDisplayPanel : MonoBehaviour {
	
	public GameObject currentTarget = null;
	public ScriptGameMaster scriptGameMaster;
	public ScriptTargetNameDisplay scriptTargetNameDisplay;
	//public ScriptTargetInfo scriptTargetInfo;
	
	// Use this for initialization
	void Start () {
		scriptTargetNameDisplay = transform.FindChild("TargetNameDisplay").GetComponent<ScriptTargetNameDisplay>();
		//scriptTargetInfo = transform.FindChild("TargetInfoDisplay").GetComponent<ScriptTargetInfo>();
	}

	
	// Update is called once per frame
	void Update () {
	
	}
	
	void SetNextTarget(){
		if(scriptGameMaster.charactersInPlay.Count > 1){
			ScriptCharacterSheet selectedSheet = scriptGameMaster.selectedSheet;
			if(scriptGameMaster.selectedSheet.target == null){
				selectedSheet.target = scriptGameMaster.charactersInPlay[1]; 
			} else {		
				int nextNumber = selectedSheet.target.GetComponent<ScriptCharacterSheet>().characterID + 1;
				int nextTargetID = nextNumber % scriptGameMaster.charactersInPlay.Count;
				bool searchingForTarget = true;
				while(searchingForTarget){
					foreach(GameObject character in scriptGameMaster.charactersInPlay){
						ScriptCharacterSheet hotSheet = character.GetComponent<ScriptCharacterSheet>();
						if(hotSheet.characterID == nextTargetID){
							if(hotSheet.characterID == scriptGameMaster.selectedSheet.characterID){
								nextTargetID = (nextTargetID + 1) % scriptGameMaster.charactersInPlay.Count;
							} else {	
								//Break while loop
								searchingForTarget = false;
								//Assign temporary character as current target
								selectedSheet.target = hotSheet.gameObject;	
								scriptTargetNameDisplay.guiText.text = GetCharacterInfo(hotSheet);
									
							}
						break;
						}	
					}
				}				
			}
		}	
	}
		string GetCharacterInfo(ScriptCharacterSheet hotSheet){
			return hotSheet.characterID.ToString() + " " + hotSheet.firstName + " " + hotSheet.lastName + 
			"\n Health " + hotSheet.health.ToString() +
			"\n Priority " + hotSheet.priority.ToString() +
			"\n Aim " + hotSheet.accuracy.ToString() +
			"\n Melee " + hotSheet.melee.ToString() +
			"\n Defense " + hotSheet.evasion.ToString() +
			"\n Damage " + hotSheet.damage.ToString();
								
	}

}
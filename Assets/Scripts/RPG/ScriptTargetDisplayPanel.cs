using UnityEngine;
using System.Collections;

public class ScriptTargetDisplayPanel : MonoBehaviour {
	
	public GameObject currentTarget = null;
	public ScriptGameMaster scriptGameMaster;
	public ScriptTargetNameDisplay scriptTargetNameDisplay;
	
	// Use this for initialization
	void Start () {
	scriptTargetNameDisplay = transform.FindChild("TargetNameDisplay").GetComponent<ScriptTargetNameDisplay>();
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
								selectedSheet.target = hotSheet.gameObject;	
								scriptTargetNameDisplay.guiText.text = hotSheet.characterID.ToString() + " " + hotSheet.firstName + " " + hotSheet.lastName;
								searchingForTarget = false;
							}
						break;
						}	
					}
				}				
			}
		}	
	}


}
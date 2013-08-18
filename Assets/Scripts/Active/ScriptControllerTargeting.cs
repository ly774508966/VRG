using UnityEngine;
using System.Collections;

public class ScriptControllerTargeting : MonoBehaviour {
	
	ScriptCharacterSheet scriptCharacterSheet;
	ScriptGameMaster scriptGameMaster;
	public Vector3 rangedAttack;
	public GameObject characterSource;
	public GameObject characterDestination;

	
	// Use this for initialization
	void Start () {
	scriptCharacterSheet = transform.parent.GetComponent<ScriptCharacterSheet>();
	scriptGameMaster = GameObject.Find ("ControllerGame").GetComponent<ScriptGameMaster>();
	
	
	
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(scriptCharacterSheet.target && scriptGameMaster.movementMode && scriptCharacterSheet.inPlay){
				rangedAttack = scriptCharacterSheet.target.transform.position - transform.position;
			if(scriptCharacterSheet.weaponRange >= rangedAttack.magnitude){
				scriptCharacterSheet.isInPosition = true;
				if(!scriptGameMaster.engagementMode && scriptCharacterSheet.waitTime == 0){
					scriptGameMaster.SendMessage("SetToEngagementMode");
				} else {
					scriptCharacterSheet.isInPosition = false;
					
				}
			}
		}
	}
	
	//Vector3 RangedAttack (Vector3 destination, Vector3 source){
	//return destination - source;	
	
}

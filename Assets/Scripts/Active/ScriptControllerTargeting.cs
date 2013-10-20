using UnityEngine;
using System.Collections;

public class ScriptControllerTargeting : MonoBehaviour {
	
	//ScriptCharacterSheet scriptCharacterSheet;
	//ScriptGameMaster scriptGameMaster;
	//public Vector3 rangedAttack;
	public GameObject characterSource;
	public GameObject characterDestination;
	public Vector3 rangedAttack;
	public float weaponRangeOffset = -1F;

	
	// Use this for initialization
	void Start () {
	//scriptCharacterSheet = transform.parent.GetComponent<ScriptCharacterSheet>();
	//scriptGameMaster = GameObject.Find ("ControllerGame").GetComponent<ScriptGameMaster>();
	
	
	
	
	}
	
	// Update is called once per frame
	void Update () {
	

		
				//if(!scriptGameMaster.engagementMode && scriptCharacterSheet.waitTime == 0){
				//	scriptGameMaster.SendMessage("SetToEngagementMode");
				//} else {
					//Debug.Log ("OutOfPosition");
					//scriptCharacterSheet.isInPosition = false;
					
				//}
			
		}
	
	//Vector3 RangedAttack (Vector3 destination, Vector3 source){
	//return destination - source;	
	
	
	public bool IsInActingPosition(ScriptCharacterSheet hotSheet)
	{
			//If character is in play and has a target,
		if(hotSheet.target && hotSheet.inPlay)
		{	
			 //calculate attack vector
			rangedAttack = hotSheet.target.transform.position - transform.position;
			if(hotSheet.maxRange >= rangedAttack.magnitude + weaponRangeOffset)
			{
				return true;
			} 
			else
			{
				return false;
			}
		}
		else 
		{
			//Debug.Log ("Invalid Query");
			return false;
		}
	}
}

using UnityEngine;
using System.Collections;



public class ScriptPhysicsController : MonoBehaviour {
	
	public float wallJointStrength = 1.0F;
	public float wallThresholdVelocity = 1.0F;
	
	public float propelForce = 10000F;
	
	// Use this for initialization
	void Start () {
	//foreach(GameObject wall in GameObject.Find ("ObjectBreakableWall")){
	RegisterWallPanels(GameObject.Find ("ObjectBreakableWall"));
	//GameObject poorBastard = GameObject.Find ("ObjectCharacterModel");
	//PropelChunk(poorBastard, propelForce);
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	

	
	void RegisterWallPanels(GameObject wallSegment){
		foreach(Transform child in wallSegment.transform){
			if(child.rigidbody == null){
				//Debug.Log ("Run");
				RegisterWallPanels(child.gameObject);
			} else {
				GameObject hotPanel = child.gameObject;
				//Add panel scripts
				hotPanel.AddComponent("ScriptWallPanel");
				hotPanel.GetComponent<ScriptWallPanel>().wallImpactThreshold = wallThresholdVelocity;
				hotPanel.AddComponent("FixedJoint");
		
		FixedJoint hotJoint = hotPanel.GetComponent<FixedJoint>();
				hotJoint.breakForce = wallJointStrength; 
				hotJoint.rigidbody.isKinematic = true;
			}
		}
	}
	
	void Ragdollify(GameObject character){
		
		foreach(Transform child in character.transform){
		if(child.rigidbody != null){
			child.rigidbody.isKinematic = false;
			child.rigidbody.WakeUp();
			} else {
			Ragdollify(child.gameObject);
			}
		}
	}
	
	void Propel(Vector3 propelDirection, GameObject targetCharacter){
		Debug.Log ("well???");
		foreach(Transform child in targetCharacter.transform){
			if(child.rigidbody != null){
				child.rigidbody.AddForce( propelDirection * propelForce);	
				} else {
			Propel(propelDirection, child.gameObject);
			}
		}
	}
		
	
	void ExecuteCharacter(GameObject targetCharacter){
		//Debug.Log (targetCharacter.GetComponent<ScriptCharacterSheet>().stringID);
		Ragdollify(targetCharacter);
		//GameObject lastAttacker = targetCharacter.GetComponent<ScriptCharacterSheet>().lastAttacker ;
		//ScriptControllerTargeting hotCont = lastAttacker.GetComponentInChildren<ScriptControllerTargeting>(); 
		
				
		Vector3 rangedAttack = targetCharacter.transform.position - 
			targetCharacter.GetComponent<ScriptCharacterSheet>().lastAttacker.
				GetComponentInChildren<ScriptControllerTargeting>().transform.position;
		Propel (rangedAttack, targetCharacter);
		
		
	}
	
}

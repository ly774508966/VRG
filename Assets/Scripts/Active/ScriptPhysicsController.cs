using UnityEngine;
using System.Collections;



public class ScriptPhysicsController : MonoBehaviour {
	
	public float wallJointStrength = 1.0F;
	public float wallThresholdVelocity = 1.0F;
	public float headExplodeForce = 1000;
	public float propelForce = 10000F;
	
	
	//Local variables
	public bool propel;
	public bool blowUpHead;
	
	
	
	//public GameObject testHead;
	
	// Use this for initialization
	void Start () {
		
	//foreach(GameObject wall in GameObject.Find ("ObjectBreakableWall")){
	RegisterAllPanels(GameObject.Find ("ContainerPanel"));
	//GameObject poorBastard = GameObject.Find ("ObjectCharacterModel");
	//PropelChunk(poorBastard, propelForce);
	
	}
	
	// Update is called once per frame
	void Update () {
	}
	

	
	void RegisterAllPanels(GameObject wallSegment){
		foreach(Transform child in wallSegment.transform){
			if(child.rigidbody == null){
				//Debug.Log ("Run");
				RegisterAllPanels(child.gameObject);
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
	
		void Unragdollify(GameObject chunk){
		
		foreach(Transform child in chunk.transform){
		if(child.rigidbody != null){
			child.rigidbody.isKinematic = true;
			} else {
			Unragdollify(child.gameObject);
			}
		}
	}
	
	
	void Ragdollify(GameObject chunk){
		
		foreach(Transform child in chunk.transform){
		if(child.rigidbody != null){
			//	Debug.Log (child);
			child.rigidbody.isKinematic = false;
			child.rigidbody.WakeUp();
			} else {
			Ragdollify(child.gameObject);
			}
		}
	}
	
	void Propel(Vector3 propelDirection, GameObject targetCharacter){
		foreach(Transform child in targetCharacter.transform){
			if(child.rigidbody != null){
				child.rigidbody.AddForce( propelDirection * propelForce);	
				} else {
			Propel(propelDirection, child.gameObject);
			}
		}
	}
		
	
	void ExecuteCharacter(GameObject targetCharacter){
		Ragdollify(targetCharacter);
		//GameObject lastAttacker = targetCharacter.GetComponent<ScriptCharacterSheet>().lastAttacker ;
		//ScriptControllerTargeting hotCont = lastAttacker.GetComponentInChildren<ScriptControllerTargeting>(); 
		
		if(propel){
		Vector3 AttackDirection = (targetCharacter.transform.position - 
			targetCharacter.GetComponent<ScriptCharacterSheet>().lastAttacker.
				GetComponentInChildren<ScriptControllerTargeting>().transform.position);
		AttackDirection.Normalize();
		Propel (AttackDirection, targetCharacter);
		}
		if(blowUpHead){
		GameObject targetHead = targetCharacter.transform.Find("ObjectCharacterModel/head").gameObject;
		//Debug.Log (targetHead);
		targetHead.SendMessage("HeadExplode", headExplodeForce);
		//testHead.SendMessage("HeadExplode", 1000);
	
		}
	}
	/*
	IEnumerator KillCam(){
		yield return new WaitForSeconds(0.1);
		
		Debug.Break();
	}
	*/
}

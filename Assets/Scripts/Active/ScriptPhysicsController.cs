using UnityEngine;
using System.Collections;



public class ScriptPhysicsController : MonoBehaviour {
	
	public float wallJointStrength = 1.0F;
	public float wallThresholdVelocity = 1.0F;
	
	public Vector3 propelForce = new Vector3(0, 0, -1000F);
	
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
	
	void PropelChunk(GameObject character){
		
		foreach(Transform child in character.transform){
		if(child.rigidbody != null){
			child.rigidbody.isKinematic = false;
			child.rigidbody.AddForce(propelForce);	
			} else {
			PropelChunk(child.gameObject);
			}
		}
	}
	
}

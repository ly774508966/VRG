using UnityEngine;
using System.Collections;

public class ScriptModelHead : MonoBehaviour {
	
	public GameObject brainBox;
	
	// Use this for initialization
	void Start () {
		
		brainBox = transform.FindChild("headBox14").gameObject;
	
		foreach(Transform child in transform){
			
			Rigidbody boxRigid = child.gameObject.GetComponent<Rigidbody>();
			
			foreach(Transform otherChild in transform){
			//if(((otherChild.transform.position - child.transform.position).magnitude) <= 0.1F && otherChild != child){
			FixedJoint hotJoint = boxRigid.gameObject.AddComponent<FixedJoint>();
					hotJoint.connectedBody = otherChild.gameObject.GetComponent<Rigidbody>();
			//	}
			}
		//boxRigid.isKinematic = false;	
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void HeadExplode (float explosiveForce){
		foreach(Transform child in transform){
			foreach(FixedJoint joint in child.GetComponents<FixedJoint>()){
				Destroy(joint);
			}
			
			Rigidbody boxRigid = child.GetComponent<Rigidbody>();
			boxRigid.AddExplosionForce(explosiveForce, brainBox.transform.position, 5.0F);
			
			
		}
	}
	
	
}

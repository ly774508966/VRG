using UnityEngine;
using System.Collections;

public class ScriptModelHead : MonoBehaviour {
	
	public GameObject impactBox;
	public AudioSource audioSource;
	public int impactBoxNumber = 14;
	
	// Use this for initialization
	void Start () {
		
		impactBox = transform.FindChild("headBox" + impactBoxNumber.ToString()).gameObject;
		audioSource = gameObject.GetComponent<AudioSource>();
		
		
		
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
			boxRigid.WakeUp();
			boxRigid.AddExplosionForce(explosiveForce, impactBox.transform.position, 5.0F);
			audioSource.Play ();
			
			
		}
	}
	
	
}

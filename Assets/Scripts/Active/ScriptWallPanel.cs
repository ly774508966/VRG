using UnityEngine;
using System.Collections;

public class ScriptWallPanel : MonoBehaviour {
	
	public float wallImpactThreshold; 
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnCollisionEnter(Collision hit){
	if(hit.gameObject.rigidbody.velocity.magnitude > wallImpactThreshold && hit.gameObject.layer == 21){
		rigidbody.isKinematic = false;	
		}
	}
	
}
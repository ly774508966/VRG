using UnityEngine;
using System.Collections;

public class CoordinationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
	
	void ActionRequest(GameObject character){
	Debug.Log (Time.frameCount + "ActionRequest" + character);
	}
}

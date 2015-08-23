using UnityEngine;
using System.Collections;

public class ScriptTargetNameDisplay : MonoBehaviour {
	
	public bool isClicked = false;
	
	//public ScriptGameMaster scriptGameMaster;
	
	// Use this for initialization
	void Start () {
		GetComponent<GUIText>().text = "Select Target";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	/*
	void OnMouseDown(){
		isClicked = true;	
	}
	
	void OnMouseUp(){
		if(isClicked){
			//Once button is clicked
			SendMessageUpwards("SetNextTarget");
		}
	}
	*/	
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptInterface : MonoBehaviour {
	
	
	public GameObject console;
	
	//public bool testSwitch = true;
	
	
	// Use this for initialization
	void Start () {
	console = transform.FindChild("Console").gameObject;	
	}
	
	// Update is called once per frame
	void Update () {


		
	}
	
	
	void AddNewLine(string textLine){
	
		console.SendMessage("AddNewLineConsole", textLine);
		
	}
	
	
	
}

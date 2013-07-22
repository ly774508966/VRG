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
		
		//	for(int i = 0; i < consoleText.Count; i++){
	//	consoleText[i].transform.Translate(0, scrollSpeed, 0);
	//	}
		
	//	consoleText.Insert (0, Instantiate(textBlock,transform.position,transform.rotation) as GameObject);
	//	consoleText[0].transform.parent = transform.FindChild("Console");
	//	consoleText[0].guiText.text = textLine;
		
	}
	
	
}

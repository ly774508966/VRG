using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptConsole : MonoBehaviour {
	
	public List<GameObject> consoleText = new List<GameObject>();
	public GameObject textBlock;

	public float lineSpacing = 0.05F;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AddNewLineConsole(string textLine){
		for(int i = 0; i < consoleText.Count; i++){
		consoleText[i].transform.position += new Vector3(0.0F, lineSpacing, 0.0F);
		//		Translate(0, scrollSpeed, 0);
		}
	
		consoleText.Insert (0, Instantiate(textBlock,transform.position,transform.rotation) as GameObject);
		consoleText[0].transform.parent = gameObject.transform;
		consoleText[0].guiText.text = textLine;
		
	}

}

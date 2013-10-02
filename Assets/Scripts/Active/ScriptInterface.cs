using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptInterface : MonoBehaviour {
	
	public GameObject selectedCharacterInfoDisplay;
	public GameObject opposingCharacterInfoDisplay;
	public GameObject console;
	
	ScriptGameMaster scriptGameMaster;
	
	GUIStyle defaultStyle;
	
	//public bool testSwitch = true;
	
	
	// Use this for initialization
	void Start () {
		scriptGameMaster = GameObject.Find ("ControllerGame").GetComponent<ScriptGameMaster>();
	//console = transform.FindChild("Console").gameObject;	
	}
	
	// Update is called once per frame
	void Update () {
		
		selectedCharacterInfoDisplay.guiText.text = GetCharacterInfo(scriptGameMaster.selectedSheet);
		opposingCharacterInfoDisplay.guiText.text = GetCharacterInfo(scriptGameMaster.opposingSheet);

		
	}
	
	
	void AddNewLine(string textLine){
	
		console.SendMessage("AddNewLineConsole", textLine);
		
	}
	
	string GetCharacterInfo(ScriptCharacterSheet hotSheet){
		return hotSheet.stringID + 
			"\n HP " + hotSheet.health.ToString() +
			"\n Priority " + hotSheet.focus.ToString() +
			"\n Attack " + hotSheet.accuracy.ToString() +
			//"\n Melee " + hotSheet.melee.ToString() +
			"\n Defense " + hotSheet.evasion.ToString() +
			"\n Damage " + hotSheet.damage.ToString() +
			"\n Range " + hotSheet.weaponRange.ToString() +
			"\n Cooldown " + hotSheet.weaponCooldown.ToString();
	}
	
}

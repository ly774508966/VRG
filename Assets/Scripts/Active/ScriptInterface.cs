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
			"\n HP " + hotSheet.meat.ToString() +
			"\n Priority " + hotSheet.nerve.ToString() +
			"\n Attack " + hotSheet.baseAttack.ToString() +
			//"\n Melee " + hotSheet.melee.ToString() +
			"\n Defense " + hotSheet.baseDefense.ToString() +
			"\n Muscle " + hotSheet.baseMuscle.ToString() +
			"\n Range " + hotSheet.readyRange.ToString() +
			//"\n Cooldown " + hotSheet.weaponCooldown.ToString() +
			"\n Hit % " + hotSheet.currentHitChance.ToString();
	}
	
	
}

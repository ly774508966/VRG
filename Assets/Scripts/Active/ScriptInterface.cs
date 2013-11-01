using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptInterface : MonoBehaviour {
	
	public GameObject selectedCharacterComparisonDisplay;
	public GameObject opposingCharacterComparisonDisplay;
	public GameObject selectedCharacterSheetDisplay;
	public GameObject opposingCharacterSheetDisplay;
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
		
		selectedCharacterComparisonDisplay.guiText.text = GetStandardCharacterComparison(scriptGameMaster.selectedSheet);
		opposingCharacterComparisonDisplay.guiText.text = GetMirroredCharacterComparison(scriptGameMaster.opposingSheet);
		selectedCharacterSheetDisplay.guiText.text = GetCharacterSheet(scriptGameMaster.selectedSheet);
		opposingCharacterSheetDisplay.guiText.text = GetCharacterSheet(scriptGameMaster.opposingSheet);
		
	}
	
	
	void AddNewLine(string textLine){
	
		console.SendMessage("AddNewLineConsole", textLine);
		
	}
	
	string GetStandardCharacterComparison(ScriptCharacterSheet hotSheet){
		return 
			//hotSheet.stringID + 
			"\n HP " + hotSheet.baseToughness.ToString() +
			"\n Priority " + hotSheet.readyPriority.ToString() +
			"\n Attack " + hotSheet.readyAttack.ToString() +
			//"\n Melee " + hotSheet.melee.ToString() +
			"\n Defense " + hotSheet.readyDefense.ToString() +
			"\n Muscle " + hotSheet.baseMuscle.ToString() +
			"\n Range " + hotSheet.readyRange.ToString() +
			"\n Damage " + hotSheet.readyDamage.ToString() +
			//"\n Cooldown " + hotSheet.weaponCooldown.ToString() +
			"\n Hit % " + hotSheet.currentHitChance.ToString();
			//"\n Item: " + hotSheet.activeItem.fullName;
	}
	
		string GetMirroredCharacterComparison(ScriptCharacterSheet hotSheet){
		return 
			//hotSheet.stringID + 
			"\n" + hotSheet.baseToughness.ToString() + " HP" +
			"\n" + hotSheet.readyPriority.ToString() + " Priority" +
			"\n" + hotSheet.readyAttack.ToString() + " Attack" + 
			"\n" + hotSheet.readyDefense.ToString() +" Defense" + 
			"\n" + hotSheet.baseMuscle.ToString() + " Muscle" + 
			"\n" + hotSheet.readyRange.ToString() +  " Range"  +
			"\n" + hotSheet.readyDamage.ToString() + " Damage" + 
			"\n" + hotSheet.currentHitChance.ToString() + " Hit %";
	}
	string GetCharacterSheet(ScriptCharacterSheet hotSheet)
	{
		return hotSheet.stringID +
			"\n Item: " + hotSheet.activeItem.fullName;
	}
	
}

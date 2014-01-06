using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum DesireColor
{
	Red,
	Orange,
	Yellow,
	Green,
	Cyan,
	Blue,
	Magenta
}

public class ScriptControllerEncounter : MonoBehaviour {

	//Scripts
	public ScriptDatabase scriptDatabase;

	//States
	bool resultRevealed = false;
	public int currentEncounter = 1;

	//Dervied strings
	public string headerString;

	//Desire Colors
	public List<DesireColor> answerColors = new List<DesireColor>(){DesireColor.Red, DesireColor.Orange,
		DesireColor.Yellow, DesireColor.Green, DesireColor.Cyan, DesireColor.Blue, DesireColor.Magenta};

	//Answers
	//Desire string dictionary template
	public Dictionary<DesireColor, string> emptyAnswerDictionary = new Dictionary<DesireColor, string>()
	{
		{DesireColor.Red, null},
		{DesireColor.Orange, null},
		{DesireColor.Yellow, null},
		{DesireColor.Green, null},
		{DesireColor.Cyan, null},
		{DesireColor.Blue, null},
		{DesireColor.Magenta, null}
	};

	public Dictionary<DesireColor, string> activeAnswers = new Dictionary<DesireColor, string>();

	//Input
	DesireColor selectedColor;

	//Styles
	//GUIStyle defaultStyle;
	//GUIStyle cardStyle;
	public GUISkin cardSkin;




	public List<string> encounterTitles = new List<string>
	{
		"The Pharmacy", //000
		"McGodagle's Restaurant" //001
	};

	public List<string> encounterPrompts = new List<string>
	{
		"Care to self medicate?", //000
		"What'll it be?" //001
	};

	//public List<Dictionary<DesireColor, string>> encounterAnswers = new List<Dictionary<DesireColor, string>>();

	// Use this for initialization
	void Start () {

		//Acquire database
		scriptDatabase = GameObject.Find ("ControllerGame").GetComponent<ScriptDatabase>();

		//Answer initialization belongs in other function
		activeAnswers = LoadAnswers(currentEncounter);
		answerColors = ShuffleColorList(answerColors);

}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		//defaultStyle = new GUIStyle(GUI.skin.box);
		//cardStyle = defaultStyle;

		GUI.Box (new Rect(10, 10, 450, 370), "");

		GUI.Label (new Rect(20, 20, 400, 30), "<b>" + encounterTitles[currentEncounter] + "</b>");

		GUI.Label (new Rect(20, 50, 400, 30), encounterPrompts[currentEncounter]);

		if(GUI.Button(new Rect(30, 080, 400, 30), activeAnswers[answerColors[0] ]))
		{
			resultRevealed = true;
			selectedColor = answerColors[0];
		}
		if(GUI.Button(new Rect(30, 120, 400, 30), activeAnswers[answerColors[1] ])){
			resultRevealed = true;
			selectedColor = answerColors[1];
		}
		if(GUI.Button(new Rect(30, 160, 400, 30),activeAnswers[answerColors[2] ])){
			resultRevealed = true;
			selectedColor = answerColors[2];
		}
		if(GUI.Button(new Rect(30, 200, 400, 30), activeAnswers[answerColors[3] ])){
			resultRevealed = true;
			selectedColor = answerColors[3];
			}
		if(GUI.Button(new Rect(30, 240, 400, 30), activeAnswers[answerColors[4] ])){
			resultRevealed = true;
			selectedColor = answerColors[4];
		}
		if(GUI.Button(new Rect(30, 280, 400, 30), activeAnswers[answerColors[5]])){
			resultRevealed = true;
			selectedColor = answerColors[5];
		}
		if(GUI.Button(new Rect(30, 320, 400, 30), activeAnswers[answerColors[6]])){
			resultRevealed = true;
			selectedColor = answerColors[6];
		}

		if(resultRevealed)
		{
			GUIStyle cardStyle = new GUIStyle();
			cardStyle.normal.textColor = GetDesireRGBColor(selectedColor);
			GUI.Label (new Rect(20, 400, 250, 30), "You received a " + selectedColor.ToString().ToLower() + " card.", cardStyle);

			//guiSkin.box.
			//GUI.Box (new Rect(300, 400, 20, 40), "", cardSkin.GetStyle(");
				}
	
	}

	List<DesireColor> ShuffleColorList(List<DesireColor> listArg)
	{
		List<DesireColor> outputList = new List<DesireColor>();
		int inputListCount = listArg.Count;

		for (int i = 0; i < inputListCount; i++) 
		{
			//Debug.Log (i.ToString ());
			int index =	(int)Mathf.Floor (Random.value * listArg.Count);
			//Debug.Log ("List count is " + listArg.Count.ToString() + "; index is " + index.ToString ());
			outputList.Add (listArg[index]);
			listArg.RemoveAt(index);
				}

		return outputList;
		}

	Color GetDesireRGBColor(DesireColor desireColor)
	{
		switch(desireColor)
		{
		case DesireColor.Red:
			return Color.red;
		case DesireColor.Orange:
			return new Color32(255, 150, 0, 255);
		case DesireColor.Yellow:
			return Color.yellow;
		case DesireColor.Green:
			return Color.green;
		case DesireColor.Cyan:
			return Color.cyan;
		case DesireColor.Blue:
			return Color.blue;
		case DesireColor.Magenta:
			return Color.magenta;
		default:
			return Color.black;
		}
	}

	Dictionary<DesireColor,string> LoadAnswers(int encounterNumber)
	{
		//Create temporary dictionary
		Dictionary<DesireColor, string> hotDic = new Dictionary<DesireColor, string>(emptyAnswerDictionary);
		
		//Encounter answers
		switch(encounterNumber)
		{
		case 0:
		hotDic[DesireColor.Red] = string.Format("Something to give me the edge, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower());
		hotDic[DesireColor.Orange] = string.Format("Nothing for me, thanks.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		hotDic[DesireColor.Yellow] = string.Format("Something to calm the nerves, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		hotDic[DesireColor.Green] = string.Format("Something to energize me, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		hotDic[DesireColor.Cyan] = string.Format("Something to loosen me up, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower());            
		hotDic[DesireColor.Blue] = string.Format("Something to enhance my performance, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		hotDic[DesireColor.Magenta] = string.Format("Something to trip balls on, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
			break;
		case 1:
		hotDic[DesireColor.Red] = string.Format("Something controversial, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Red
		hotDic[DesireColor.Orange] = string.Format("Something vegan, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Orange
		hotDic[DesireColor.Yellow] = string.Format("Something familiar, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Yellow
		hotDic[DesireColor.Green] = string.Format("Something healthy, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Green
		hotDic[DesireColor.Cyan] = string.Format("Something decadent, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Cyan
		hotDic[DesireColor.Blue] = string.Format("Something spicy, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Blue
		hotDic[DesireColor.Magenta] = string.Format("Something bizarre, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Magenta
			break;
		default:
			Debug.Log("Invalid encounter number: " + encounterNumber);
			break;
		}

		//hotDic = ShuffleColorList(hotDic);

		return hotDic;
	}
}

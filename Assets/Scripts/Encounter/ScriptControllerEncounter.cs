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
	int currentEncounter;

	//Dervied strings
	public string headerString;

	//Desire Colors
	public List<DesireColor> answerColors = new List<DesireColor>(){DesireColor.Red, DesireColor.Orange,
		DesireColor.Yellow, DesireColor.Green, DesireColor.Cyan, DesireColor.Blue, DesireColor.Magenta};

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

	public List<Dictionary<DesireColor, string>> encounterAnswers = new List<Dictionary<DesireColor, string>>();

	//Encounter000
	//public string encounter00Title = "The Pharmacy";
	//public string encounter00Prompt = "Care to self medicate?";
	//public Dictionary<DesireColor, string> encounter000Answers = new Dictionary<DesireColor, string>();

	//Encounter001
	//public string encounter001Title = "McGodagle's Restaurant";
	//public string encounter001Prompt = "What'll it be?";
	//public Dictionary<DesireColor, string> encounter001Answers = new Dictionary<DesireColor, string>();
	//public List<Item> foodList = new List<Item>();


	// Use this for initialization
	void Start () {

		//Acquire database
		scriptDatabase = GameObject.Find ("ControllerGame").GetComponent<ScriptDatabase>();

		//for(int i = 0; i < answerColors.Count; i++)
		//{
		//foodList.Add (scriptDatabase.GetRandomConsumable());
		//}



		//Encounter answers
		encounterAnswers[0][DesireColor.Red] = string.Format("Something to give me the edge, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		encounterAnswers[0][DesireColor.Orange] = string.Format("Nothing for me, thanks.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		encounterAnswers[0][DesireColor.Yellow] = string.Format("Something to calm the nerves, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		encounterAnswers[0][DesireColor.Green] = string.Format("Something to energize me, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		encounterAnswers[0][DesireColor.Cyan] = string.Format("Something to loosen me up, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		encounterAnswers[0][DesireColor.Blue] = string.Format("Something to enhance my performance, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 
		encounterAnswers[0][DesireColor.Magenta] = string.Format("Something to trip balls on, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); 

		encounterAnswers[1][DesireColor.Red] = string.Format("Something controversial, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Red
		encounterAnswers[1][DesireColor.Orange] = string.Format("Something vegan, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Orange
		encounterAnswers[1][DesireColor.Yellow] = string.Format("Something familiar, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Yellow
		encounterAnswers[1][DesireColor.Green] = string.Format("Something healthy, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Green
		encounterAnswers[1][DesireColor.Cyan] = string.Format("Something decadent, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Cyan
		encounterAnswers[1][DesireColor.Blue] = string.Format("Something spicy, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Blue
		encounterAnswers[1][DesireColor.Magenta] = string.Format("Something bizarre, the {0}.", scriptDatabase.GetRandomConsumable().fullName.ToLower()); //Magenta

		//Create encounter strings determine encounter answer order - should go in update or function
		//headerString = string.Format ("{0} - {1}", encounterTitles[currentEncounter], encounterPrompts[currentEncounter]);
		//answerColors = ShuffleColorList(answerColors);
}

	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI()
	{
		//defaultStyle = new GUIStyle(GUI.skin.box);
		//cardStyle = defaultStyle;

		GUI.Box (new Rect(10, 10, 400, 370), "");

		GUI.Label (new Rect(20, 20, 350, 30), headerString);
		//Debug.Log (answers[answerColors[0]]);
		if(GUI.Button(new Rect(30, 080, 350, 30), answers[answerColors[0]])){
			resultRevealed = true;
			selectedColor = answerColors[0];}
		if(GUI.Button(new Rect(30, 120, 350, 30), answers[answerColors[1]])){
			resultRevealed = true;
			selectedColor = answerColors[1];;}
		if(GUI.Button(new Rect(30, 160, 350, 30), answers[answerColors[2]])){
			resultRevealed = true;
			selectedColor = answerColors[2];;}
		if(GUI.Button(new Rect(30, 200, 350, 30), answers[answerColors[3]])){
			resultRevealed = true;
			selectedColor = answerColors[3];;}
		if(GUI.Button(new Rect(30, 240, 350, 30), answers[answerColors[4]])){
			resultRevealed = true;
			selectedColor = answerColors[4];}
		if(GUI.Button(new Rect(30, 280, 350, 30), answers[answerColors[5]])){
			resultRevealed = true;
			selectedColor = answerColors[5];}
		if(GUI.Button(new Rect(30, 320, 350, 30), answers[answerColors[6]])){
			resultRevealed = true;
			selectedColor = answerColors[6];}

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
}

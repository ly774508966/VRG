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

public class ScriptChoiceDialogue : MonoBehaviour {

	//Scripts
	public ScriptDatabase scriptDatabase;

	//States
	bool resultRevealed = false;

	//Scaffolding
	public List<Item> foodList = new List<Item>();

	//Header
	public string titleString = "<b>McGodagle's Restaurant</b>";
	public string promptString = "What'll it be?";
	public string headerString;
	
	//Answers
	public Dictionary<DesireColor, string> answers = new Dictionary<DesireColor, string>();
	//public string answersString;

	//Desire Colors
	public List<DesireColor> answerColors = new List<DesireColor>(){DesireColor.Red, DesireColor.Orange,
		DesireColor.Yellow, DesireColor.Green, DesireColor.Cyan, DesireColor.Blue, DesireColor.Magenta};
	DesireColor selectedColor;

	//Styles
	//GUIStyle defaultStyle;
	//GUIStyle cardStyle;
	public GUISkin cardSkin;

	// Use this for initialization
	void Start () {

		scriptDatabase = GameObject.Find ("ControllerGame").GetComponent<ScriptDatabase>();

		for(int i = 0; i < answerColors.Count; i++)
		{
		foodList.Add (scriptDatabase.GetRandomConsumable());
		}


		answerColors = ShuffleColorList(answerColors);

		//Editable answer records

		answers[DesireColor.Red] = string.Format("Something controversial, the {0}.", foodList[0].fullName.ToLower()); //Red
		answers[DesireColor.Orange] = string.Format("Something vegan, the {0}.", foodList[1].fullName.ToLower()); //Orange
		answers[DesireColor.Yellow] = string.Format("Something familiar, the {0}.", foodList[2].fullName.ToLower()); //Yellow
		answers[DesireColor.Green] = string.Format("Something healthy, the {0}.", foodList[3].fullName.ToLower()); //Green
		answers[DesireColor.Cyan] = string.Format("Something decadent, the {0}.", foodList[4].fullName.ToLower()); //Cyan
		answers[DesireColor.Blue] = string.Format("Something spicy, the {0}.", foodList[5].fullName.ToLower()); //Blue
		answers[DesireColor.Magenta] = string.Format("Something bizarre, the {0}.", foodList[6].fullName.ToLower()); //Magenta

		//Debug.Log (answers[DesireColor.Red]);


		//Randomize answer order
		//answers = ShuffleStringList (answers);
		//Debug.Log (answers.Count);
		//answersString = string.Format ("1. {0} \n2. {1} \n3. {2} \n4. {3} \n5. {4} \n6. {5} \n7. {6}", 
		  //                             new string[]{answers[0], answers[1], answers[2], answers[3],
			//							answers[4], answers[5], answers[6]});

		//guiText.text = string.Format ("{0} - {1}\n \n{2}", headerString, questionString, answersString);

		headerString = string.Format ("{0} - {1}", titleString, promptString);

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

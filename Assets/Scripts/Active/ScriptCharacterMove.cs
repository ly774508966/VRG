using UnityEngine;
using System.Collections;

public class ScriptCharacterMove : MonoBehaviour {
	
	ScriptCharacterSheet scriptCharacterSheet;
	public Vector3 startMarker;
	public Vector3 endMarker;
	public float movementSpeed = 0.5F;
	public float journeyLength;
	public float startTime;
	public bool greenLight = false;
	//public bool redLight = false;
	public bool resetLerp = true;
	
	
	//public float durationMultiplier = 1;
	
	// Use this for initialization
	void Start () {
		scriptCharacterSheet = GetComponent<ScriptCharacterSheet>();
		
		
		//GreenLight();
		//RedLight();
		
	
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		if(greenLight){
			if(resetLerp){
				resetLerp = false;
				startTime = Time.time;
		startMarker = transform.position;
		endMarker = scriptCharacterSheet.destination;
		journeyLength = Vector3.Distance(startMarker, endMarker);
			}
			float distCovered = (Time.time - startTime) * movementSpeed;
		float fracJourney = (Time.time - startTime) / journeyLength;
			transform.position = Vector3.Lerp (startMarker, endMarker, fracJourney);
			
		} else {
			if(!resetLerp){
			resetLerp = true;	
			}
		}
		
		
	
		
		
		
		
	}
	/*
	void GreenLight(){
		//Assign static variables
		startTime = Time.time;
		startMarker = transform.position;
		endMarker = scriptCharacterSheet.destination;
		journeyLength = Vector3.Distance(startMarker, endMarker);
		
		//Assign dynamic variables
		float distCovered = (Time.time - startTime) * movementSpeed;
		float fracJourney = (Time.time - startTime) / journeyLength;
		
		//Lerp
		
		
		
		//float duration = (endPoint - startPoint).magnitude * durationMultiplier	
	}
	
	void RedLight(){
		
	}
	*/
}

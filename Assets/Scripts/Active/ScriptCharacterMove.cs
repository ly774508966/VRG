using UnityEngine;
using System.Collections;

public class ScriptCharacterMove : MonoBehaviour {
	
	ScriptCharacterSheet scriptCharacterSheet;
	public Transform startMarker;
	public Transform endMarker;
	public float movementSpeed = 1.0F;
	public float journeyLength;
	public float startTime;
	
	
	public float durationMultiplier = 1;
	
	// Use this for initialization
	void Start () {
		scriptCharacterSheet = GetComponent<ScriptCharacterSheet>();
		startTime = Time.time;
		
		startMarker = transform;
		endMarker = scriptCharacterSheet.destination;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
		
		
		
		
	}
	
	// Update is called once per frame
	void Update () {
		float distCovered = (Time.time - startTime) * movementSpeed;
		float fracJourney = (Time.time - startTime) / journeyLength;
		transform.position = Vector3.Lerp (startMarker.position, endMarker.position, fracJourney);
	}
	
	void GreenLight(){
		
		
		//float duration = (endPoint - startPoint).magnitude * durationMultiplier;
		
		
			
			
	}
}

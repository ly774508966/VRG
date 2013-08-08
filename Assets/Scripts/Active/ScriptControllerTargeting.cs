using UnityEngine;
using System.Collections;

public class ScriptControllerTargeting : MonoBehaviour {
	
	ScriptCharacterSheet scriptCharacterSheet;
	public Vector3 rangedAttack;
	public GameObject characterSource;
	public GameObject characterDestination;
	
	// Use this for initialization
	void Start () {
	scriptCharacterSheet = transform.parent.GetComponent<ScriptCharacterSheet>();
	
	
	
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//Vector3 RangedAttack (Vector3 destination, Vector3 source){
	//return destination - source;	
	
}

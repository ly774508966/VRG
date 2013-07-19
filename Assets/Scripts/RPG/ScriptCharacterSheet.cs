using UnityEngine;
using System.Collections;

public class ScriptCharacterSheet : MonoBehaviour {
	
//Metagame Properties
	public int characterID = -1;
	public int waitTime;
	public bool inPlay = true;
	public string control = null;
	//Initiative
	public bool isInPosition = true;
	
	
//Proper fields
	
	//Info
	public string firstName;
	public string lastName;
	
	//Stats
	public float health = -9999;
	public float focus = -9999;
	public float damage = -9999;
	public float speed = -9999;
	public float accuracy = -9999;
	public float evasion = -9999;
	public float armor = -9999;
	
	
	//Derived stats
	public float priority = -9999;
	public int delay = -9999;
	
//Behavior
	
	//Objectives 
	public GameObject target = null;
	
	//public bool engageTargets;
	//public bool retreat;
	
	
	//Tactics
	
	
	// Use this for initialization
	void Start () {
	
	waitTime = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

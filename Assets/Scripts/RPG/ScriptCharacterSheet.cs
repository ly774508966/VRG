using UnityEngine;
using System.Collections;

public class ScriptCharacterSheet : MonoBehaviour {
	
//Metagame Properties
	public int characterID = -1;
	public int waitTime;
	public bool inPlay = true;
	public string allegiance = null;
	//Initiative
	public bool isInPosition = true;
	
	
//Proper fields
	
	//Info
	public string characterName;
	
	//Stats
	public float health = 10.0F;
	public float focus = 10.0F;
	public float damage = 5.0F;
	public float speed = 5.0F;
	public float accuracy = 10.0F;
	public float evasion = 5.0F;
	public float armor = 0.0F;
	
	
	//Derived stats
	public float priority = 5.0F;
	public int delay = 1;
	
//Behavior
	
	//Objectives 
	public GameObject target = null;
	
	public bool engageTargets;
	public bool retreat;
	
	
	//Tactics
	
	
	// Use this for initialization
	void Start () {
	
	waitTime = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

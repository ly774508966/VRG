using UnityEngine;
using System.Collections;

public class ScriptCharacterSheet : MonoBehaviour {
	
/*NOTES
 * -9999 is used as a placeholder for values that are assigned during runtime (easy debugging). 
 * */

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
	public string fullName;
	
	//Stats
	public int health = -9999;
	public int focus = -9999;
	public int damage = -9999;
	public int speed = -9999;
	public int accuracy = -9999;
	public int evasion = -9999;
	public int armor = -9999;
	public int melee = -9999;
	
	
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

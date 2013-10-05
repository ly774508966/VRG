using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptCharacterSheet : MonoBehaviour {
	
/*NOTES
 * -9999 is used as a placeholder for values that are assigned during runtime (easy debugging). 
 * */

//Metagame Properties
	//Identification
	public int characterID = -1;
	
	//Timing
	public int waitTime = -9999;
	
	//Status
	public bool inPlay = true;
	
	
	
	
	
	
//Appearance	
	public Color primaryColor;
	public Color secondaryColor;
	public Color skinColor;
	
//Proper fields
	
	//Identification
	public string firstName;
	public string lastName;
	public string fullName;
	public string stringID;
	
	//First-Order Stats
	
	public int health = -9999;
	
	public int focus = -9999;
	
	//public int meleeDamage = -9999;
	public int accuracy = -9999;
	public int evasion = -9999;
	public int melee = -9999;
	//public int muscle = -9999;
	
	//Weapon
	public int damage = -9999;
	public float weaponRange = -9999;
	public int weaponCooldown = -9999;
	//public int speed = -9999;
	
	//Equipment
	//public int damageResistanceKinetic;
	//public int damageResistanceThermal;
	
	//Status Effects
	//public List<string> statusEffects = new List<string>();
	
    //Second-Order Stats
	public int headHP = -9999;
	public int bodyHP = -9999;
	public int leftArmHP = -9999;
	public int rightArmHP = -9999;
	public int leftLegHP = -9999;
	public int rightLegHP = -9999;
	
//Update stats
	
	public float lastHitPercentage = -9999;

	
	
//Behavior
	
	//Position
	public bool isInActingPosition = false;
	public Vector3 positionObjective;
	public bool suspendPositionObjective = false;
	
	//Objectives
	public GameObject target = null;
	

	
	//public bool engageTargets;
	//public bool retreat;
	
	
	//Tactics
	public bool engageAtRange = true;
	public bool engageInMelee = false;
	//public bool targetReassess;
	
	//Firing Mode
	public bool aggressiveFire = false;
	public bool blindFire = false;
	public bool aimedFire = false;
	
	//Combat
	
	
//Physics
	public GameObject lastAttacker;
	
		//Attack properties
	public bool propel;
	public bool blowUpHead;
	
	
	
	
	
	// Use this for initialization
	void Start () {
	//positionObjective = transform.position;
		
		
	//waitTime = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
}

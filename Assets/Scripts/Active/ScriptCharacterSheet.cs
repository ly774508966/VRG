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
	
	//public StatProfile baseProfile = new StatProfile(-9999,-9999,-9999,-9999,-9999,-9999,-9999);
	
	//Base/ innate stats
	
	public int meat = -9999;
	public int nerve = -9999;
	public int baseAttack = -9999;
	public int baseDefense = -9999;
	public int baseMuscle = -9999;
	public int unarmedRange = 1;
	
	//Second-Order Stats
	public int headHP = -9999;
	public int bodyHP = -9999;
	public int leftArmHP = -9999;
	public int rightArmHP = -9999;
	public int leftLegHP = -9999;
	public int rightLegHP = -9999;
	
	//public int baseBrains = -9999;
	//public int basePresence = -9999;
	
	//public int melee = -9999; OLD
	
	//Updated, useable stats
	public int readyPriority;
	public int readyAttack;
	public int readyDefense;
	public int readyMuscle;
	public int readyRange;
	public int readyDamage;
	public float currentHitChance;
	
	//public int readyBrains = -9999;
	//public int readyFace
	
	
	//Items
	public List<Item> equippedItems = new List<Item>();
	public List<Item> unequippedItems = new List<Item>();
	
	//public int damage = -9999;
	//public float weaponRange = -9999;
	//public int weaponCooldown = -9999;
	//public int precision = -9999;
	//public DamageType damageType = DamageType.None; 
	//public int speed = -9999;

	//Status Effects
	//public List<string> statusEffects = new List<string>();
	
	//Tactics
	Action currentAction;
	
	//public bool engageAtRange = true;
	//public bool engageInMelee = false;
	
	//public bool targetReassess;
	
//Behavior
	
	//Position
	public bool isInActingPosition = false;
	public Vector3 positionObjective;
	public bool suspendPositionObjective = false;
	
	//Objectives
	public GameObject target = null;
	

	
	//public bool engageTargets;
	//public bool retreat;
	
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

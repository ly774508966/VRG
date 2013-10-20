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
	
	//Base stats
	
	public int health = -9999;
	
	public int focus = -9999;
	
	public int baseAttack = -9999;
	public int baseDefense = -9999;
	//public int melee = -9999;
	public int baseMuscle = -9999;
	public int unarmedRange = 1;
	
	//public int baseBrains = -9999;
	//public int basePresence = -9999;
	
	
	//Modified stats
	//public int finalHealth;
	public int finalFocus;
	public int finalAttack;
	public int finalDefense;
	public int finalMuscle;
	public int finalRange;
	//public int finalBrains;
	//public int finalPresence;
	public int maxRange;
	public int priority;
	public int finalDamage;
	
	
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

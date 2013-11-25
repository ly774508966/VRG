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

	//Identification
	public string firstName;
	public string lastName;
	public string stringID;
	
	//Timing
	public int waitTime = -9999;
	
	//Status
	public bool inPlay = true;
	
//Appearance	
	public Color primaryColor;
	public Color secondaryColor;
	public Color skinColor;
	
//Proper fields
	
	public string fullName;
	
	//public FrameSize frameSize = FrameSize.None;
	public int frameNumber = -9999;
	
	//First-Order Stats
	
	//public StatProfile baseProfile = new StatProfile(-9999,-9999,-9999,-9999,-9999,-9999,-9999);
	
	//Base/ innate stats
	
	//public int baseToughness = -9999;

	//Attributes
	public int toughness;

	//public int strength;
	//public int dexterity;
	//public int agility;
	//public int focus;
	//public int moxie;
	//public int presence;

	//Skills
	//public int archery;
	//public int blades;
	//public int brawl;
	//public int computers;
	//public int firearms;
	//public int grapple;
	//public int hacking;
	//public int psionics;
	//public int seduction;
	//public int stealth;

	//Old Stats

	public int baseMuscle;
	public int baseBrawl;
	public int baseMelee;
	public int baseShot;
	public int baseEvasion;
	public int baseIntelligence;
	public int basePresence;



	public int currentFocus = -9999;
	public int maxFocus = -9999;
	
	//public int baseAttack = -9999;
	//public int baseDefense = -9999;
	//public int baseEvasion = -9999;
	//public int baseMuscle = -9999;
	//public int baseIntelligence = -9999;
	//public int basePresence = -9999;
	//public int baseBrawl = -9999;
	//public int baseMelee = -9999;
	//public int baseShot = -9999;
	
	//Second-Order Stats
	public int unarmedDamage = -9999;
	
	//HP and armor
	public CharacterHitProfile maxHitProfile;
	public CharacterHitProfile currentHitProfile;
	public CharacterHitProfile resistanceHitProfile;

	//public int maxHeadHP = -9999;
	//public int maxBodyHP = -9999;
	//public int maxLeftArmHP = -9999;
	//public int maxRightArmHP = -9999;
	//public int maxLeftLegHP = -9999;
	//public int maxRightLegHP = -9999;
	
	//Current HP
	//public int currentHeadHP = -9999;
	//public int currentBodyHP = -9999;
	//public int currentLeftArmHP = -9999;
	//public int currentRightArmHP = -9999;
	//public int currentLeftLegHP = -9999;
	//public int currentRightLegHP = -9999;

	//Updated, useable stats
	public int readyAttack;
	public int readyDefense;
	public int readyPriority;
	public int readyDamage;
	//public int readyMuscle;
	public int readyRange;
	

	public float currentHitChance;
	
	//Items
	public Item activeItem = null;
	public List<Item> equippedItems = new List<Item>();
	public List<Item> unequippedItems = new List<Item>();
	
	public int netEquipmentAttack = -9999;
	//public int netEquipmentDefense = -9999;
	public int netEquipmentPriority = -9999;
	public int netEquipmentDamage = -9999;
	public int netEquipmentRange = -9999;

	//Status Effects
	//public List<StatusEffect> statusEffects = new List<StatusEffect>();
	
	//Tactics
	public List<Tactic> activeTactics = new List<Tactic>();
	
	public int netTacticsAttack = -9999;
	public int netTacticsDefense = -9999;
	public int netTacticsPriority = -9999;
	public int netTacticsDamage = -9999;
	public int netTacticsRange = -9999;
	
	//public bool engageAtRange = true;
	//public bool engageInMelee = false;
	
	//public bool targetReassess;
	
//Behavior
	
	//Position
	public bool isInActingPosition = false;
	public Vector3 positionObjective;
	public bool suspendPositionObjective = false;
	
	//Objectives
	public ScriptCharacterSheet target = null;
	

	
	//public bool engageTargets;
	//public bool retreat;
	
	//Firing Mode
	//public bool aggressiveFire = false;
	//public bool blindFire = false;
	//public bool aimedFire = false;
	
	//Combat
	
	
//Physics
	//public ScriptCharacterSheet lastAttacker;
	
		//Attack properties
	//public bool propel;
	//public bool blowUpHead;
	
	
	
	
	
	// Use this for initialization
	void Start () {
	//positionObjective = transform.position;
		

	//waitTime = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
}

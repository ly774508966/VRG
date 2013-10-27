using UnityEngine;
using System.Collections;
using System.Collections.Generic;


//Damage
public enum DamageType 
{
	None,
	Kinetic,
	Thermal,
	Sonic
}

//Body parts
public enum BodyPart 
{
	None,
	Head,
	Body,
	LeftArm,
	RightArm,
	LeftLeg,
	RightLeg
}

public enum AttributeName
{
	Pistol
}

public enum AttributeType
{
	Quality,
	Power,
	Purpose
}

//public enum TacticName
//{
//	BasicShot,
//	QuickShot,
//	CalledShot,
//	SteadyShot
//}

public enum TacticType
{
	Action,
	Stance,
	Movement,
	Style
}

//public enum Size
//{
//	None, //Initialization value
//	Tiny, //Can carry infinite, concealable
//	Small, //Can carry a few(?), concealable
//	Large, //Can carry one, not concealable
//	Huge //Can carry one, must be equipped to carry
//}

//public enum ItemStat
//{
//	AttackBonus,
//	PriorityBonus,
//	Damage,
//	
//}

[System.Serializable]
public class ItemStatProfile
{
	public int attackModifier = 0; //Additive- Add to base attack and attack from tactics
	public int priorityModifier = 0; //Additive- Add to focus
	public int damageModifier = 0; //Additive- Add to muscle for certain weapons and damage from tactics
	public int maxRangeAspect = 0; //Native- Primitive/ overrides other
	public int cooldownAspect = 0; //Native
	public int maxAmmoAspect = 0; //Native
	public int sizeAspect = 0; //Native - 0: Tiny, 1: Small, 2: Large, 3: Huge
	public DamageType damageTypeAspect = DamageType.None; //Native
	
	public ItemStatProfile(){}
	
	public ItemStatProfile (int attack, int priority, int damage, int maxRange, int cooldown, int maxAmmo, int size, DamageType damageType) 
		{
		attackModifier = attack;
		priorityModifier = priority;
		damageModifier = damage;
		maxRangeAspect = maxRange;
		cooldownAspect = cooldown;
		maxAmmoAspect = maxAmmo;
		sizeAspect = size;
		damageTypeAspect = damageType;
		}
	}

[System.Serializable]
public class CharacterStatProfile
{
	//public int meat = -9999;
	//public int nerve = -9999;
	public int attack = -9999; 
	public int defense = -9999;
	public int priority = -9999; 
	//public int readyMuscle = -9999;
	public int damage = -9999; 
	public int maxRange = -9999; 
	//public float currentHitChance = -9999;
	public DamageType damageType = DamageType.None; 
	
	//Second-Order Stats
	//public int headHP = -9999;
	//public int bodyHP = -9999;
	//public int leftArmHP = -9999;
	//public int rightArmHP = -9999;
	//public int leftLegHP = -9999;
	//public int rightLegHP = -9999;
	
	public CharacterStatProfile ( int attackArg, int defenseArg, int priorityArg, 
		int damageArg, int maxRangeArg, DamageType damageTypeArg) 
		{
		
		//nerve = nerveArg;
		attack = attackArg;
		defense = defenseArg;
		priority = priorityArg;
		damage = damageArg;
		maxRange = maxRangeArg;
		//currentHitChance = currentHitChaceArg;
		damageType = damageTypeArg;
		
		}
	
	public CharacterStatProfile(){}
	
	} 
	
[System.Serializable]
public class Tactic
{
	//public TacticName tacticName;
	public string stringName;
	public TacticType tacticType;
	public CharacterStatProfile modifierProfile;

	public Tactic (string stringNameArg, TacticType tacticTypeArg, CharacterStatProfile modifierProfileArg)
	{
		//tacticName = tacticNameArg;
		stringName = stringNameArg;
		tacticType = tacticTypeArg;
		modifierProfile = modifierProfileArg;
	}	
}
		
		
		public class Attribute
	{
		public string attributeName = name;
		//AttributeType attributeType = type;
		public ItemStatProfile attributeProfile = profile;
		//public DamageType damageType = DamageType.None; //Overrides damage type
		public Tactic[] basicActions;
		//Effect[] effects;
	
		public Attribute (string name, ItemStatProfile profile)
	{
		
		attributeName = name;
		//AttributeType attributeType = type;
		attributeProfile = profile;
		//damageType = DamageType.None; //Overrides damage type
	}
}
	[System.Serializable]
	public class Item
	{
		
		public string fullName;
		public int itemID = -9999;
		public string namePart0 = "";
		public string namePart1 = "";
		public string namePart2 = "";
		
		public ScriptCharacterSheet owner = null;
		
		//Crate
		string crateLabel;
		Color crateColor;
		
		//Stats
		public ItemStatProfile netStatProfile = new ItemStatProfile();
	
		public DamageType damageType = DamageType.None;
	
		//Status
		public int currentAmmo = -9999;
		public bool isConcealed = false;
		public int itemWaitTime = -9999;
	
		public Item (){
	    //itemID = nextItemID;
		//fullName = fullNameArg;
	}
	}
/*
public class Statshot
{
	public Statshot(ScriptCharacterSheet hotChar)
	{
		ScriptCharacterSheet owner = hotChar;
		
		StatProfile hotProfile = hotChar.baseProfile;
		hotProfile = ApplyEquippedItemProfiles(hotProfile);
		hotProfile = ApplyTacticsProfiles(hotProfile);
		
	}
}
 */

public class ScriptDatabase : MonoBehaviour {
	
	Item tempItem;
	public int nextItemID = 0;
	
	//Attribute records
	Attribute[] purposeAttributes = new Attribute[]{
		new Attribute("Pistol", new ItemStatProfile(0, 1, 4, 10, 0, 8, 1, DamageType.Kinetic)),
		new Attribute("Shotgun", new ItemStatProfile(2, 1, 10, 8, 2, 6, 2, DamageType.Kinetic))
	};
	
	Attribute[] powerAttributes = new Attribute[]{
		new Attribute("Gauss", new ItemStatProfile(0, 0, 2, 2, 1, 0, 0, DamageType.Kinetic)),
		new Attribute("Powder", new ItemStatProfile(-1, 0, 0, -2, 0, 0, 0, DamageType.Kinetic))
	};
	
	Attribute[] qualityAttributes = new Attribute[]{
		new Attribute("Overclocked", new ItemStatProfile(0, 0, 2, 0, 1, 0, 0, DamageType.None)),
		new Attribute("Crummy", new ItemStatProfile(-1, 0, -1, -2, 0, 0, 0, DamageType.None))
	};
	
	//Tactic records
	public Dictionary<string, Tactic> tacticsLookup = new Dictionary<string, Tactic>(){
		{"Basic Shot", new Tactic("Basic Shot", TacticType.Action, new CharacterStatProfile(0, 0, 0, 0, 0, DamageType.None))},
		{"Wild Shot", new Tactic("Wild Shot", TacticType.Action, new CharacterStatProfile(-2, 0, 3, 0, 0, DamageType.None))}	

	};
	
	//Premade items
	//Item unarmed = new Item();
	
	//Tactic[] tacticsLookup = new Tactic[]{
	//	new Tactic("Basic Shot", TacticType.Action, new CharacterStatProfile( 0, 0, 0, 0, 0, DamageType.None))  	
	//};

	
	//public Dictionary<AttributeName,Attribute> attributeInfo = 
	//	new Dictionary<AttributeName, Attribute> ()
	//{
	//	{pistol,SetAttribute( 	
	//};
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public Item GetRandomItem()
	{
		Item hotItem = new Item();
		
		//Assign item ID
		hotItem.itemID = nextItemID;
		nextItemID += 1;
		
		//Determine purpose attribute
		Attribute purposeAttribute = purposeAttributes[Random.Range (0, purposeAttributes.Length)];
			
		//Determine material attribute
		Attribute powerAttribute = powerAttributes[Random.Range (0, powerAttributes.Length)];
		
		//Determine quality attribute
		Attribute qualityAttribute = qualityAttributes[Random.Range (0, qualityAttributes.Length)];
		
		//Apply purpose attribute
		hotItem = ApplyAttribute(hotItem, purposeAttribute, AttributeType.Purpose);
		
		//Apply material attribute
		hotItem = ApplyAttribute(hotItem, powerAttribute, AttributeType.Power);
		
		//Apply quality attribute
		hotItem = ApplyAttribute(hotItem, qualityAttribute, AttributeType.Quality);
		
		//Debug
		//hotItem.attack = 42;
		
		//Set name
		hotItem.fullName = hotItem.namePart0 + " " + hotItem.namePart1 + " " + hotItem.namePart2;
		
		//Fill clip
		hotItem.currentAmmo = hotItem.netStatProfile.maxAmmoAspect;
		
		return hotItem;
	}
	
	//ModifierProfile GetModifierProfile()
	//{
	//	return new ModifierProfile(0,0,0,0,0,0,0);
	//}
	
	//void SetModifierProfile(ModifierProfile hotProfile)
	//{
	//	
	//}
	
	Item ApplyAttribute(Item hotItem, Attribute hotAttribute, AttributeType attributeType)
	{
		//Populate name fields
		if(attributeType == AttributeType.Quality)
		{
			hotItem.namePart0 = hotAttribute.attributeName;
		}
		else if(attributeType == AttributeType.Power)
		{
			hotItem.namePart1 = hotAttribute.attributeName;
		}
		else if(attributeType == AttributeType.Purpose)
		{
			hotItem.namePart2 = hotAttribute.attributeName;
		}
		else
		{
			Debug.Log ("Invalid attribute type: " + attributeType);
		}
		
		//Add both profiles
		hotItem.netStatProfile.attackModifier += hotAttribute.attributeProfile.attackModifier;
		hotItem.netStatProfile.cooldownAspect += hotAttribute.attributeProfile.cooldownAspect;
		hotItem.netStatProfile.damageModifier += hotAttribute.attributeProfile.damageModifier;
		hotItem.netStatProfile.maxAmmoAspect += hotAttribute.attributeProfile.maxAmmoAspect;
		hotItem.netStatProfile.maxRangeAspect += hotAttribute.attributeProfile.maxRangeAspect;
		hotItem.netStatProfile.priorityModifier += hotAttribute.attributeProfile.priorityModifier;
		hotItem.netStatProfile.sizeAspect = hotAttribute.attributeProfile.sizeAspect;
		
		//Overwrite current damage type if new one exists
		if(hotAttribute.attributeProfile.damageTypeAspect != DamageType.None)
		{
		hotItem.netStatProfile.damageTypeAspect = hotAttribute.attributeProfile.damageTypeAspect;
		}
		
		//Do something with actions
		
		return hotItem;
	}
	
}
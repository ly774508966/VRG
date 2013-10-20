using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Damage
public enum DamageType 
	{
		None,
		Kinetic,
		Thermal
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
	Composition,
	Purpose
}

public enum ItemStat
{
	AttackBonus,
	PriorityBonus,
	Damage,
	
}

	public class ItemStatProfile
	{
	public ItemStatProfile (int attack, int priority, int damage, int maxRange, int cooldown, int maxAmmo, int size) 
		{
		int attackModifier = attack;
		int priorityModifier = priority;
		int damageModifier = damage;
		int maxRangeModifier = maxRange;
		int cooldownModifier = cooldown;
		int maxAmmoModifier = maxAmmo;
		int sizeModifier = size;
		}
	}

	
	public class Action
		{
		string actionName;
		ItemStatProfile modifierProfile;
		
		}
		
		public class Attribute
	{
		public Attribute (string name, AttributeType type, ItemStatProfile profile)
	{
		
		string attributeName = name;
		AttributeType attributeType = type;
		ItemStatProfile attributeProfile = profile;
		DamageType damageType = DamageType.None;
		ItemStatProfile modifierProfile;
		Action[] basicActions;
		//Effect[] effects;
	}
}
	[System.Serializable]
	public class Item
	{
		
		public string itemName;
		public int itemID;
		
		public ScriptCharacterSheet owner = null;
		
		//Crate
		string crateLabel;
		Color crateColor;
		
		//Stats
		public int attack = -9999;
		public int priority = -9999;
		public int damage = -9999;
		public int maxRange = -9999;
		public int cooldown = -9999;
		public int maxAmmo = -9999;
		public int size = -9999;
		public DamageType damageType = DamageType.None;
	
		//Status
		public int currentAmmo = -9999;
		public bool isConcealed = false;
		public int itemWaitTime = -9999;
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
	int nextItemID = 0;

	Attribute[] attributes = new Attribute[]{
		new Attribute("Pistol", AttributeType.Purpose, new ItemStatProfile(0, 1, 4, 10, 0, 8, 1))
	};

	
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
	
	public Item CreateRandomItem()
	{
		Item hotItem = new Item();
		
		//Reigster item
		hotItem.itemID = nextItemID;
		nextItemID += 1;
		
		//Determine purpose attribute
		
		//Determine material attribute
		
		//Determine quality attribute
		
		//Apply purpose attribute
		
		//Apply material attribute
		
		//Apply quality attribute
		
		//Increment item ID
		
		//Debug
		hotItem.attack = 42;
		
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
	
}
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





public class ScriptDatabase : MonoBehaviour {
		
	
	public class Effect
		{
		int modifier;
		ItemStat stat;
			string special;
		}
		
		public class Attribute
	{
		public Attribute (string name, AttributeType type, int attack, int priority,
			int damage, int maxRange, int cooldown, int maxAmmo, int size)
		{
		string attributeName = name;
		AttributeType attributeType = type;
		int attackModifier = attack;
		int priorityModifier = priority;
		int damageModifier = damage;
		int maxRangeModifier = maxRange;
		int cooldownModifier = cooldown;
		//int currentAmmo = -9999;
		int maxAmmoModifier = maxAmmo;
		int sizeModifier = size;
		DamageType damageType = DamageType.None;
		Effect[] effects;
		}	
	}
	public class Item
	{
		
		string itemName;
		int itemID;
		
		int attack = -9999;
		int priority = -9999;
		
		int damage = -9999;
		DamageType damageType = DamageType.None;
		
		int maxRange = -9999;
		int cooldown = -9999;
		
		int currentAmmo = -9999;
		int maxAmmo = -9999;
		
		int size = -9999;
		bool canBeConcealed = false;
}

		
	Attribute[] attributes = new Attribute[]{
		new Attribute("Pistol", AttributeType.Purpose, 0, 1, 4, 10, 0, 8, 1)
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
	
	Item GetRandomItem(int itemID)
	{
		Item hotItem = new Item();
		
		//Determine purpose attribute
		
		//Determine material attribute
		
		//Determine quality attribute
		
		//Apply purpose attribute
		
		//Apply material attribute
		
		//Apply quality attribute
		
		
		
		
		return hotItem;
	}
	
}

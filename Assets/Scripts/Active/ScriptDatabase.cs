using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Enums


public enum DamageType 
{
	None,
	Piercing,
	Slashing,
	Bludgeoning,
	Burning,
	Sonic,
	Corrosive,
	Electrical,
	Disintegrating
}

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


public enum AttributeType
{
	Quality,
	Power,
	Purpose,
	FoodQuality,
	Ingredient,
	Dish
}

public enum TacticType
{
	None,
	Action,
	Stance,
	Movement,
	Style
}

public enum AttackType
{
	None,
	Brawl,
	Melee,
	Shot
}

//Character classes

public class CharacterTemplate
{
	public string fullName;
	public CharacterPoolProfile characterStatProfile;
	public Color primaryColor;
	public Color secondaryColor;
	public Color skinColor;
	//public FrameSize frameSize;
	
	public CharacterTemplate (string fullNameArg, Color primaryColorArg, 
		Color secondaryColorArg, Color skinColorArg, CharacterPoolProfile characterStatProfileArg)
	{
	fullName = fullNameArg;
	primaryColor = primaryColorArg;
	secondaryColor = secondaryColorArg;
	skinColor = skinColorArg;
	characterStatProfile = characterStatProfileArg;
	}

}

[System.Serializable]
public class CharacterPoolProfile
{
	//public int meat = -9999;
	//public int nerve = -9999;
	public int focus = -9999; 
	public int brawl = -9999;
	public int melee = -9999;
	public int shot = -9999;
	public int evasion = -9999;
	public int toughness = -9999;
	public int muscle = -9999;
	public int intelligence = -9999;
	public int presence = -9999;
	
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
	
	public CharacterPoolProfile(){}
	
	public CharacterPoolProfile ( int focusArg, int brawlArg, int meleeArg, int shotArg, int evasionArg, 
		 int toughnessArg, int muscleArg, int intelligenceArg, int presenceArg) 
		{
		//nerve = nerveArg;
		focus = focusArg;
		brawl = brawlArg;
		melee = meleeArg;
		shot = shotArg;
		evasion = evasionArg;
		toughness = toughnessArg;
		muscle = muscleArg;
		intelligence = intelligenceArg;
		presence = presenceArg;
		
		//damage = damageArg;
		//maxRange = maxRangeArg;
		//currentHitChance = currentHitChaceArg;
		//damageType = damageTypeArg;
		}
} 

[System.Serializable]
public class CharacterHitProfile
{
	public int head;
	public int body;
	public int leftArm;
	public int rightArm;
	public int leftLeg;
	public int rightLeg;

	public CharacterHitProfile(){}

	public CharacterHitProfile(CharacterHitProfile originalHitProfile)
	{
		head = originalHitProfile.head;
		body = originalHitProfile.body;
		leftArm = originalHitProfile.leftArm;
		rightArm = originalHitProfile.rightArm;
		leftLeg = originalHitProfile.leftLeg;
		rightLeg = originalHitProfile.rightLeg;
	}

	public CharacterHitProfile(int headArg, int bodyArg, int leftArmArg, int rightArmArg, int leftLegArg, int rightLegArg)
	{
		head = headArg;
		body = bodyArg;
		leftArm = leftArmArg;
		rightArm = rightArmArg;
		leftLeg = leftLegArg;
		rightLeg = rightLegArg;
	}
}




//Item classes
	[System.Serializable]
	public class Item
	{
		//Name
		public string fullName;
		public int itemID = -9999;
		public string namePart0 = "";
		public string namePart1 = "";
		public string namePart2 = "";
		
		public CharacterSheet owner = null;
		
		//Appearance
	//Color itemColor;
	//ItemStyle itemStyle;
		
		//Stats
		public AttackType attackType;
		public DamageType damageType = DamageType.None;
		public ItemStatProfile itemStatProfile = new ItemStatProfile();
	
		//Status
		public int usesRemaining = -9999;
		public bool isConcealed = false;
		public int itemWaitTime = -9999;

		//Aesthetics
	public AudioClip itemSound;
	public Color projectileColor;
	public Color effectColor;
	public Color itemColor;
	
		public Item (){}
		public Item (string fullNameArg, AttackType attackTypeArg, DamageType damageTypeArg, ItemStatProfile itemStatProfileArg ){
	    //itemID = nextItemID;
		fullName = fullNameArg;
		attackType = attackTypeArg;
		damageType = damageTypeArg;
		itemStatProfile = itemStatProfileArg;
	
		}
	}

[System.Serializable]
public class ItemStatProfile
{
	public int attackModifier = 0; //Additive- Add to base attack and attack from tactics
	public int priorityModifier = 0; //Additive- Add to focus
	public int damageModifier = 0; //Additive- Add to muscle for certain weapons and damage from tactics
	public int maxRangeAspect = 0; //Native- Primitive/ overrides other
	public int cooldownAspect = 0; //Native
	public int usesStarting = 0; //Native
	public int sizeAspect = 0; //Native - 0: Tiny, 1: Small, 2: Large, 3: Huge
	//public DamageType damageTypeAspect = DamageType.None; //Native
	
	public ItemStatProfile(){}
	
	public ItemStatProfile (int attack, int priority, int damage, int maxRange, int cooldown, int startingUsesArg, int size) 
		{
		attackModifier = attack;
		priorityModifier = priority;
		damageModifier = damage;
		maxRangeAspect = maxRange;
		cooldownAspect = cooldown;
		usesStarting = startingUsesArg;
		sizeAspect = size;
		//damageTypeAspect = damageType;
		}
	}

public class Attribute
	{
		public string attributeName;
		//AttributeType attributeType = type;
		public AttackType attackType = AttackType.None;
		public DamageType damageType = DamageType.None;
		public ItemStatProfile attributeProfile;
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
	public Attribute (string attributeNameArg) 
	{
		attributeName = attributeNameArg;
	}
			public Attribute (string attributeNameArg, AttackType attackTypeArg, DamageType damageTypeArg, ItemStatProfile itemStatProfileArg)
	{
		
		attributeName = attributeNameArg;
		//AttributeType attributeType = type;
		attackType = attackTypeArg;
		damageType = damageTypeArg;
		attributeProfile = itemStatProfileArg;
		//damageType = DamageType.None; //Overrides damage type
	}
}

	//Tactic classes
[System.Serializable]
public class Tactic
{
	//public TacticName tacticName;
	public string stringName;
	public TacticType tacticType;
	public TacticStatProfile tacticStatProfile;

	public Tactic (string stringNameArg, TacticType tacticTypeArg, TacticStatProfile tacticStatProfileArg)
	{
		//tacticName = tacticNameArg;
		stringName = stringNameArg;
		tacticType = tacticTypeArg;
		tacticStatProfile = tacticStatProfileArg;
	}	
}

[System.Serializable]
public class TacticStatProfile
{
	public int attack = -9999; //Additive- Add to base attack and attack from tactics
	public int defense = -9999;
	public int priority = -9999; //Additive- Add to focus
	public int damage = -9999; //Additive- Add to muscle for certain weapons and damage from tactics
	public int maxRange = -9999; //Native- Primitive/ overrides other
	public int cooldown = -9999; //Native

	
	public TacticStatProfile(){}
	
	public TacticStatProfile (int attackArg, int defenseArg, int priorityArg, int damageArg, int maxRangeArg, int cooldownArg) 
	{
		attack = attackArg;
		defense = defenseArg;
		priority = priorityArg;
		damage = damageArg;
		maxRange = maxRangeArg;
		cooldown = cooldownArg;
	}
}

//Action classes
	[System.Serializable]
public class Result
{
	public CharacterSheet actingCharacter = null;
	public CharacterSheet targetCharacter = null;
	public bool success = false;
	public DamageType damageType = DamageType.None;
	public int grossDamage = -9999;
	public int successNumber = -9999;
	public int hitPercentage = -9999;
	public int roll = -9999;
	public int rollExcess = -9999;
	public int actingAttack = -9999;
	public int targetDefense = -9999;
	//public List<BodyPart> hitLocations = new List<BodyPart>();
	public CharacterHitProfile targetGrossHitProfile = new CharacterHitProfile();
	//public CharacterHitProfile targetHitResistanceProfile = new CharacterHitProfile();
	public CharacterHitProfile targetNetHitProfile = new CharacterHitProfile();
	public GameObject hitLocation;
	public PlayerShotInfo playerShotInfo;

	
		public Result(CharacterSheet actingCharacterSheet)
		{
			actingCharacter = actingCharacterSheet;
		}
	}

//Player
public class PlayerShotInfo
{
	public CharacterSheet shooter;
	public CharacterSheet target;
	public GameObject shotLocation;
	public Ray shotRay;

	public PlayerShotInfo(CharacterSheet shooterArg)
	{
		shooter = shooterArg;
		//target = targetArg;
		//shotLocation = shotLocationArg;
	}
}

public class ScriptDatabase : MonoBehaviour {

	private static ScriptDatabase _instance;
	public static ScriptDatabase Instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<ScriptDatabase>();
			}
			return _instance;
		}
	}

	//Runtime variables
	Item tempItem;
	public int nextItemID = 0;

	//Name records
	public List<string> firstNames = new List<string>(new string[] {"Jumbo", "Ham", "Tassik", 
		"Marinn", "Rose", "Joseph", "Dash", "Jaedon", "Argot", "Tau", "Rachel", "Julien", "Lily", "Larry", 
		"Maynard", "Leo", "Ota", "Gulliver", "Megan", "Freck", "Korder", "Lincoln, Hayden, Mary, Violet"});
	public List<string> lastNames = new List<string>(new string[] {"Baloney", "Jehosephat", "Kayla", 
		"Dillon", "Reynolds", "Wild", "Rendar", "Casio", "Veis", "Ceti", "Vega", "Pavec", "Puncture", 
		"Jello", "Thatcher", "Marshall", "Stockholm", "Retri", "Freck", "Korder", "Lincoln, Wyr, Gentry, Nova"});

	//Item attribute records
	Attribute[] purposeAttributes = new Attribute[]{
		new Attribute("Pistol", AttackType.Shot, DamageType.None, new ItemStatProfile(0, 1, 4, 10, 0, 8, 1)),
		new Attribute("Shotgun", AttackType.Shot, DamageType.None, new ItemStatProfile(2, 1, 10, 8, 2, 6, 2)),
		new Attribute("Machine Gun", AttackType.Shot, DamageType.None, new ItemStatProfile(2, 2, 6, 8, 1, 3, 3))
	};
	
	Attribute[] powerAttributes = new Attribute[]{
		new Attribute("Gauss", AttackType.None, DamageType.Piercing, new ItemStatProfile(0, 0, 2, 2, 1, 0, 0)),
		new Attribute("Powder", AttackType.None, DamageType.Piercing, new ItemStatProfile(-1, 0, 0, -2, 0, 0, 0)),
		new Attribute("Sonic", AttackType.None, DamageType.Sonic, new ItemStatProfile(3, -2, 4, 4, 3, 3, 1))
	};
	
	Attribute[] qualityAttributes = new Attribute[]{
		new Attribute("Overclocked", new ItemStatProfile(0, 0, 2, 0, 1, 0, 0)),
		new Attribute("Crummy", new ItemStatProfile(-1, 0, -1, -2, 0, 0, 0))
	};


	Attribute[] foodQualityAttributes = new Attribute[]
	{
		new Attribute("Extra Spicy"),
		new Attribute("Sour"),
		new Attribute("Tantalizing"),
		new Attribute("Rancid"),
		new Attribute("Pungent"),
		new Attribute("Pangalactic"),
		new Attribute("Zero-Calorie"),
		new Attribute("Invigorating"),
		new Attribute("Squirming"),
		new Attribute("Putrid"),
		new Attribute("Piping-Hot"),
		new Attribute("Bitter"),
		new Attribute("Whole-Fat"),
		new Attribute("Decadent"),
		new Attribute("Salty"),
		new Attribute("Hypnotic"),
		new Attribute("Velvety"),
		new Attribute("Whipped"),
		new Attribute("Vegan"),
		new Attribute("Dangerous"),
		new Attribute("Soggy"),
		new Attribute("Poisonous"),
		new Attribute("Barbequed"),
		new Attribute("Pickled"),
		new Attribute("Expired"),
		new Attribute("Fuzzy"),
		new Attribute("Grade-D"),
		new Attribute("Hallucinogenic"),
		new Attribute("Deep-Fried"),
		new Attribute("Teriyaki"),
		new Attribute("Grilled"),

	};

	Attribute[] ingredientAttributes = new Attribute[]
	{
		new Attribute("Turkey"),
		new Attribute("Whiskey"),
		new Attribute("Tequila"),
		new Attribute("Ranch"),
		new Attribute("Shrimp"),
		new Attribute("Excrement"),
		new Attribute("Mushroom"),
		new Attribute("Bacon"),
		new Attribute("Goat Cheese"),
		new Attribute("Buffalo"),
		new Attribute("Tentacle"),
		new Attribute("Escargot"),
		new Attribute("Spinach"),
		new Attribute("Mystery"),
		new Attribute("Dragon"),
		new Attribute("Habanero"),
		new Attribute("Knuckle"),
		new Attribute("Lard"),
		new Attribute("Spam"),
		new Attribute("Chocolate"),
		new Attribute("Strawberry"),
		new Attribute("Pork"),
		new Attribute("Garden"),
		new Attribute("Fatback"),
		new Attribute("Pickle"),
		new Attribute("Watermelon"),
		new Attribute("Alligator"),
		new Attribute("Antiseptic"),
		new Attribute("Mayo-Grade"),
		new Attribute("Human"),
		new Attribute("Unicorn"),
		new Attribute("Tuna"),
		new Attribute("Eel"),
		new Attribute("Garlic")
	};

	Attribute[] dishAttributes = new Attribute[]
	{
		new Attribute("Burrito"),
		new Attribute("Cocktail"),
		new Attribute("Smoothie"),
		new Attribute("Sundae"),
		new Attribute("Taco"),
		new Attribute("Noodles"),
		new Attribute("Sauce"),
		new Attribute("Syrup"),
		new Attribute("Frittata"),
		new Attribute("Surprise"),
		new Attribute("Jerky"),
		new Attribute("Coffee"),
		new Attribute("Croissant"),
		new Attribute("Salad"),
		new Attribute("Pie"),
		new Attribute("Froyo"),
		new Attribute("Roll"),
		new Attribute("Gelato"),
		new Attribute("Pickle"),
		new Attribute("Kabob"),
		new Attribute("Banana"),
		new Attribute("Relish"),
		new Attribute("Aoili"),
		//new Attribute("Lager"),
		new Attribute("Merlot"),
		new Attribute("Dressing"),
		new Attribute("Pizza"),
		new Attribute("Curry"),
		new Attribute("Testicles")
	};
	
	//Tactic records
	public Dictionary<string, Tactic> tacticsLookup = new Dictionary<string, Tactic>(){
		{"Basic Shot", new Tactic("Basic Shot", TacticType.Action, new TacticStatProfile(0, 0, 0, 0, 0, 0))}
		//,
		//{"Wild Shot", new Tactic("Wild Shot", TacticType.Action, new CharacterPoolProfile(-2, 0, 3, 0, 0, DamageType.None))}	

	};
	
	//Premade items
	public Item debugItem = new Item("Debugger", AttackType.Brawl, DamageType.Corrosive, new ItemStatProfile(99999, 99999, 99999, 99999, 99999, 99999, 99999));
	public Item unarmed = new Item("Unarmed", AttackType.Brawl, DamageType.Bludgeoning, new ItemStatProfile(0, 0, 0, 1, 0, 0, 0));
	
	//Premade characters
	//public CharacterTemplate coppermouth = new CharacterTemplate(
	//	"Coppermouth", Color.green, Color.yellow, Color.white,
	//	new CharacterPoolProfile(10, 10, 8, 4, 10, 4, 5, 8, 4));
	
	//Tactic[] tacticsLookup = new Tactic[]{
	//	new Tactic("Basic Shot", TacticType.Action, new CharacterPoolProfile( 0, 0, 0, 0, 0, DamageType.None))  	
	//};

	void Awake() {
		_instance = this;
	}


	public Item GetRandomConsumable()
	{
		Item hotItem = new Item();

		//Assign item ID
		hotItem.itemID = nextItemID;
		nextItemID += 1;

		//Determine and apply dish attribute
		//int i = dishAttributes.Length - 1;
		int i = Random.Range (0, dishAttributes.Length);
		Attribute dishAttribute = dishAttributes[i];
		hotItem = ApplyAttribute(hotItem, dishAttribute, AttributeType.Dish);

		//Debug
		//if(i > highestI)
		//{
		//	highestI = i;
		//}

		//Determine and apply ingredient attribute
		int j = Random.Range (0, ingredientAttributes.Length);
		Attribute ingredientAttribute = ingredientAttributes[j];
		hotItem = ApplyAttribute(hotItem, ingredientAttribute, AttributeType.Ingredient);

		//Determine apply food quality attribute
		int k = Random.Range (0, foodQualityAttributes.Length);
		Attribute foodQualityAttribute = foodQualityAttributes[k];
		hotItem = ApplyAttribute(hotItem, foodQualityAttribute, AttributeType.FoodQuality);

		//Debug.Log(string.Format("{0} / {1}; {2} / {3}; {4} / {5}", k, foodQualityAttributes.Length - 1, j, 
		  //                      ingredientAttributes.Length - 1, i, dishAttributes.Length - 1));

		//Set name
		hotItem.fullName = hotItem.namePart0 + " " + hotItem.namePart1 + " " + hotItem.namePart2;

		return hotItem;

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
		hotItem.usesRemaining = hotItem.itemStatProfile.usesStarting;
		
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
		//If item is equippable
		if(attributeType == AttributeType.Quality || attributeType == AttributeType.Power || 
		   attributeType == AttributeType.Purpose)
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
		hotItem.itemStatProfile.attackModifier += hotAttribute.attributeProfile.attackModifier;
		hotItem.itemStatProfile.cooldownAspect += hotAttribute.attributeProfile.cooldownAspect;
		hotItem.itemStatProfile.damageModifier += hotAttribute.attributeProfile.damageModifier;
		hotItem.itemStatProfile.usesStarting += hotAttribute.attributeProfile.usesStarting;
		hotItem.itemStatProfile.maxRangeAspect += hotAttribute.attributeProfile.maxRangeAspect;
		hotItem.itemStatProfile.priorityModifier += hotAttribute.attributeProfile.priorityModifier;
		hotItem.itemStatProfile.sizeAspect = hotAttribute.attributeProfile.sizeAspect;
		
		//Overwrite current damage type if new one exists
		if(hotAttribute.damageType != DamageType.None)
		{
		hotItem.damageType = hotAttribute.damageType;
		}

		
		//Overwrite attack type if new one exists
		if(hotAttribute.attackType != AttackType.None)
		{
		hotItem.attackType = hotAttribute.attackType;
		}
		
		
		//return hotItem;

		}
		else if (attributeType == AttributeType.FoodQuality || attributeType == AttributeType.Ingredient || 
		              attributeType == AttributeType.Dish)
		{
			//Apply consumable attributes
			//Populate name fields
			if(attributeType == AttributeType.FoodQuality)
			{
				hotItem.namePart0 = hotAttribute.attributeName;
			}
			else if(attributeType == AttributeType.Ingredient)
			{
				hotItem.namePart1 = hotAttribute.attributeName;
			}
			else if(attributeType == AttributeType.Dish)
			{
				hotItem.namePart2 = hotAttribute.attributeName;
			}
			else
			{
				Debug.Log ("Invalid attribute type: " + attributeType);
			}
		}
		else
		{
			Debug.Log("Invalid Attribute Type: " + attributeType.ToString());

		}
		return hotItem;
	}
	
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScriptItemCrate : MonoBehaviour {

	public List<Item> containedItems = new List<Item>();

	public GameObject lid;
	public GameObject xSide0;
	public GameObject xSide1;
	public GameObject ySide;
	public Light crateLight;
	public Color crateColor;
	public Material crateMaterial;
	AudioSource audioSource;
	public const float Y_FORCE = 1000;
	public ScriptDatabase scriptDatabase;
	
	
	// Use this for initialization
	void Start () {

		//Get scripts
		scriptDatabase = GameObject.Find("ControllerGame").GetComponent<ScriptDatabase>();

		//Get pieces
		lid = transform.FindChild("ObjCrateLid").gameObject;
		xSide0 = transform.FindChild("ObjCrateXSide0").gameObject;
		xSide1 = transform.FindChild("ObjCrateXSide1").gameObject;
		ySide = transform.FindChild("ObjCrateYSide").gameObject;
		
		crateLight = transform.FindChild("LigCratePoint").gameObject.GetComponent<Light>();
		audioSource = GetComponent<AudioSource>();
		
		SetCrateColor(GetRandomCrateColor());

		//Fill with 100 random consumables
		for(int i = 0; i < 100; i++)
		{
		containedItems.Add (scriptDatabase.GetRandomConsumable());
		}

	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.O))
		{
			
			PopOpen();
		}
		
				if(Input.GetKeyDown(KeyCode.C))
		{
			
			SetCrateColor(GetRandomCrateColor());
		}
		
	}
	
	void PopOpen ()
	{
		//Debug.Log ("Pop open call received");
		lid.rigidbody.isKinematic = false;
		lid.rigidbody.WakeUp();
		lid.rigidbody.AddForce((Random.value * 2 - 1) * 100, Y_FORCE, Random.value * -500);
		lid.renderer.material.color = Color.white;
		xSide0.renderer.material.color = Color.white;
		xSide1.renderer.material.color = Color.white;
		ySide.renderer.material.color = Color.white;
		crateLight.gameObject.SetActive(false);
		audioSource.Play ();

		//Debug.Log(containedItem.fullName);
		//Debug.Log(scriptDatabase.highestI);
	}
	
	Color GetRandomCrateColor()
	{
		//Debug.Log ("Random number call");
	return new Color(Random.value, Random.value, Random.value, 255);
		
	}
	void SetCrateColor(Color crateColor)
	{
		//Debug.Log ("Set color call");
		//foreach(Transform child in transform)
		//{
		//child.gameObject.renderer.material.color = crateColor;		
		//}
		lid.renderer.material.color = crateColor;
		xSide0.renderer.material.color = crateColor;
		xSide1.renderer.material.color = crateColor;
		ySide.renderer.material.color = crateColor;
		
		//crateMaterial.SetColor("_Color", crateColor);
		crateLight.color = crateColor;
		
	}
}

using UnityEngine;
using System.Collections;

public class ScriptItemCrate : MonoBehaviour {
	
	GameObject lid;
	Light crateLight;
	public Color crateColor;
	public Material crateMaterial;
	AudioSource audioSource;
	
	
	// Use this for initialization
	void Start () {
		lid = transform.FindChild("ObjCrateLid").gameObject;
		crateLight = transform.FindChild("LigCratePoint").gameObject.GetComponent<Light>();
		audioSource = GetComponent<AudioSource>();
		
		SetCrateColor(GetRandomCrateColor());
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
		lid.rigidbody.isKinematic = false;
		lid.rigidbody.WakeUp();
		lid.rigidbody.AddForce((Random.value * 2 - 1) * 100, 750, Random.value * -500);
		crateMaterial.SetColor("_Color", Color.white);
		crateLight.gameObject.SetActive(false);
		audioSource.Play ();
	}
	
	Color GetRandomCrateColor()
	{
		Debug.Log ("Random number call");
	return new Color(Random.value, Random.value, Random.value, 255);
		
	}
	void SetCrateColor(Color crateColor)
	{
		Debug.Log ("Set color call");
		foreach(Transform child in transform)
		{
		child.gameObject.renderer.material.color = crateColor;		
		}
		
		//crateMaterial.SetColor("_Color", crateColor);
		crateLight.color = crateColor;
		
	}
}

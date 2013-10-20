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
		lid.rigidbody.AddForce((Random.value * 2 - 1) * 100, 500, (Random.value * 2 - 1) * 100);
		crateMaterial.SetColor("_Color", Color.white);
		crateLight.gameObject.SetActive(false);
		
		audioSource.Play ();
		
		Debug.Log ("Open");
	}
	
	Color GetRandomCrateColor()
	{
	return new Color(Random.value, Random.value, Random.value, 255);
		
	}
	void SetCrateColor(Color crateColor)
	{
		crateMaterial.SetColor("_Color", crateColor);
		crateLight.color = crateColor;
		
	}
}

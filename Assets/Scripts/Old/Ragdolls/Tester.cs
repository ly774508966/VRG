using UnityEngine;
using System.Collections;

public class Tester : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

public GameObject gun1;
public GameObject gun2;
public bool clicked = false;
	
	void OnMouseDown(){
	clicked = true;
	Debug.Break ();
		gun1.SendMessage("FireBeam");
		gun2.SendMessage("FireBeam");
	}	

}

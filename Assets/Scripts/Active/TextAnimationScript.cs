using UnityEngine;
using System.Collections;

public class TextAnimationScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void DeleteContainer() {
		gameObject.SendMessageUpwards("DisplayOver");
}
}
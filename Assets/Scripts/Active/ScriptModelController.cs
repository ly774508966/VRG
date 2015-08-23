using UnityEngine;
using System.Collections;

public class ScriptModelController : MonoBehaviour {
	
	public ScriptCharacterSheet scriptCharacterSheet;
	public GameObject rightLeg;
	public GameObject leftLeg;
	public GameObject rightArm;
	public GameObject leftArm;
	public GameObject spine;
	public GameObject head;
	public GameObject weapon;
	
	//public GameObject face;
	
	
	// Use this for initialization
	void Start () {
	
		//WeaponEffect();
		
		//ColorCharacter();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//void InitializeModel(){
	//}
	
	void ColorCharacter(){
		//Debug.Log ("ColorCharacter");
		
		//Primary coloring
		foreach(Transform child in leftArm.transform)
		{
			if(child.gameObject.GetComponent<Renderer>())
			{
			Material hotMat = child.gameObject.GetComponent<Renderer>().material;
			hotMat.color = scriptCharacterSheet.primaryColor;
			}	
		}
			foreach(Transform child in rightArm.transform)
		{
			if(child.gameObject.GetComponent<Renderer>())
			{
			Material hotMat = child.gameObject.GetComponent<Renderer>().material;
			hotMat.color = scriptCharacterSheet.primaryColor;
			}	
		}

		
				foreach(Transform child in spine.transform){
			if(child.gameObject.GetComponent<Renderer>()){
			Material hotMat = child.gameObject.GetComponent<Renderer>().material;
				//Spine is primary color except for hips (spinebox6)
				if(child.name == "spineBox6"){
			hotMat.color = scriptCharacterSheet.secondaryColor;
				} else {
						hotMat.color = scriptCharacterSheet.primaryColor;
				}
			}	
		}
		
		//Secondary coloring
			
				foreach(Transform child in rightLeg.transform){
			if(child.gameObject.GetComponent<Renderer>()){
			Material hotMat = child.gameObject.GetComponent<Renderer>().material;
			hotMat.color = scriptCharacterSheet.secondaryColor;
			}	
		}
				foreach(Transform child in leftLeg.transform){
			if(child.gameObject.GetComponent<Renderer>()){
			Material hotMat = child.gameObject.GetComponent<Renderer>().material;
			hotMat.color = scriptCharacterSheet.secondaryColor;
			}	
		}
		//Skin coloring
		
				foreach(Transform child in head.transform){
			if(child.gameObject.GetComponent<Renderer>()){
			Material hotMat = child.gameObject.GetComponent<Renderer>().material;
			hotMat.color = scriptCharacterSheet.skinColor;
			}	
		}
		
		
	}
	
	//void WeaponEffect(){
	//weapon.SendMessage("GunshotEffect");
	//}
	
	
}

using UnityEngine;
using System.Collections;

public class ScriptModelController : MonoBehaviour
{
	
	private CharacterSheet _characterSheet;
	CharacterSheet CharacterSheet { 
		get {
			if (_characterSheet == null) {
				_characterSheet = Utility.GetParentCharacterRecursively (transform);
			}
			return _characterSheet;
		}
	}

	public GameObject rightLeg;
	public GameObject leftLeg;
	public GameObject rightArm;
	public GameObject leftArm;
	public GameObject spine;
	public GameObject head;
	public GameObject weapon;
	
	//public GameObject face;

	// Use this for initialization
	void Start ()
	{
	
		_characterSheet = Utility.GetParentCharacterRecursively (transform);

		//WeaponEffect();
		
		//ColorCharacter();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	
	//void InitializeModel(){
	//}
	
	public void ColorCharacter ()
	{
		//Debug.Log ("ColorCharacter");
		
		//Primary coloring
		foreach (Transform child in leftArm.transform) {
			if (child.gameObject.GetComponent<Renderer> ()) {
				Material hotMat = child.gameObject.GetComponent<Renderer> ().material;
				hotMat.color = CharacterSheet.primaryColor;
			}	
		}
		foreach (Transform child in rightArm.transform) {
			if (child.gameObject.GetComponent<Renderer> ()) {
				Material hotMat = child.gameObject.GetComponent<Renderer> ().material;
				hotMat.color = CharacterSheet.primaryColor;
			}	
		}

		foreach (Transform child in spine.transform) {
			if (child.gameObject.GetComponent<Renderer> ()) {
				Material hotMat = child.gameObject.GetComponent<Renderer> ().material;
				//Spine is primary color except for hips (spinebox6)
				if (child.name == "spineBox6") {
					hotMat.color = CharacterSheet.secondaryColor;
				} else {
					hotMat.color = CharacterSheet.primaryColor;
				}
			}	
		}
		
		//Secondary coloring
		foreach (Transform child in rightLeg.transform) {
			if (child.gameObject.GetComponent<Renderer> ()) {
				Material hotMat = child.gameObject.GetComponent<Renderer> ().material;
				hotMat.color = CharacterSheet.secondaryColor;
			}	
		}
		foreach (Transform child in leftLeg.transform) {
			if (child.gameObject.GetComponent<Renderer> ()) {
				Material hotMat = child.gameObject.GetComponent<Renderer> ().material;
				hotMat.color = CharacterSheet.secondaryColor;
			}	
		}

		//Skin coloring
		foreach (Transform child in head.transform) {
			if (child.gameObject.GetComponent<Renderer> ()) {
				Material hotMat = child.gameObject.GetComponent<Renderer> ().material;
				hotMat.color = CharacterSheet.skinColor;
			}	
		}
	}
}

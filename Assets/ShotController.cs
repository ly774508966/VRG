using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShotController : MonoBehaviour{

	public ScriptCharacterSheet playerOneCharacter;

//	public void OnPointerDown(PointerEventData eventData) {
//		
//	}

	void Start() {
		ScriptGameMaster.Instance.GiveCharacterItem (playerOneCharacter, ScriptGameMaster.Instance.CreateRandomItem ());
	}

	void Update() {
		if (Input.GetMouseButtonDown (0)) {
//			print ("mouse down");
//			print ("pointer down");
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out hit)) {
				Transform objectHit = hit.transform;
				
				print ("Tapped: " + objectHit.gameObject.name + " @"+Time.time);

				PlayerShotInfo playerShotInfo = new PlayerShotInfo(playerOneCharacter);
				playerShotInfo.shotLocation = hit.collider.gameObject;
				playerShotInfo.target = GetParentCharacterRecursively(playerShotInfo.shotLocation.transform);
//				playerShotInfo.target = playerShotInfo.shotLocation.transform.parent.parent.GetComponent<ScriptCharacterSheet>();
				
				playerShotInfo.shotRay = ray;
				playerShotInfo.shotLocation = hit.transform.gameObject;
				ScriptGameMaster.Instance.ExecuteAction(playerShotInfo);
			}
		}
	}

	private ScriptCharacterSheet GetParentCharacterRecursively(Transform child) {
		ScriptCharacterSheet characterSheet = child.parent.GetComponent<ScriptCharacterSheet>();
		if (characterSheet != null) {
			return characterSheet;
		} else {
			if(child.parent == child.root) {
				return null;
			} else {
				return GetParentCharacterRecursively(child.parent);
			}
		}
	}
}

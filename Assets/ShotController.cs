using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShotController : MonoBehaviour{

	public CharacterSheet playerOneCharacter;

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
				playerShotInfo.target = Utility.GetParentCharacterRecursively(playerShotInfo.shotLocation.transform);
//				playerShotInfo.target = playerShotInfo.shotLocation.transform.parent.parent.GetComponent<ScriptCharacterSheet>();
				
				playerShotInfo.shotRay = ray;
				playerShotInfo.shotLocation = hit.transform.gameObject;
				ScriptGameMaster.Instance.ExecuteAction(playerShotInfo);
			}
		}
	}


}

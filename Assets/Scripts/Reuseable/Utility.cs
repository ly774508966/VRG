using UnityEngine;
using System.Collections;

public class Utility  {

	public static CharacterSheet GetParentCharacterRecursively(Transform child) {
		CharacterSheet characterSheet = child.parent.GetComponent<CharacterSheet>();
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

using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class EntityLink {
	static Dictionary<GameObject, Entity> goToEntity = new Dictionary<GameObject, Entity> ();

	public static Entity GetEntity(GameObject go){
		if (goToEntity.ContainsKey (go)) {
			if (goToEntity [go] != null) {
				return goToEntity [go];
			} else {
				goToEntity.Remove (go);
			}
		}
		return null;
	}

	public static void AddLink(GameObject go, Entity e){
		goToEntity.Add (go, e);
	}

	public static bool RemoveLink(GameObject go){
		if (goToEntity.ContainsKey (go)) {
			goToEntity.Remove (go);
			return true;
		} else {
			return false;
		}
	}
}

using UnityEngine;
using System.Collections;

public static class GameObjectExtension {

	public static Vector3 SliderOffset(this GameObject go, bool isUp){
		var rend = go.GetComponent<Collider> ();
		if(rend == null){
			return Vector3.up * (isUp ? 1 : -1);
		}else{
			var v = isUp ? new Vector3 (0f, rend.bounds.size.y + rend.bounds.extents.x, 0f) : new Vector3 (0f, rend.bounds.extents.x * -1, 0f);
			return v;
		}
	}

	public static Vector3 BotToCenterOffset(this GameObject go){
		var rend = go.GetComponent<Collider> ();
		if(rend == null){
			return Vector3.zero;
		}else{
			return new Vector3(0f, rend.bounds.max.y/2, 0f);
		}
	}
}

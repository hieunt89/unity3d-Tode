using UnityEngine;
using System.Collections;

public static class GameObjectExtension {

	public static Vector3 GetRendererOffset(this GameObject go, bool isUp){
		var rend = go.GetComponent<Renderer> ();
		if(rend == null){
			return Vector3.up * (isUp ? 1 : -1);
		}else{
			return new Vector3 (0f, rend.bounds.extents.y * (isUp ? 1 : -1), 0f);
		}
	}

}

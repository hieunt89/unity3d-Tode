using UnityEngine;
using System.Collections;

public static class GameObjectExtension {

	public static Vector3 GetRendererOffset(this GameObject go, bool isUp){
		var rend = go.GetComponent<Collider> ();
		if(rend == null){
//			Debug.Log ("collider null");
			return Vector3.up * (isUp ? 1 : -1);
		}else{
			var v = isUp ? new Vector3 (0f, rend.bounds.size.y + rend.bounds.extents.x, 0f) : new Vector3 (0f, rend.bounds.extents.x * -1, 0f);
//			Debug.DrawLine (go.transform.position,  go.transform.position + v, Color.red, Mathf.Infinity);
			return v;
		}
	}

}

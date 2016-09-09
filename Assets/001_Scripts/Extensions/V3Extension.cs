using UnityEngine;

public static class V3Extension {

	public static bool IsInRange(this Vector3 target, Vector3 origin, float range){
		if ( Vector3.Distance (target, origin) < range ) {
			return true;
		} else {
			return false;
		}
	}

}

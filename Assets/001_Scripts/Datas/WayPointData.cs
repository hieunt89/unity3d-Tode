using UnityEngine;

[System.Serializable]
public class WayPointData {
	// TODO: neu waypoint chir co moi field vector3 thi sao khong cho luon list Vector3 vao path, tao ra waypoint data lam deo gi nhi???
	[UnityEngine.SerializeField] private Vector3 wayPointPos;

	public Vector3 WayPointPos {
		get {
			return wayPointPos;
		}
		set {
			wayPointPos = value;
		}
	}

    public WayPointData (){
    }

	public WayPointData (Vector3 _wpp){
		wayPointPos = _wpp;
	}
}

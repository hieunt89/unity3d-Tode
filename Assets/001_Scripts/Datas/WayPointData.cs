using UnityEngine;

[System.Serializable]
public class WayPointData {
	[UnityEngine.SerializeField] public string id;
    [UnityEngine.SerializeField] public GameObject wayPointGo;
    [UnityEngine.SerializeField] public Vector3 wayPointPosition;

     public WayPointData (string _id, GameObject _wpg){
        id = _id;
        wayPointGo = _wpg;
    }

    public WayPointData (string _id, GameObject _wpg, Vector3 _wpp){
        id = _id;
        wayPointGo = _wpg;
        wayPointPosition = _wpp;
    }
}

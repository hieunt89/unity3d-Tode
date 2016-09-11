using UnityEngine;

[System.Serializable]
public class WayPointData {
	[UnityEngine.SerializeField] public string id;
    [UnityEngine.SerializeField] public GameObject wayPointGo;

     public WayPointData (string _id, GameObject _wpg){
        id = _id;
        wayPointGo = _wpg;
    }
}

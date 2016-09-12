using UnityEngine;

[System.Serializable]
public class WayPointData {
    [UnityEngine.SerializeField] public GameObject wayPointGo;

    public WayPointData (){
    }
    public WayPointData (GameObject _wpg){
        wayPointGo = _wpg;
    }
}

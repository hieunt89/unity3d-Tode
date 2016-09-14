using UnityEngine;
using System.Collections;

[System.Serializable]
public class TowerPointData {
    [UnityEngine.SerializeField] public string id;
    [UnityEngine.SerializeField] public GameObject towerPointGo;

    public TowerPointData (string _id, GameObject _tpg){
        id = _id;
        towerPointGo = _tpg;
    }
}

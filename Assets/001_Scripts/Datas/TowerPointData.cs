using UnityEngine;
using System.Collections;

[System.Serializable]
public class TowerPointData {
    [UnityEngine.SerializeField] public string id;
    [UnityEngine.SerializeField] public GameObject towerPointGo;
    [UnityEngine.SerializeField] public Vector3 towerPointPosition;
    public TowerPointData (string _id, GameObject _tpg, Vector3 _tpp){
        id = _id;
        towerPointGo = _tpg;
        towerPointPosition = _tpp;
    }
}

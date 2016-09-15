using UnityEngine;

[System.Serializable]
public class TowerPointData {
    [UnityEngine.SerializeField] private string id;
    public string Id {
        get {
            return id;
        }
        set {
            id = value;
        }
    }

    [UnityEngine.SerializeField] private Vector3 towerPointPos;
     public Vector3 TowerPointPos {
        get {
            return towerPointPos;
        }
        set {
            towerPointPos = value;
        }
    }
    
    public TowerPointData (string _id, Vector3 _tpp){
        id = _id;
        towerPointPos = _tpp;
    }
}

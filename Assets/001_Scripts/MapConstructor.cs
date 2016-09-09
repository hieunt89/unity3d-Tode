using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

[System.Serializable]
public class WayPointData {
    [UnityEngine.SerializeField] public string id;
    [UnityEngine.SerializeField] public GameObject wayPointGo;
    [UnityEngine.SerializeField] public Vector3 wayPointPosition;

    public WayPointData (string _id, GameObject _wpg, Vector3 _wpp){
        id = _id;
        wayPointGo = _wpg;
        wayPointPosition = _wpp;
    }
}

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

[System.Serializable]
public class WaveData {
    [UnityEngine.SerializeField] public int id;
    [UnityEngine.SerializeField] public List<WaveGroup> groups;

    public WaveData (int _id, List<WaveGroup> _groups){
        id = _id;
        groups = _groups;
    }
}

[ExecuteInEditMode]
public class MapConstructor : MonoBehaviour {

    // map id
    [UnityEngine.SerializeField] public int mapId = 1;

    public Vector3 mapPos;    
    private WayPointData wpd;
    private TowerPointData tpd;
    private WaveData wd;

    [UnityEngine.SerializeField] public List<WayPointData> wayPoints;
    [UnityEngine.SerializeField] public List<TowerPointData> towerPoints;    
    [UnityEngine.SerializeField] public List<WaveData> waves;
    [UnityEngine.SerializeField] public List<WaveGroup> waveGroups;  // test

    private Transform mTransform;

    #region Mono
    void Awake () {
        mTransform = this.transform;
    }

    #endregion Mono
    public void CreateNewWayPoint (int _id) {
        GameObject wpg = new GameObject("WP_" + (_id+1));
        wpg.transform.position = new Vector3 ((float)_id, 0f , 0f);
        wpg.transform.SetParent (mTransform, false);
        IconManager.SetIcon (wpg, IconManager.LabelIcon.Yellow);
        
        
        // set way point data then add it to way point list
        wayPoints.Add (new WayPointData(wpg.name, wpg, wpg.transform.position));  
    }

    public void ClearAllWayPoints () {
        for (int i = 0; i < wayPoints.Count; i++)
        {
            DestroyImmediate(wayPoints[i].wayPointGo);
        }
        wayPoints.Clear ();
    }

    public void CreateNewTowerPoint (int _id) {
        GameObject tpg = new GameObject("TP_" + (_id+1));
        tpg.transform.position = new Vector3 ((float)_id, 0f, 1f);
        tpg.transform.SetParent (mTransform, false);
        IconManager.SetIcon (tpg, IconManager.LabelIcon.Blue);

        // set tower point data then add it to tower point list 
        towerPoints.Add (new TowerPointData(tpg.name, tpg, tpg.transform.position));  
    }

    public void ClearAllTowerPoints (){
        for (int i = 0; i < towerPoints.Count; i++)
        {
            DestroyImmediate(towerPoints[i].towerPointGo);
        }
        towerPoints.Clear();
    }

    public void CreateNewWave(int _id){
        waves.Add(new WaveData(_id + 1, new List<WaveGroup>()));
    }

    public void ClearAllWaves (){
        waves.Clear();
    }
    public string Save () {
        var sb = new StringBuilder ();
        for (int i = 0; i < waveGroups.Count; i++)
        {
            sb.Append (waveGroups[i].ToString());
            if (i < waveGroups.Count - 1)
            {
                sb.Append("; ");
            }
        }
        return sb.ToString();
    }

    public void Load (string data) {
        var mGroup = data.Split (';');
        waveGroups = new List<WaveGroup> ();
        for (int i = 0; i < mGroup.Length; i++)
        {
            var values = mGroup[i].Split(',');
            var waveGroup = new WaveGroup();
            waveGroup.Type = (EnemyType) System.Enum.Parse(typeof(EnemyType), values[0]);   // parse string to enum ...
            waveGroup.Amount = Int32.Parse(values[1]);
            waveGroup.SpawnInterval = float.Parse(values[2]);
            waveGroup.WaveDelay = float.Parse(values[3]);
            waveGroups.Add(waveGroup);
        }
    }
    public void Update () {

        // transform.position = mapPos;

        // if (wayPoints.Count > 0) { 
        //     for (int i = 0; i < wayPoints.Count; i++)
        //     {
        //         // wayPoints[i].wayPointGo.transform.position = wayPoints[i].wayPointPosition;
        //     }
        // }

        // if (towerPoints.Count > 0) {
        //     for (int i = 0; i < towerPoints.Count; i++)
        //     {
        //         towerPoints[i].towerPointGo.transform.position = towerPoints[i].towerPointPosition;
        //     }
        // }
    }

    public void Reset (){
        this.mapId = 1;

        ClearAllWayPoints();
        ClearAllTowerPoints();
        ClearAllWaves();
    }
}

using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

[System.Serializable]
public struct WayPointData {
    [UnityEngine.SerializeField] public string id;
    [UnityEngine.SerializeField] public GameObject wayPointGo;
    [UnityEngine.SerializeField] public Vector3 wayPointPosition;
}

[System.Serializable]
public struct TowerPointData {
    [UnityEngine.SerializeField] public string id;
    [UnityEngine.SerializeField] public GameObject TowerPointGo;
    [UnityEngine.SerializeField] public Vector3 towerPointPosition;
}

[System.Serializable]
public struct WaveData {
    [UnityEngine.SerializeField] public int id;
    [UnityEngine.SerializeField] public List<WaveGroup> groups;
}

[ExecuteInEditMode]
public class MapConstructor : MonoBehaviour {

    // map id
    [UnityEngine.SerializeField] public int mapId = 1;
    private WayPointData wpd;
    private TowerPointData tpd;
    private WaveData wd;

    [UnityEngine.SerializeField] public List<WayPointData> wayPoints;
    [UnityEngine.SerializeField] public List<TowerPointData> towerPoints;    
    [UnityEngine.SerializeField] public List<WaveData> waves;
    [UnityEngine.SerializeField] public List<WaveGroup> waveGroups;  // test

    public void CreateNewWayPoint (int _id) {
        GameObject wpg = new GameObject("WP_" + (_id+1));
        IconManager.SetIcon (wpg, IconManager.LabelIcon.Yellow);
        
        wpd.id = wpg.name;
        wpd.wayPointGo = wpg;
        wpd.wayPointPosition = wpg.transform.position;
        wayPoints.Add (wpd);  
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
        IconManager.SetIcon (tpg, IconManager.LabelIcon.Blue); 
        tpd.id = tpg.name;
        tpd.TowerPointGo = tpg;
        tpd.towerPointPosition = tpg.transform.position;
        towerPoints.Add (tpd);  
    }

    public void ClearAllTowerPoints (){
        for (int i = 0; i < towerPoints.Count; i++)
        {
            DestroyImmediate(towerPoints[i].TowerPointGo);
        }
        towerPoints.Clear();
    }

    public void CreateNewWave(int _id){
        wd.id = _id + 1;
        wd.groups = new List<WaveGroup> ();
        waves.Add(wd);
    }

    public void ClearAllWaves (){
        // for (int i = 0; i < waves.Count; i++)
        // {
        //     DestroyImmediate(waves[i]);
        // }
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
    void Update () {
        for (int i = 0; i < wayPoints.Count; i++)
        {
            wayPoints[i].wayPointGo.transform.position = wayPoints[i].wayPointPosition;
        }

        for (int i = 0; i < towerPoints.Count; i++)
        {
            towerPoints[i].TowerPointGo.transform.position = towerPoints[i].towerPointPosition;
        }
    }
}

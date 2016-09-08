using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;


public class MapConstructor : MonoBehaviour {

    // map id
    public int mapId = 1;

    // way point
    [System.Serializable]
    public class WayPointData {
        public string id;
        public GameObject wayPointGo;
        public Vector3 wayPointPosition;

        public WayPointData (string _id, GameObject _wpg, Vector3 _wpp){
            id = _id;
            wayPointGo = _wpg;
            wayPointPosition = _wpp;
        }
    }
    public List<WayPointData> wayPoints;

    // tower point
    [System.Serializable]
    public class TowerPointData {
        public string id;
        public Vector3 towerPointPosition;

        public TowerPointData (string _id, Vector3 _tpp){
            id = _id;
            towerPointPosition = _tpp;
        }
    }

    public List<TowerPointData> towerPoints;

    // wave
    public List<Wave> waves;
    // wave group 
    public List<WaveGroup> waveGroups;

    // TODO: SAVE AND LOAD MAP DATA
    public void CreateWayPoint (int _id) {
        GameObject wpg = new GameObject("WP_" + (_id+1));
        IconManager.SetIcon (wpg, IconManager.LabelIcon.Yellow);

        wayPoints.Add (new WayPointData(wpg.name, wpg, wpg.transform.position));  
    }

    public void ClearWayPoints () {
        wayPoints.Clear ();
    }

    public void CreateTowerPoint (int _id) {
        GameObject tp = new GameObject("TP_" + (_id+1));
        IconManager.SetIcon (tp, IconManager.Icon.DiamondBlue);   
        // towerPoints.Add (tp.transform.position);  
    }

    public void ClearTowerPoints (){
        towerPoints.Clear();
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
            waveGroup.type = (EnemyType) System.Enum.Parse(typeof(EnemyType), values[0]);   // parse string to enum ...
            waveGroup.amount = Int32.Parse(values[1]);
            waveGroup.spawnInterval = float.Parse(values[2]);
            waveGroup.waveDelay = float.Parse(values[3]);
            waveGroups.Add(waveGroup);
        }
    }
}

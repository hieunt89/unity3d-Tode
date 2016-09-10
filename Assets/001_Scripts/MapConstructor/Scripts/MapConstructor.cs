using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

[ExecuteInEditMode]
public class MapConstructor : MonoBehaviour {

    // map id
    [UnityEngine.SerializeField] public int mapId = 1;

    public Color pointColor = Color.magenta;
    public float pointSize = 1f;
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
            waveGroup.EnemyId = values[0];   // parse string to enum ...
            waveGroup.Amount = Int32.Parse(values[1]);
            waveGroup.SpawnInterval = float.Parse(values[2]);
            waveGroup.WaveDelay = float.Parse(values[3]);
            waveGroups.Add(waveGroup);
        }
    }
    public void Update () {
       
    }

    public void Reset (){
        this.mapId = 1;

        ClearAllWayPoints();
        ClearAllTowerPoints();
        ClearAllWaves();
    }
}

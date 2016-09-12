using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

[ExecuteInEditMode]
public class MapConstructor : MonoBehaviour {

    // map id
    [SerializeField] public int mapId = 1;
    [SerializeField] public float pointSize = 1f;
    [SerializeField] public float maxPointSize = 2f;
    [SerializeField] public Color baseColor = Color.gray;
    [SerializeField] public Color pathColor = Color.gray;
    [SerializeField] public Color wayPointColor = Color.gray;
    [SerializeField] public Color towerPointColor = Color.gray;

    [SerializeField] public List<PathData> paths;
    // [UnityEngine.SerializeField] public List<WayPointData> wayPoints;
    [SerializeField] public List<TowerPointData> towerPoints;    
    [SerializeField] public List<WaveData> waves;
    [SerializeField] public List<WaveGroup> waveGroups;  // test

    private Transform mTransform;

    #region Mono
    void Awake () {
        mTransform = this.transform;
    }
    #endregion Mono

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
}

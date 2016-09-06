using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

public class MapConstructor : MonoBehaviour {

    // map id
    public int mapId = 1;
    // public int MapId
    // {
    //     get
    //     {
    //         return mapId;
    //     }

    //     set
    //     {
    //         mapId = value;
    //     }
    // }

    // wave datas
    public List<WaveGroup> waveGroups;
      
    // tower point 
    public List<Vector3> towerPoints;
      
    // way point
    public List<Vector3> wayPoints;


    // TODO: SAVE AND LOAD MAP DATA
    public void CreateTowerPoint (int _id) {
        GameObject tp = new GameObject("TP_" + _id);
        IconManager.SetIcon (tp, IconManager.Icon.DiamondBlue);   
        towerPoints.Add (tp.transform.position);  
    }

    public void CreateWayPoint (int _id) {
        GameObject wp = new GameObject("WP_" + _id);
        IconManager.SetIcon (wp, IconManager.LabelIcon.Yellow);   
        wayPoints.Add (wp.transform.position);  
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
        // Debug.Lo
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

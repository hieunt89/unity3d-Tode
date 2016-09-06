using UnityEngine;
using System.Collections.Generic;
using System.Text;
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

    // map name
    public string mapName = "level1";
    // public string MapName
    // {
    //     get
    //     {
    //         return mapName;
    //     }

    //     set
    //     {
    //         mapName = value;
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

    public void Load () {

    }
}

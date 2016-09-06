using UnityEngine;
using System.Collections.Generic;

public class MapConstructor : MonoBehaviour {

    // map id
    public int mapId;
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
    public string mapName;
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
	
    public List <WaveGroup> waveGroups;
    
	  // tower point 
    public List<Vector3> towerPoints;
    
	  // way point
    public List<Vector3> wayPoints;

    // TODO: SAVE AND LOAD MAP DATA
    public void CreateTowerPoint (int _id) {
        // Create tower point game object
        // Change name and add icon
        // Add tower point game object to list

        GameObject tp = new GameObject("TP_" + _id);
     
        IconManager.SetIcon(tp, IconManager.LabelIcon.Blue);
        // IconManager._SetIcon(tp, Resources.Load <Texture2D> ("white_rock") as Texture2D);
    }

    public void RemoveTowerPoint () {
        
    }

    public void CreateWayPoint () {

    }
    
    public void RemoveWayPoint () {

    }

    public void SaveMap () {

    }

    public void LoadMap () {
        
    }
}

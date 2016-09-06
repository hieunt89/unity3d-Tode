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

    // wave datas
    public struct WaveData {
		EnemyType type;
		int amount;
		float interval;
	}
	public List <WaveData> waveDatas;
    
	// tower point 
	public List<Vector3> towerPoints;
    
	// way point
	public List<Vector3> wayPoints;


	// TODO: SAVE AND LOAD MAP DATA

}

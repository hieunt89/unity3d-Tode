using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System;

[ExecuteInEditMode]
[System.Serializable]
public class MapConstructor : MonoBehaviour {

    // map id
	[SerializeField] private int mapId = 1;

	public int MapId {
		get {
			return mapId;
		}
		set {
			mapId = value;
		}
	}

	[SerializeField] private float pointSize = 1f;

	public float PointSize {
		get {
			return pointSize;
		}
		set {
			pointSize = value;
		}
	}

	[SerializeField] private float maxPointSize = 2f;

	public float MaxPointSize {
		get {
			return maxPointSize;
		}
		set {
			maxPointSize = value;
		}
	}

	[SerializeField] private Color baseColor = Color.gray;

	public Color BaseColor {
		get {
			return baseColor;
		}
		set {
			baseColor = value;
		}
	}

	[SerializeField] private Color pathColor = Color.gray;

	public Color PathColor {
		get {
			return pathColor;
		}
		set {
			pathColor = value;
		}
	}

	[SerializeField] private Color wayPointColor = Color.gray;

	public Color WayPointColor {
		get {
			return wayPointColor;
		}
		set {
			wayPointColor = value;
		}
	}

	[SerializeField] private Color towerPointColor = Color.gray;

	public Color TowerPointColor {
		get {
			return towerPointColor;
		}
		set {
			towerPointColor = value;
		}
	}

	[SerializeField] private List<PathData> paths;

	public List<PathData> Paths {
		get {
			return paths;
		}
		set {
			paths = value;
		}
	}

	[SerializeField] private List<TowerPointData> towerPoints;

	public List<TowerPointData> TowerPoints {
		get {
			return towerPoints;
		}
		set {
			towerPoints = value;
		}
	}

    
	[SerializeField] private List<WaveData> waves;

	public List<WaveData> Waves {
		get {
			return waves;
		}
		set {
			waves = value;
		}
	}
	#region Mono
    #endregion Mono

    // public string Save () {
    //     var sb = new StringBuilder ();
    //     for (int i = 0; i < waveGroups.Count; i++)
    //     {
    //         sb.Append (waveGroups[i].ToString());
    //         if (i < waveGroups.Count - 1)
    //         {
    //             sb.Append("; ");
    //         }
    //     }
    //     return sb.ToString();
    // }

    // public void Load (string data) {
    //     var mGroup = data.Split (';');
    //     waveGroups = new List<WaveGroup> ();
    //     for (int i = 0; i < mGroup.Length; i++)
    //     {
    //         var values = mGroup[i].Split(',');
    //         var waveGroup = new WaveGroup();
    //         waveGroup.EnemyId = values[0];   // parse string to enum ...
    //         waveGroup.Amount = Int32.Parse(values[1]);
    //         waveGroup.SpawnInterval = float.Parse(values[2]);
    //         waveGroup.WaveDelay = float.Parse(values[3]);
    //         waveGroups.Add(waveGroup);
    //     }
    // }
}

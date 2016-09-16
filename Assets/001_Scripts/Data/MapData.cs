using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MapData {
	
	[SerializeField] public string id;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
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

	public MapData (string id, List<PathData> paths, List<TowerPointData> towerPoints, List<WaveData> waves)
	{
		this.id = id;
		this.paths = paths;
		this.towerPoints = towerPoints;
		this.waves = waves;
	}
	
}

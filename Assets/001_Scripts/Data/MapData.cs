using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MapData {
	[SerializeField] public string id;
	[SerializeField] private int initGold;
	[SerializeField] private int initLife;
	[SerializeField] private List<PathData> paths;
	[SerializeField] private List<TowerPointData> towerPoints;
	[SerializeField] private List<WaveData> waves;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public int InitGold {
		get {
			return this.initGold;
		}
		set {
			initGold = value;
		}
	}

	public int InitLife {
		get {
			return this.initLife;
		}
		set {
			initLife = value;
		}
	}

	public List<PathData> Paths {
		get {
			return paths;
		}
		set {
			paths = value;
		}
	}

	public List<TowerPointData> TowerPoints {
		get {
			return towerPoints;
		}
		set {
			towerPoints = value;
		}
	}

	public List<WaveData> Waves {
		get {
			return waves;
		}
		set {
			waves = value;
		}
	}	

	public MapData (string id, int initGold, int initLife, List<PathData> paths, List<TowerPointData> towerPoints, List<WaveData> waves)
	{
		this.id = id;
		this.initGold = initGold;
		this.initLife = initLife;
		this.paths = paths;
		this.towerPoints = towerPoints;
		this.waves = waves;
	}
	
}

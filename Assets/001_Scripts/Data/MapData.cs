using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class MapData {
	[SerializeField] public string id;
	public int initGold;
	public int initLife;
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

	public MapData (string id)
	{
		this.id = id;
		this.initGold = 0;
		this.initLife = 0;
		paths = new List<PathData> ();
		towerPoints = new List<TowerPointData> ();
		waves = new List<WaveData> ();
	}

//	public MapData (string id, int initGold, int initLife, List<PathData> paths, List<TowerPointData> towerPoints, List<WaveData> waves)
//	{
//		this.id = id;
//		this.initGold = initGold;
//		this.initLife = initLife;
//		this.paths = paths;
//		this.towerPoints = towerPoints;
//		this.waves = waves;
//	}
	
}

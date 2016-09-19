using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class WaveGroupData { // TODO: rename WaveGroupData

	// TODO: get index of enemyid string in list of enemyid (pre-defined)
//	List<string> existEnemies;
//	List<MapData> existMaps;

	[SerializeField] private string id;

	[SerializeField] private int eId;
	[SerializeField] private string enemyId;

	[SerializeField] private int amount;
	[SerializeField] private float spawnInterval;
	[SerializeField] private float waveDelay;

	[SerializeField] private int pId;
	[SerializeField] private string pathId;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public int EId {
		get {
//			existEnemies = DataManager.Instance.LoadAllData <EnemyData> ();
//
//			for (int i = 0; i < existEnemies.Count; i++) {
//				if (enemyId.Equals (existEnemies [i])) {
//					return i;
//				}
//			}
			return eId;
		}
		set {
			eId = value;
//			enemyId = existEnemies[eId];
		}
	}

	public string EnemyId {
		get {
			return enemyId;
		}
        set {
            enemyId = value;
        }
	}

	public int Amount {
		get {
			return amount;
		}
        set {
            amount = value;
        }
	}

	public float SpawnInterval {
		get {
			return spawnInterval;
		}
        set {
            spawnInterval = value;
        }
	}

	public float WaveDelay {
		get {
			return waveDelay;
		}
        set {
            waveDelay = value;
        }
	}

	public int PId {
		get {
//			existMaps = DataManager.Instance.LoadAllData <MapData> ();
//			if (existMaps.Count > 0) {
//				for (int i = 0; i < existMaps.Count; i++) {
//					for (int j = 0; j < existMaps [i].Paths.Count; j++) {
//						if (pathId.Equals (existMaps [i].Paths [j].Id)) {
//							return i;
//						}
//					}
//				}
//			}
			return pId;
		}
		set {
			pId = value;

//			existMaps = DataManager.Instance.LoadAllData <MapData> ();
//			if (existMaps.Count > 0) {
//				for (int i = 0; i < existMaps.Count; i++) {
//					
//				}
//			}
//
//			pathId = pathIdOptions [pId];
		}
	}

	public string PathId {
		get {
			return pathId;
		}
        set {
            pathId = value;
        }
	}

	public WaveGroupData (string id, string enemyId, string pathId) {
		this.id = id;
		this.enemyId = enemyId;
		this.pathId = pathId;
	}

	public WaveGroupData (string id, string enemyId, int amount, float spawnInterval, float waveDelay, string pathId)
	{
		this.id = id;
		this.enemyId = enemyId;
		this.amount = amount;
		this.spawnInterval = spawnInterval;
		this.waveDelay = waveDelay;
		this.pathId = pathId;
	}
	
}

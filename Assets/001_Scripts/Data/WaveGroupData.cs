using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class WaveGroupData { // TODO: rename WaveGroupData

	// TODO: get index of enemyid string in list of enemyid (pre-defined)
//	List<string> existEnemies;
//	List<MapData> existMaps;

	[SerializeField] private string id;

	[SerializeField] private int enemyIdIndex;
	[SerializeField] private string enemyId;

	[SerializeField] private int amount;
	[SerializeField] private float spawnInterval;
	[SerializeField] private float groupDelay;

	[SerializeField] private int pathIdIndex;
	[SerializeField] private string pathId;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public int EnemyIdIndex {
		get {
			return enemyIdIndex;
		}
		set {
			enemyIdIndex = value;
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

	public float GroupDelay {
		get {
			return groupDelay;
		}
        set {
            groupDelay = value;
        }
	}

	public int PathIdIndex {
		get {
//		
			return pathIdIndex;
		}
		set {
			pathIdIndex = value;
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

	public WaveGroupData (string id, string enemyId, int amount, float spawnInterval, float groupDelay, string pathId)
	{
		this.id = id;
		this.enemyId = enemyId;
		this.amount = amount;
		this.spawnInterval = spawnInterval;
		this.groupDelay = groupDelay;
		this.pathId = pathId;
	}
	
}

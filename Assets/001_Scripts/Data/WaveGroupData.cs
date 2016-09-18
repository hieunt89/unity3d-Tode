using UnityEngine;

[System.Serializable]
public class WaveGroupData { // TODO: rename WaveGroupData

	// TODO: get index of enemyid string in list of enemyid (pre-defined)
	string[] enemyIdOptions;
	string[] pathIdOptions;

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
			return this.id;
		}
		set {
			id = value;
		}
	}

	public int EId {
		get {
			enemyIdOptions = new string[] {"e01", "e02", "e03"};	// test
			for (int i = 0; i < enemyIdOptions.Length; i++) {
				if (enemyId.Equals (enemyIdOptions [i])) {
					return i;
				}
			}
			return 0;
		}
		set {
			eId = value;
			// TODO: something wrong here
			enemyId = enemyIdOptions[eId];
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
			pathIdOptions = new string[] {"p01", "p02", "p03"};
			for (int i = 0; i < pathIdOptions.Length; i++) {
				if (pathId.Equals (pathIdOptions [i])) {
					return i;
				}
			}
			return 0;
		}
		set {
			pId = value;
			pathId = pathIdOptions [pId];
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

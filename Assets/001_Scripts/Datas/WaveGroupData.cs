using UnityEngine;

[System.Serializable]
public class WaveGroupData : ScriptableObject { // TODO: rename WaveGroupData

	// TODO: get index of enemyid string in list of enemyid (pre-defined)
	string[] enemyIdOptions = new string[] {"e01", "e02", "e03"};	// test
	[SerializeField] private int eId;
	public int EId {
		get {
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
	
	[SerializeField] private string enemyId;
	public string EnemyId {
		get {
			return enemyId;
		}
        set {
            enemyId = value;
        }
	}

	[SerializeField] private int amount;

	public int Amount {
		get {
			return amount;
		}
        set {
            amount = value;
        }
	}

	[SerializeField] private float spawnInterval;

	public float SpawnInterval {
		get {
			return spawnInterval;
		}
        set {
            spawnInterval = value;
        }
	}

	[SerializeField] private float waveDelay;

	public float WaveDelay {
		get {
			return waveDelay;
		}
        set {
            waveDelay = value;
        }
	}

	string[] pathIdOptions = new string[] {"p01", "p02", "p03"};

	[SerializeField] private int pId;

	public int PId {
		get {
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

	[SerializeField] private string pathId;

	public string PathId {
		get {
			return pathId;
		}
        set {
            pathId = value;
        }
	}

    public WaveGroupData () {
        
    }
	public WaveGroupData (string enemyId, string pathId) {
		this.enemyId = enemyId;
		this.pathId = pathId;
	}

	public WaveGroupData (string enemyId, int amount, float spawnInterval, float waveDelay, string pathId)
	{
		this.enemyId = enemyId;
		this.amount = amount;
		this.spawnInterval = spawnInterval;
		this.waveDelay = waveDelay;
		this.pathId = pathId;
	}
	
}

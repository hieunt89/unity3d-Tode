using UnityEngine;
[System.Serializable]
public class WaveGroup {
	[SerializeField] public string enemyId;

	public string EnemyId {
		get {
			return enemyId;
		}
        set {
            enemyId = value;
        }
	}

	[SerializeField] public int amount;

	public int Amount {
		get {
			return amount;
		}
        set {
            amount = value;
        }
	}

	[SerializeField] public float spawnInterval;

	public float SpawnInterval {
		get {
			return spawnInterval;
		}
        set {
            spawnInterval = value;
        }
	}

	[SerializeField] public float waveDelay;

	[SerializeField] public float WaveDelay {
		get {
			return waveDelay;
		}
        set {
            waveDelay = value;
        }
	}

	[SerializeField] public string pathId;

	public string PathId {
		get {
			return pathId;
		}
        set {
            pathId = value;
        }
	}

    public WaveGroup () {
        
    }

	public WaveGroup (string enemyId, int amount, float spawnInterval, float waveDelay, string pathId)
	{
		this.enemyId = enemyId;
		this.amount = amount;
		this.spawnInterval = spawnInterval;
		this.waveDelay = waveDelay;
		this.pathId = pathId;
	}
	
}

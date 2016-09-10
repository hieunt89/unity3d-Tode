public class WaveGroup {
	string enemyId;

	public string EnemyId {
		get {
			return enemyId;
		}
        set {
            enemyId = value;
        }
	}

	int amount;

	public int Amount {
		get {
			return amount;
		}
        set {
            amount = value;
        }
	}

	float spawnInterval;

	public float SpawnInterval {
		get {
			return spawnInterval;
		}
        set {
            spawnInterval = value;
        }
	}

	float waveDelay;

	public float WaveDelay {
		get {
			return waveDelay;
		}
        set {
            waveDelay = value;
        }
	}

	string pathId;

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

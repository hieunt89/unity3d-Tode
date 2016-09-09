public class WaveGroup {
	string eId;

	public string EnemyId {
		get {
			return EnemyId;
		}
	}

	int amount;

	public int Amount {
		get {
			return amount;
		}
	}

	float spawnInterval;

	public float SpawnInterval {
		get {
			return spawnInterval;
		}
	}

	float waveDelay;

	public float WaveDelay {
		get {
			return waveDelay;
		}
	}

	string pathId;

	public string PathId {
		get {
			return pathId;
		}
	}

	public WaveGroup (string enemyId, int amount, float spawnInterval, float waveDelay, string pathId)
	{
		this.eId = enemyId;
		this.amount = amount;
		this.spawnInterval = spawnInterval;
		this.waveDelay = waveDelay;
		this.pathId = pathId;
	}
	
}

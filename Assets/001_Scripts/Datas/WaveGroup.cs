public class WaveGroup {
	EnemyType type;

	public EnemyType Type {
		get {
			return type;
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

	public WaveGroup(EnemyType type, int amount, float spawnInterval, float waveDelay){
		this.type = type;
		this.amount = amount;
		this.spawnInterval = spawnInterval;
		this.waveDelay = waveDelay;
	}
}

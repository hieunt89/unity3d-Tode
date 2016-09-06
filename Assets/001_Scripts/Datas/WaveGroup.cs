public class WaveGroup {
	EnemyClass eClass;

	public EnemyClass EClass {
		get {
			return eClass;
		}
	}

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

	string pathId;

	public string PathId {
		get {
			return pathId;
		}
	}

	public WaveGroup(EnemyClass eClass, EnemyType type, int amount, float spawnInterval, float waveDelay, string pathId){
		this.eClass = eClass;
		this.type = type;
		this.amount = amount;
		this.spawnInterval = spawnInterval;
		this.waveDelay = waveDelay;
		this.pathId = pathId;
	}
}

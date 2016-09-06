[System.SerializableAttribute]
public class WaveGroup {
	// TODO: tam thoi public field de lam editor, sau nay se dung custom editor de thay the
	public EnemyType type;

	public EnemyType Type {
		get {
			return type;
		}
	}

	public int amount;

	public int Amount {
		get {
			return amount;
		}
	}

	public float spawnInterval;
	public float SpawnInterval {
		get {
			return spawnInterval;
		}
	}

	public float waveDelay;
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

	public override string ToString () {
		return type + "," + amount + "," + spawnInterval + "," + waveDelay;
	}
}

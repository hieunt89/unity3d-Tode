public class WaveData {
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

	float interval;

	public float Interval {
		get {
			return interval;
		}
	}

	float delay;

	public float Delay {
		get {
			return delay;
		}
	}

	public WaveData(EnemyType type, int amount, float interval, float delay){
		this.type = type;
		this.amount = amount;
		this.interval = interval;
		this.delay = delay;
	}
}

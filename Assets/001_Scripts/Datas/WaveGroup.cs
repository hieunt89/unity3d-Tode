[System.Serializable]

public class WaveGroup {
	// TODO: tam thoi public field de lam editor, sau nay se dung custom editor de thay the
	[UnityEngine.SerializeField] private EnemyType type;
	public EnemyType Type
    {
        get
        {
            return type;
        }

        set
        {
            type = value;
        }
    }

    [UnityEngine.SerializeField] private int pathId;
	public int PathId
    {
        get
        {
            return pathId;
        }

        set
        {
            pathId = value;
        }
    }

    [UnityEngine.SerializeField] private int amount;
  	public int Amount
    {
        get
        {
            return amount;
        }

        set
        {
            amount = value;
        }
    }
	
	[UnityEngine.SerializeField] private float spawnInterval;
 	public float SpawnInterval
    {
        get
        {
            return spawnInterval;
        }

        set
        {
            spawnInterval = value;
        }
    }

    [UnityEngine.SerializeField] private float waveDelay;
	public float WaveDelay
    {
        get
        {
            return waveDelay;
        }

        set
        {
            waveDelay = value;
        }
    }
	
    public WaveGroup() {
	}

	public WaveGroup(EnemyType type, int amount, float spawnInterval, float waveDelay){
		this.Type = type;
		this.Amount = amount;
		this.SpawnInterval = spawnInterval;
		this.WaveDelay = waveDelay;
	}

	public override string ToString () {
		return Type + "," + Amount + "," + SpawnInterval + "," + WaveDelay;
	}
}

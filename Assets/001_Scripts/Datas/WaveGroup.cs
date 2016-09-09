public class WaveGroup {
	EnemyClass eClass;

	public EnemyClass EClass {
		get {
			return eClass;
		}
	}

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

    [UnityEngine.SerializeField] private string pathId;
	public string PathId
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
    
    public override string ToString () {
		return Type + "," + Amount + "," + SpawnInterval + "," + WaveDelay;
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

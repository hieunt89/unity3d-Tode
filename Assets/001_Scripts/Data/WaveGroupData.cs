using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class WaveGroupData {
	[SerializeField] private string id;

	[SerializeField] private string enemyId;

	[SerializeField] private int amount;
	[SerializeField] private float spawnInterval;
	[SerializeField] private float groupDelay;

	[SerializeField] private string pathId;

	public string Id {
		get {
			return id;
		}
		set {
			id = value;
		}
	}

	public string EnemyId {
		get {
			return enemyId;
		}
        set {
            enemyId = value;
        }
	}

	public int Amount {
		get {
			return amount;
		}
        set {
            amount = value;
        }
	}

	public float SpawnInterval {
		get {
			return spawnInterval;
		}
        set {
            spawnInterval = value;
        }
	}

	public float GroupDelay {
		get {
			return groupDelay;
		}
        set {
            groupDelay = value;
        }
	}

	public string PathId {
		get {
			return pathId;
		}
        set {
            pathId = value;
        }
	}

	public WaveGroupData (string id, string enemyId, string pathId) {
		this.id = id;
		this.enemyId = enemyId;
		this.pathId = pathId;
	}

	public WaveGroupData (string id, string enemyId, int amount, float spawnInterval, float groupDelay, string pathId)
	{
		this.id = id;
		this.enemyId = enemyId;
		this.amount = amount;
		this.spawnInterval = spawnInterval;
		this.groupDelay = groupDelay;
		this.pathId = pathId;
	}
	
}

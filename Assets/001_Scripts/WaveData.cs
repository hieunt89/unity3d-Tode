using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

	public WaveData(EnemyType t, int a, float i){
		type = t;
		amount = a;
		interval = i;
	}
}

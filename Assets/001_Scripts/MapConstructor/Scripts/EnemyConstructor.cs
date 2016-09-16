using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyConstructor : MonoBehaviour {

	public EnemyData enemy;
	public EnemyData Enemy {
		get {
			return this.enemy;
		}
		set {
			enemy = value;
		}
	}
}

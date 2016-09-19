using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyConstructor : MonoBehaviour {

	[SerializeField] private  EnemyData enemy;
	public EnemyData Enemy {
		get {
			return enemy;
		}
		set {
			enemy = value;
		}
	}
}

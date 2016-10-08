using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class EnemyConstructor : MonoBehaviour {

	[SerializeField] private  CharacterData enemy;
	public CharacterData Enemy {
		get {
			return enemy;
		}
		set {
			enemy = value;
		}
	}
}

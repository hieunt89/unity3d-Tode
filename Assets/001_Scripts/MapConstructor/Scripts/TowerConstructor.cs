using System.Collections.Generic;
using UnityEngine;

public class TowerConstructor : MonoBehaviour {

	public TowerData tower;

	public TowerData Tower {
		get {
			return this.tower;
		}
		set {
			tower = value;
		}
	}
}

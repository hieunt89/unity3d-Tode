using UnityEngine;
using System.Collections;
using Entitas;

public static class EntityExtension {

	public static Entity BeDamaged(this Entity e, int damage){
		int hpLeft = Mathf.Clamp(e.hp.value - damage, 0, e.hpTotal.value);
		e.ReplaceHp (hpLeft);
		return e;
	}

}

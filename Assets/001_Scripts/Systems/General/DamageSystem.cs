using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class DamageSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf (Matcher.Hp, Matcher.HpTotal);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			e.BeDamaged (e.damage.value).RemoveDamage ();
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Damage.OnEntityAdded ();
		}
	}

	#endregion
}

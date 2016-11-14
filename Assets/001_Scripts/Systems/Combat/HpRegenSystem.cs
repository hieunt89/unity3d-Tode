using Entitas;
using UnityEngine;

public class HpRegenSystem : ISetPool, IReactiveSystem {
	Pool _pool;
	Group _group;

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_group = _pool.GetGroup (Matcher.AllOf (Matcher.Wound, Matcher.HpRegen));
	}

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_group.count == 0)
			return;

		var tick = entities.SingleEntity ();
		var _entities = _group.GetEntities ();
		for (int i = 0; i < _entities.Length; i++) {
			var e = _entities [i];

			if (e.hpRegen.duration <= 0) {
				var newValue = e.hp.value + (e.hpTotal.value * Mathf.Clamp01 (e.hpRegen.value));
				e.ReplaceHp (Mathf.Clamp (Mathf.CeilToInt (newValue), 0, e.hpTotal.value));

				e.hpRegen.duration = e.hpRegen.interval;
			} else {
				e.hpRegen.duration -= tick.tick.change;
			}

		}
	}

	public TriggerOnEvent trigger {
		get {
			return Matcher.Tick.OnEntityAdded ();
		}
	}
}

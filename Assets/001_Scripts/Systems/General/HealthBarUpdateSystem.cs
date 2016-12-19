using UnityEngine;
using System.Collections;
using Entitas;
public class HealthBarUpdateSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupHasHP;
	public void SetPool (Pool pool)
	{
		_groupHasHP = pool.GetGroup (Matcher.AllOf (Matcher.Hp, Matcher.HpTotal, Matcher.Position, Matcher.ViewSlider, Matcher.Active));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupHasHP.count <= 0) {
			return;
		}

		var ens = _groupHasHP.GetEntities ();

		for (int i = 0; i < ens.Length; i++) {
			var e = ens [i];

			e.viewSlider.bar.transform.position = Camera.main.WorldToScreenPoint (e.position.value + e.viewSlider.offset);
			e.viewSlider.bar.value = Mathf.Clamp01((float)e.hp.value / (float)e.hpTotal.value);
		}

	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Tick.OnEntityAdded ();
		}
	}
	#endregion
	
}

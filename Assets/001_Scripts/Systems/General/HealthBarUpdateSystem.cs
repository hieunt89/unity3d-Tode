using UnityEngine;
using System.Collections;
using Entitas;
public class HealthBarUpdateSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf (Matcher.ViewSlider, Matcher.Active);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			e.viewSlider.bar.transform.position = Camera.main.WorldToScreenPoint (e.position.value + e.viewSlider.offset);
			e.viewSlider.bar.value = Mathf.Clamp01((float)e.hp.value / (float)e.hpTotal.value);
		}

	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AnyOf (Matcher.Hp, Matcher.HpTotal, Matcher.Position).OnEntityAdded ();
		}
	}
	#endregion
	
}

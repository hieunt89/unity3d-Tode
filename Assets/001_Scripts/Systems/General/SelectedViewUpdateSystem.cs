using UnityEngine;
using System.Collections;
using Entitas;

public class SelectedViewUpdateSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf(Matcher.Selected, Matcher.ViewSelected);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			e.viewSelected.indicator.transform.position = e.position.value;
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.Position).OnEntityAdded();
		}
	}
	#endregion
}

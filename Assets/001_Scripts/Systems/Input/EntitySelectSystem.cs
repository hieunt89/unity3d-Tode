using UnityEngine;
using System.Collections;
using Entitas;
public class EntitySelectSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		var e = entities.SingleEntity ().currentSelected.e;

	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.CurrentSelected.OnEntityAdded ();
		}
	}
	#endregion
}

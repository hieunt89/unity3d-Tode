using UnityEngine;
using System.Collections;
using Entitas;
public class EntitySelectSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		var e = entities.SingleEntity ().currentSelected.e;

		if (e != null) {
			if (e.hasTower || e.isTowerBase || e.isTowerUpgrading) {
				Messenger.Broadcast<Entity> (Events.Input.TOWER_SELECT, e);
			}
		} else {
			Messenger.Broadcast (Events.Input.EMPTY_SELECT);
		}
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

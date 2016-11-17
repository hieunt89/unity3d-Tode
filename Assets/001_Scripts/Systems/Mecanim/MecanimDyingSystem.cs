using UnityEngine;
using System.Collections;
using Entitas;

public class MecanimDyingSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.ViewAnims;
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			var anims = e.viewAnims.anims;

			for (int j = 0; j < anims.Length; j++) {
				anims [j].Play (AnimState.Die, AnimLayer.Base, e.dying.timeSpent / e.dyingTime.value);
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Dying.OnEntityAdded ();
		}
	}

	#endregion
}

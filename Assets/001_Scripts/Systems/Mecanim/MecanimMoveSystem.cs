using UnityEngine;
using System.Collections;
using Entitas;
public class MecanimMoveSystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.View;
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (e.view.Anim != null) {
				var anims = e.view.Anim;
				if (e.isMovable) {
					for (int j = 0; j < anims.Length; j++) {
						if (!anims[j].GetCurrentAnimatorStateInfo(0).IsName(AnimState.Move)) {
							anims[j].Play (AnimState.Move, 0);
						}
					}
				} else {
					for (int j = 0; j < anims.Length; j++) {
						if (!anims [j].GetCurrentAnimatorStateInfo (0).IsName (AnimState.Idle)) {
							anims [j].Play (AnimState.Idle);
						}
					}
				}
			}
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Movable.OnEntityAddedOrRemoved ();
		}
	}
	#endregion
}

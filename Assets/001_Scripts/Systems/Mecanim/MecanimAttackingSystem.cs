using UnityEngine;
using System.Collections;
using Entitas;
public class MecanimAttackingSystem : IReactiveSystem, IEnsureComponents {
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
				if (e.hasAttacking) {
					for (int j = 0; j < anims.Length; j++) {
						anims [j].Play (e.attackingParams.stateToPlay, 0, e.attacking.spentTime / e.attackingParams.duration);
					}
				} 
//				else {
//					for (int j = 0; j < anims.Length; j++) {
//						anims [j].Play (AnimState.Idle);
//					}
//				}
			}
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Attacking.OnEntityAddedOrRemoved ();
		}
	}
	#endregion
	
}

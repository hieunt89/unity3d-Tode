using UnityEngine;
using System.Collections;
using Entitas;
public class MecanimAttackingSystem : IReactiveSystem, IEnsureComponents {
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
		
			if (e.hasAttacking) {
				for (int j = 0; j < anims.Length; j++) {
					anims [j].Play (e.attackingParams.state, AnimLayer.Base, e.attacking.timeSpent/e.attackingParams.duration);
				}
			} else {
				for (int j = 0; j < anims.Length; j++) {
					if (!anims [j].GetCurrentAnimatorStateInfo (AnimLayer.Base).IsName (AnimState.Idle)) {
						anims [j].CrossFade (AnimState.Idle, e.attackingParams.duration/10);
					}
				}
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

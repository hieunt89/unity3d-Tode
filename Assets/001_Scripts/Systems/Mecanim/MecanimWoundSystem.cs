using UnityEngine;
using System.Collections;
using Entitas;

public class MecanimWoundSystem : IReactiveSystem, IEnsureComponents {
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
				for (int j = 0; j < anims.Length; j++) {
					if (!anims[j].GetCurrentAnimatorStateInfo(1).IsName(AnimState.Wound)) {
						anims [j].SetTrigger (AnimTrigger.Wound);
					}
				}
			}

			e.IsWounded (false);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Wounded.OnEntityAdded ();
		}
	}

	#endregion


}

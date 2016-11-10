using UnityEngine;
using System.Collections;
using Entitas;
public class AnimChangeSystem : IReactiveSystem, IEnsureComponents {
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
					anims [j].Play (e.animChange.state, AnimLayer.Combat, 0f);
				}
			}

			e.RemoveAnimChange ();
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AnimChange.OnEntityAdded ();
		}
	}
	#endregion
	
}

using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class SkillEffectWatcherInitSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var target = entities [i];
			if (!target.hasSkillEffectWatcherList) {
				target.AddSkillEffectWatcherList (new List<Entity> ());
			}

			var watchers = target.skillEffectWatcherList.watchers;

			for (int j = 0; j < target.skillEffects.effects.Count; j++) {
				var newEf = target.skillEffects.effects [j];
				var foundDuplicate = false;

				for (int k = 0; k < watchers.Count; k++) {
					var oldEf = watchers [k].skillEffectWatcher.effect;

					if (string.Equals (newEf.skillId, oldEf.skillId) && newEf.effectType == oldEf.effectType) {
						watchers [k].ReplaceDuration (newEf.duration);
						foundDuplicate = true;
						break;
					}
				}

				if (!foundDuplicate) {
					watchers.Add (_pool.CreateEntity ().AddSkillEffectWatcher (target, newEf));
				}
			}

			target.RemoveSkillEffects ();
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.SkillEffects.OnEntityAdded ();
		}
	}
	#endregion
}

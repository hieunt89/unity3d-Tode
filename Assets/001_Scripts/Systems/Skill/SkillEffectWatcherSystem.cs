using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class SkillEffectWatcherSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupEfWatchers;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupEfWatchers = _pool.GetGroup (Matcher.AllOf(Matcher.SkillEffectWatcher));
	}
	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupEfWatchers.count <= 0) {
			return;
		}

		var watchers = _groupEfWatchers.GetEntities ();
		for (int i = 0; i < watchers.Length; i++) {
			var watcher = watchers [i];

			if (!watcher.hasDuration) {
				watcher.AddDuration (watcher.skillEffectWatcher.effect.duration);
				ProcessEffect (watcher.skillEffectWatcher, watcher.skillEffectWatcher.target, true);
			} else {
				watcher.ReplaceDuration (watcher.duration.value -= Time.deltaTime);
			}

			if (watcher.duration.value <= 0) {
				ProcessEffect (watcher.skillEffectWatcher, watcher.skillEffectWatcher.target, false);
				watcher.IsMarkedForDestroy (true);
				return;
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Tick.OnEntityAdded ();
		}
	}

	#endregion

	void ProcessEffect(SkillEffectWatcher watcher, Entity target, bool isApplying){
		switch (watcher.effect.effectType) {
		case EffectType.PhysicArmorReduce:
			ProcessPhysicArmor (watcher, target, isApplying);
			break;
		case EffectType.MoveSpeedSlow:
			ProcessMovmentSpeed (watcher, target, isApplying);
			break;
		case EffectType.Root:
			break;
		case EffectType.Stun:
			break;
		default:
			break;
		}
	}

	void ProcessPhysicArmor(SkillEffectWatcher watcher, Entity target, bool isApplying){
		if (target.hasArmor) {
			List<ArmorData> armors = target.armor.armorList;
			for (int i = 0; i < armors.Count; i++) {
				if (armors[i].Type == AttackType.physical) {
					if (isApplying) {
						watcher.Changes = armors [i].Reduction;
						armors [i].Reduction = armors [i].Reduction * watcher.effect.value / 100;
						watcher.Changes = watcher.Changes / armors [i].Reduction;
						target.ReplaceArmor (armors);
					} else {
						armors [i].Reduction = watcher.Changes * armors [i].Reduction;
						target.ReplaceArmor (armors);
					}
				}
			}
		}
	}

	void ProcessMovmentSpeed(SkillEffectWatcher watcher, Entity target, bool isApplying){
		if (target.hasMovable) {
			float speed = target.moveSpeed.speed;
			if (isApplying) {
				watcher.Changes = speed;
				speed = speed * watcher.effect.value / 100;
				watcher.Changes = watcher.Changes / speed;
				target.ReplaceMoveSpeed (speed);
			} else {
				speed = watcher.Changes * speed;
				target.ReplaceMoveSpeed (speed);
			}
		}
	}

	void ProcessRoot(SkillEffectWatcher watcher, Entity target, bool isApplying){
		
	}
}

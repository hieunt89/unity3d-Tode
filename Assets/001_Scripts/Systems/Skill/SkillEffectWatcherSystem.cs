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

			if (!watcher.skillEffectWatcher.target.hasSkillWatcherList) {
				watcher.IsMarkedForDestroy (true);
				continue;
			}

			if (watcher.duration.value <= 0) {
				ProcessEffect (watcher.skillEffectWatcher, watcher.skillEffectWatcher.target, false);

				var origin = watcher.skillEffectWatcher.target;
				var watcherList = origin.skillWatcherList.watchers;
				watcherList.Remove (watcher);
				origin.ReplaceSkillWatcherList (watcherList);

				watcher.IsMarkedForDestroy (true);
				continue;
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
			ProcessRoot (watcher, target, isApplying);
			break;
		case EffectType.Stun:
			ProcessStun (watcher, target, isApplying);
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
						var temp = armors [i].Reduction;
						armors [i].Reduction = armors [i].Reduction * watcher.effect.value / 100;
						watcher.SetChanges(temp / armors [i].Reduction);
						target.ReplaceArmor (armors);
					} else {
						armors [i].Reduction = watcher.GetChanges() * armors [i].Reduction;
						target.ReplaceArmor (armors);
					}
				}
			}
		}
	}

	void ProcessMovmentSpeed(SkillEffectWatcher watcher, Entity target, bool isApplying){
		if (target.hasMoveSpeed) {
			float speed = target.moveSpeed.speed;
			if (isApplying) {
				var temp = speed;
				speed = speed * watcher.effect.value / 100;
				watcher.SetChanges(temp / speed);
				target.ReplaceMoveSpeed (speed);
			} else {
				speed = watcher.GetChanges() * speed;
				target.ReplaceMoveSpeed (speed);
			}
		}
	}

	void ProcessRoot(SkillEffectWatcher watcher, Entity target, bool isApplying){
		if (isApplying) {
			target.IsRooted (true);
		} else {
			target.IsRooted (false);
		}
	}

	void ProcessStun(SkillEffectWatcher watcher, Entity target, bool isApplying){
		if (isApplying) {
			target.IsStunned (true);
		} else {
			target.IsStunned (false);
		}
	}

}

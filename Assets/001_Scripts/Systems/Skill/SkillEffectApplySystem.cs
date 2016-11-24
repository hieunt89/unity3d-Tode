using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class SkillEffectApplySystem : IReactiveSystem, ISetPool {
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
			var e = entities [i];

			for (int j = 0; j < e.effects.effects.Count; j++) {
				var ef = e.effects.effects [j];

				switch (ef.effectType) {
				case EffectType.MoveSpeedSlow:
					ProcessEffectMovement (ef, e);
					break;
				default:
					break;
				}
			}

			e.RemoveEffects ();
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Effects.OnEntityAdded ();
		}
	}
	#endregion

	bool CheckEffectDuplicate(Dictionary<SkillEffect, float> efToDur, SkillEffect ef){
		foreach(KeyValuePair<SkillEffect, float> entry in efToDur)
		{
			if (entry.Key.skillId == ef.skillId) {
				entry.Value = ef.duration;
				return true;
			}
		}
		return false;
	}

	void ProcessEffectMovement(SkillEffect ef, Entity target){
		if (!target.hasEffectMovementSpeed) {
			target.AddEffectMovementSpeed (new Dictionary<SkillEffect, float>());
		}

		if (!CheckEffectDuplicate(target.effectMovementSpeed.efToDuration, ef)) {
			target.effectMovementSpeed.efToDuration.Add (ef, ef.duration);
		}
	}

}

using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class EffectMovementApplySystem : IReactiveSystem, IEnsureComponents {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf (Matcher.Active);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			ProcessEffectMovement (e.effectMovement.ef, e);

			e.RemoveEffectMovement ();
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.EffectMovement.OnEntityAdded ();
		}
	}
	#endregion

	void ProcessEffectMovement(SkillEffect ef, Entity target){
		if (!target.hasEffectMovementList) {
			target.AddEffectMovementList (new Dictionary<SkillEffect, float> ());
		}

		if (!CombatUtility.ReplaceEffectDurationIfDuplicated(ref target.effectMovementList.efToDuration, ef)) {
			ApplySlow (ef.value, target);
			target.effectMovementList.efToDuration.Add (ef, ef.duration);
		}
	}

	void ApplySlow(float slow, Entity target){
		var newSpeed = Mathf.Max ( (target.moveSpeed.speed * slow.ReverseAndClamp01()), ConstantData.MIN_MOVE_SPEED );
		target.ReplaceMoveSpeed (newSpeed);
	}
}

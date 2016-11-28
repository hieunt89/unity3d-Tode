using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class EffectMovementUpdateSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Group _groupMovementSlowed;
	public void SetPool (Pool pool)
	{
		_groupMovementSlowed = pool.GetGroup (Matcher.AllOf(Matcher.EffectMovementList, Matcher.Active));
	}

	#endregion

	
	#region IReactiveExecuteSystem implementation
	List<SkillEffect> bufferList;
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupMovementSlowed.count <= 0) {
			return;
		}

		var tickEn = entities.SingleEntity ();
		var ens = _groupMovementSlowed.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var efToDur = ens [i].effectMovementList.efToDuration;

			if (efToDur.Count <= 0) {
				ens [i].RemoveEffectMovementList ();
				continue;
			}
				
			bufferList = new List<SkillEffect>(efToDur.Keys);
			foreach (var item in bufferList) {
				if (efToDur [item] > 0) {
					efToDur [item] -= tickEn.tick.change;
				} else {
					DeApplySlow (item.value, ens[i]);
					efToDur.Remove (item);
				}
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

	void DeApplySlow(float slow, Entity target){
		var newSpeed = target.moveSpeed.speed / slow.ReverseAndClamp01 ();
		target.ReplaceMoveSpeed (newSpeed);
	}
}

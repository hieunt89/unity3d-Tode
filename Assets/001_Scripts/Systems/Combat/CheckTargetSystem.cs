using UnityEngine;
using System.Collections;
using Entitas;
public class CheckTargetSystem : IReactiveSystem, ISetPool {
	Pool _pool;
	Group _groupHasTarget;
	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupHasTarget = _pool.GetGroup (Matcher.AllOf (Matcher.Active, Matcher.Target).AnyOf(Matcher.Tower, Matcher.Skill, Matcher.Ally));
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(_groupHasTarget.count <= 0){
			return;
		}

		var ens = _groupHasTarget.GetEntities ();
		for (int i = 0; i < ens.Length; i++) {
			var attacker = ens [i];
			var target = ens [i].target.e;
			var origin = attacker.hasSkill ? attacker.origin.e : attacker;
			if(!target.isTargetable || !target.position.value.IsInRange(origin.position.value, attacker.attackRange.value)){
				attacker.RemoveTarget ();
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


}

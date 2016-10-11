using UnityEngine;
using System.Collections;
using Entitas;

public class SkillCastSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	Group _groupSkillCastable;
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupSkillCastable = _pool.GetGroup (Matcher.AllOf(Matcher.Skill, Matcher.Active, Matcher.Target).NoneOf(Matcher.AttackCooldown));
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if (_groupSkillCastable.count <= 0) {
			return;
		}

		var skillEns = _groupSkillCastable.GetEntities ();
		for (int i = 0; i < skillEns.Length; i++) {
			var e = skillEns [i];
			if (e.origin.e.isChanneling) {
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
	
}

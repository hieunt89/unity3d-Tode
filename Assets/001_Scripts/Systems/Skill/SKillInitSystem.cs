using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class SkillInitSystem : IReactiveSystem, ISetPool {
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
			List<Entity> sList = new List<Entity> ();
			
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Position.OnEntityAdded ();
		}
	}
	#endregion

	Entity CreateSkill(Skill s){
		return null;
	}
}

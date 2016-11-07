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
				
			var eList = new List<Entity> ();
			for (int j = 0; j < e.skillList.skillTrees.Count; j++) {
				eList.Add (CreateSkill(e.skillList.skillTrees[j].Root, e));
			}
			e.AddSkillEntityList (eList);
			e.RemoveSkillList ();
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.SkillList.OnEntityAdded ();
		}
	}
	#endregion

	Entity CreateSkill(Node<string> skillNode, Entity origin){
		var e = _pool.CreateEntity ()
			.AddOrigin (origin)
			.AddSkill (skillNode)
			.ReplaceSkillStats (DataManager.Instance.GetSkillData (skillNode.data));
		return e;
	}
}

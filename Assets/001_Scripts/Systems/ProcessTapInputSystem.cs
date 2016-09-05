using UnityEngine;
using System.Collections;
using Entitas;

public class ProcessTapInputSystem : IReactiveSystem, ISetPool {
	
	Group _group;
	Pool _pool;

	#region ISetPool implementation

	public void SetPool (Pool pool)
	{
		_pool = pool;
		_group = _pool.GetGroup (Matcher.Id);
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = GetEntityById (entities [i].tapInput.id);
			if(e != null){
				e.ReplaceTower (TowerType.type2);
			}
			_pool.DestroyEntity (entities[i]);
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.TapInput.OnEntityAdded ();
		}
	}
	#endregion

	private Entity GetEntityById(string id){
		var entities = _group.GetEntities ();
		for (int i = 0; i < entities.Length; i++) {
			if(entities[i].id.value.Equals(id)){
				return entities [i];
			}
		}
		return null;
	}
}

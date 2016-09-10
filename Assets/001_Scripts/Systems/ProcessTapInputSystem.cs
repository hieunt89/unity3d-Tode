using Entitas;
using UnityEngine;

public class ProcessTapInputSystem : IReactiveSystem, ISetPool {
	
	Group _groupInteractable;
	Pool _pool;

	#region ISetPool implementation
	public void SetPool (Pool pool)
	{
		_pool = pool;
		_groupInteractable = _pool.GetGroup (Matcher.AllOf(Matcher.Id, Matcher.Interactable));
	}
	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = GetEntityById (entities [i].tapInput.id);
			if(e != null && e.hasTower){
				ProcessTowerTap (e);
			}else if(e != null && e.hasEnemy){
				ProcessEnemyTap (e);
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
		var entities = _groupInteractable.GetEntities ();
		for (int i = 0; i < entities.Length; i++) {
			if(entities[i].id.value.Equals(id)){
				return entities [i];
			}
		}
		return null;
	}

	private void ProcessTowerTap(Entity e){
		Debug.Log("tower tap");
	}

	private void ProcessEnemyTap(Entity e){
		Debug.Log("enemy tap");
	}
}

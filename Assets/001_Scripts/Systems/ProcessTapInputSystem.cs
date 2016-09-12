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
		var input = entities.SingleEntity ();
		var e = GetEntityById (input.inputTap.id);
		if(e != null){
			if(e.hasTower){
				HandleTowerTap (e);
			}
		}
		_pool.DestroyEntity (input);
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.InputTap.OnEntityAdded ();
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

	void HandleTowerTap(Entity e){
		Messenger.Broadcast<Entity> (Events.Input.TOWER_CLICK, e);
	}
}

using UnityEngine;
using System.Collections;
using Entitas;

public class TowerCreateViewSystem : IReactiveSystem {
	GameObject towerViewParent;
	public TowerCreateViewSystem(){
		towerViewParent = GameObject.Find ("TowersView");
		if(towerViewParent == null){
			towerViewParent = new GameObject ("TowersView");
			towerViewParent.transform.position = Vector3.zero;
		}
	}
	#region IReactiveExecuteSystem implementation

	void IReactiveExecuteSystem.Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			#region loading pref no async
//			GameObjectgo = Lean.LeanPool.Spawn (Resources.Load<GameObject>("Tower/" + entities[i].tower.towerId));
//
//			if (!entities [i].hasView) {
//				entities [i].AddView (go);
//			} else {
//				Lean.LeanPool.Despawn (entities [i].view.go);
//				entities [i].ReplaceView (go);
//			}
//
//			go.name = entities [i].id.value;
//			go.transform.position = entities [i].position.value;
//			go.transform.SetParent (towerViewParent.transform, false);
			#endregion

			#region loading pref async
			e.AddCoroutine(CreateTowerView(e));
			#endregion
		}
	}

	#endregion

	#region IReactiveSystem implementation

	TriggerOnEvent IReactiveSystem.trigger {
		get {
			return Matcher.Tower.OnEntityAdded();
		}
	}

	#endregion

	IEnumerator CreateTowerView(Entity e){
		var r = Resources.LoadAsync<GameObject> ("Tower/" + e.tower.towerId);
		while(!r.isDone){
			yield return null;
		}

		GameObject go = Lean.LeanPool.Spawn ( r.asset as GameObject );

		if (!e.hasView) {
			e.AddView (go);
		} else {
			Lean.LeanPool.Despawn (e.view.go);
			e.ReplaceView (go);
		}

		go.name = e.id.value;
		go.transform.position = e.position.value;
		go.transform.SetParent (towerViewParent.transform, false);
	}

}

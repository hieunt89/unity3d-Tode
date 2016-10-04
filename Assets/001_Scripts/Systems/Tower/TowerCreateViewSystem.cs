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
			entities [i].AddCoroutine(CreateTowerView(entities [i]));
		}
	}

	#endregion

	#region IReactiveSystem implementation

	TriggerOnEvent IReactiveSystem.trigger {
		get {
			return Matcher.AnyOf(Matcher.Tower, Matcher.TowerBase).OnEntityAdded();
		}
	}

	#endregion

	IEnumerator CreateTowerView(Entity e){
		string prefToLoad;
		if (e.isTowerBase) {
			prefToLoad = "towerbase";
		} else {
			prefToLoad = e.tower.currentNode.data;
		}

		var r = Resources.LoadAsync<GameObject> ("Tower/" + prefToLoad);
		while(!r.isDone){
			yield return null;
		}

		if(r.asset == null){
			if (GameManager.debug) {
				Debug.Log ("Fail to load asset " + prefToLoad + " from Resources");
			}
			yield break;
		}
		GameObject go = Lean.LeanPool.Spawn ( r.asset as GameObject );

		if (!e.hasView) {
			e.AddView (go);
		} else {
			EntityLink.Instance.RemoveLink (e.view.go);
			Lean.LeanPool.Despawn (e.view.go);
			e.ReplaceView (go);
		}

		EntityLink.Instance.AddLink (go, e);
		e.IsActive (true).IsInteractable (true);

		go.name = e.id.value;
		go.transform.position = e.position.value;
		go.transform.SetParent (towerViewParent.transform, false);
	}

}

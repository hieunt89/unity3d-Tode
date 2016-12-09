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
			entities [i].AddCoroutineTask (CreateTowerView (entities [i]));
		}
	}

	#endregion

	#region IReactiveSystem implementation

	TriggerOnEvent IReactiveSystem.trigger {
		get {
			return Matcher.AnyOf(Matcher.Tower, Matcher.TowerBase, Matcher.TowerUpgrading).OnEntityAdded();
		}
	}

	#endregion

	IEnumerator CreateTowerView(Entity e){
		string prefToLoad;
		if (e.isTowerBase) {
			prefToLoad = "towerbase";
		}else if (e.isTowerUpgrading) {
			prefToLoad = "towerupgrade";
		} else {
			prefToLoad = e.tower.towerNode.data;
		}

		var r = Resources.LoadAsync<GameObject> (ConstantString.ResourcesPrefab + prefToLoad);
		while(!r.isDone){
			yield return null;
		}

		if(r.asset == null){
			if (GameManager.debug) {
				Debug.Log ("Fail to load asset " + prefToLoad + " from Resources");
			}
			yield break;
		}

		if (e.hasView) {
			EntityLink.RemoveLink (e.view.go);
			Lean.LeanPool.Despawn (e.view.go);
		}

		GameObject go = Lean.LeanPool.Spawn ( r.asset as GameObject );
		go.name = e.id.value;
		go.transform.position = e.position.value;
		go.transform.SetParent (towerViewParent.transform, false);

		if (e.hasTower || e.isTowerBase) {
			EntityLink.AddLink (go, e);

			var anims = go.GetComponentsInChildren<Animator>();
			if (anims != null) {
				e.ReplaceViewAnims (anims);
			} else if (e.hasViewAnims) {
				e.RemoveViewAnims ();
			}

			var col = go.GetComponent<Collider> ();
			if (col != null) {
				e.ReplaceViewCollider (col);
			}else if (e.hasViewCollider) {
				e.RemoveViewCollider ();
			}
		}

		e.ReplaceView (go)
			.AddViewLookAtComponent()
			.IsInteractable (true);
	}
}

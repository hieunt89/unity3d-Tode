using UnityEngine;
using System.Collections;
using Entitas;

public class CharacterCreateViewSystem : IReactiveSystem {
	#region Constructor
	GameObject charViewParent;
	public CharacterCreateViewSystem(){
		charViewParent = GameObject.Find ("CharactersView");
		if(charViewParent == null){
			charViewParent = new GameObject ("CharactersView");
			charViewParent.transform.position = Vector3.zero;
		}
	}
	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			e.AddCoroutineTask(CreateCharView(e), true);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Active).AnyOf(Matcher.Enemy, Matcher.Ally).OnEntityAdded ();
		}
	}

	#endregion

	IEnumerator CreateCharView(Entity e){
		string prefToLoad = null;
		if (e.hasEnemy) {
			prefToLoad = e.enemy.enemyId;
		}else if (e.hasAlly) {
			prefToLoad = e.ally.charId;
		}
		var r = Resources.LoadAsync<GameObject> (ConstantString.ResourcesPrefab + prefToLoad);
		while(!r.isDone){
			yield return null;
		}

		GameObject go = Lean.LeanPool.Spawn (r.asset as GameObject);
		go.name = e.id.value;
		go.transform.position = e.position.value;
		if (e.hasDestination) {
			go.transform.rotation = Quaternion.LookRotation(e.destination.value - e.position.value);
		}
		go.transform.SetParent (charViewParent.transform, false);

		EntityLink.AddLink (go, e);

		var col = go.GetComponent<Collider> ();
		if (col != null) {
			e.AddViewCollider (col);
		}

		var anims = go.GetComponentsInChildren<Animator>();
		if (anims != null) {
			e.AddViewAnims (anims);
		}

		e.AddView (go)
			.IsInteractable (true);
	}
}

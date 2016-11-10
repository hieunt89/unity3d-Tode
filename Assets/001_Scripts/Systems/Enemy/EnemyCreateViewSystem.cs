using UnityEngine;
using System.Collections;
using Entitas;

public class EnemyCreateViewSystem : IReactiveSystem {
	#region Constructor
	GameObject enemyViewParent;
	public EnemyCreateViewSystem(){
		enemyViewParent = GameObject.Find ("EnemysView");
		if(enemyViewParent == null){
			enemyViewParent = new GameObject ("EnemysView");
			enemyViewParent.transform.position = Vector3.zero;
		}
	}
	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			e.AddCoroutineTask(CreateEnemyView(e), true);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf(Matcher.Enemy, Matcher.Active).OnEntityAdded ();
		}
	}

	#endregion

	IEnumerator CreateEnemyView(Entity e){
		var r = Resources.LoadAsync<GameObject> (ConstantString.ResourcesPrefab + e.enemy.enemyId);
		while(!r.isDone){
			yield return null;
		}

		GameObject go = Lean.LeanPool.Spawn (r.asset as GameObject);

		EntityLink.AddLink (go, e);

		go.name = e.id.value;
		go.transform.position = e.position.value;
		go.transform.rotation = Quaternion.LookRotation(e.destination.value - e.position.value);
		go.transform.SetParent (enemyViewParent.transform, false);

		e.AddView (go)
			.AddPointTarget(go.BotToCenterOffset())
			.IsInteractable (true);
	}
}

using UnityEngine;
using System.Collections;
using Entitas;

public class EnemyCreateViewSystem : IReactiveSystem, IEnsureComponents {
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
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.Enemy;
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		GameObject go;
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			if(e.hasEnemy){
				go = Lean.LeanPool.Spawn (Resources.Load<GameObject> ("Enemy/" + e.enemy.enemyId));
				go.name = e.id.value;

				go.transform.position = e.position.value;
				go.transform.rotation = Quaternion.LookRotation(e.destination.value - e.position.value);
				go.transform.SetParent (enemyViewParent.transform, false);

				e.AddView (go);
			}
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

}

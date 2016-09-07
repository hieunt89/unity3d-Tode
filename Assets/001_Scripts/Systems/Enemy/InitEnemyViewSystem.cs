using UnityEngine;
using System.Collections;
using Entitas;

public class InitEnemyViewSystem : IReactiveSystem, IEnsureComponents {
	GameObject enemyViewParent;
	public InitEnemyViewSystem(){
		enemyViewParent = GameObject.Find ("EnemysView");
		if(enemyViewParent == null){
			enemyViewParent = new GameObject ("EnemysView");
			enemyViewParent.transform.position = Vector3.zero;
		}
	}
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
				go = GameObject.Instantiate(Resources.Load<GameObject> ("Enemy/" + e.enemy.eClass + "/" + e.enemy.eType));
				go.name = e.id.value;
				go.transform.position = e.position.value;
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

using UnityEngine;
using System.Collections;
using Entitas;

public class InitEnemyViewSystem : IReactiveSystem, IEnsureComponents {
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
				go.transform.position = e.position.value;
				go.name = e.id.value;
				e.AddView (go);
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.Activable.OnEntityRemoved ();
		}
	}

	#endregion

}

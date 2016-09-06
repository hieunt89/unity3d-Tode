using UnityEngine;
using Entitas;

public class UpdateTowerViewSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation

	void IReactiveExecuteSystem.Execute (System.Collections.Generic.List<Entity> entities)
	{
		GameObject go = null;
		for (int i = 0; i < entities.Count; i++) {
			switch (entities[i].tower.type) {
			case TowerType.type1:
				go = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				break;
			case TowerType.type2:
				go = GameObject.CreatePrimitive (PrimitiveType.Cube);
				break;
			default:
				go = new GameObject ();
				break;
			}

			if (!entities [i].hasView) {
				entities [i].AddView (go);
			} else {
				GameObject.Destroy (entities [i].view.go);
				entities [i].ReplaceView (go);
			}

			go.name = entities [i].id.value;
			go.transform.position = entities [i].position.value;
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


}

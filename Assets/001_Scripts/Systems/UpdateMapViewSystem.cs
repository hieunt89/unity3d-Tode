using UnityEngine;
using Entitas;

public class UpdateMapViewSystem : IReactiveSystem {
	#region IReactiveExecuteSystem implementation

	void IReactiveExecuteSystem.Execute (System.Collections.Generic.List<Entity> entities)
	{
		GameObject go = null;
		for (int i = 0; i < entities.Count; i++) {
			switch (entities [i].tile.tileType) {
			case TileType.none:
				go = new GameObject ();
				break;
			case TileType.movable:
				go = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				break;
			case TileType.constructable:
				go = GameObject.CreatePrimitive (PrimitiveType.Cube);
				break;
			}

			GameObject.Destroy (entities [i].tileView.view);

			go.name = entities [i].position.x + "_" + entities [i].position.y;
			go.transform.position = new Vector3 ((float)entities [i].position.x, 0f, (float)entities [i].position.y);
			go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
			go.transform.SetParent (Pools.pool.map.view.transform, false);
			entities [i].ReplaceTileView (go);
		}
	}

	#endregion

	#region IReactiveSystem implementation

	TriggerOnEvent IReactiveSystem.trigger {
		get {
			return Matcher.Tile.OnEntityAdded();
		}
	}

	#endregion


}

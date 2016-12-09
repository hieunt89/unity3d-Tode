using UnityEngine;
using System.Collections;
using Entitas;

public class SelectedViewToggleSystem : IReactiveSystem, IEnsureComponents, IInitializeSystem {
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf(Matcher.View, Matcher.ViewCollider);
		}
	}

	#endregion

	#region IInitializeSystem implementation
	GameObject prefab;
	GameObject selectedViewParent;
	public void Initialize ()
	{
		prefab = Resources.Load<GameObject> (ConstantString.ResourcesPrefab + "SelectedIndicator");

		selectedViewParent = GameObject.Find ("SelectedIndicator");
		if(selectedViewParent == null){
			selectedViewParent = new GameObject ("SelectedIndicator");
			selectedViewParent.transform.position = Vector3.zero;
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (e.isSelected) {
				GameObject go = Lean.LeanPool.Spawn ( prefab as GameObject );

				go.GetComponent<Projector> ().orthographicSize = e.viewCollider.collider.bounds.size.x;
				go.name = e.id.value + " selected";
				go.transform.position = e.position.value;
				go.transform.eulerAngles = new Vector3 (90f, 0f, 0f);
				go.transform.SetParent (selectedViewParent.transform, false);

				e.ReplaceViewSelected (go);
			} else {
				Lean.LeanPool.Despawn (e.viewSelected.indicator);
				e.RemoveViewSelected ();
			}
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Selected.OnEntityAddedOrRemoved ();
		}
	}
	#endregion
	
}

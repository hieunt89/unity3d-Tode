using UnityEngine;
using System.Collections;
using Entitas;

public class HeathBarViewSystem : IReactiveSystem, IInitializeSystem {
	#region IInitializeSystem implementation
	BarGUI barGUI;
	public void Initialize ()
	{
		barGUI = GameObject.FindObjectOfType<BarGUI> ();
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(barGUI == null){
			Debug.Log ("GUI not found");
			return;
		}

		Vector3 offset;
		Entity e;
		for (int i = 0; i < entities.Count; i++) {
			e = entities [i];
			if (!e.hasViewSlider) {
				e.AddViewSlider (barGUI.CreateHealthBar ());
			}

			offset = e.view.go.GetRendererOffset (true);
			e.viewSlider.bar.transform.position = Camera.main.WorldToScreenPoint (e.position.value + offset);

			e.viewSlider.bar.value = (float)e.hp.value / (float)e.hpTotal.value;
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.Hp, Matcher.HpTotal, Matcher.Active, Matcher.Position, Matcher.View).OnEntityAdded ();
		}
	}
	#endregion
}
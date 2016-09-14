using UnityEngine;
using System.Collections;
using Entitas;

public class HeathBarViewSystem : IReactiveSystem, IInitializeSystem {
	#region IInitializeSystem implementation
	HealthBarGUI healthBarGUI;
	public void Initialize ()
	{
		healthBarGUI = GameObject.FindObjectOfType<HealthBarGUI> ();
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(healthBarGUI == null){
			Debug.Log ("HealthBarGUI not found");
			return;
		}

		Vector3 offset;
		Renderer rend;
		Entity e;
		for (int i = 0; i < entities.Count; i++) {
			e = entities [i];
			if (!e.hasViewSlider) {
				e.AddViewSlider (healthBarGUI.CreateHealthBar ());
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
			return Matcher.AllOf (Matcher.Hp, Matcher.HpTotal, Matcher.Active, Matcher.Position).OnEntityAdded ();
		}
	}
	#endregion
}

using UnityEngine;
using System.Collections;
using Entitas;

public class HealthBarToggleSystem : IReactiveSystem, IInitializeSystem, IEnsureComponents {
	#region IEnsureComponents implementation
	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf (Matcher.Active, Matcher.View);
		}
	}
	#endregion

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
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			if (e.isWound) {
				if (!e.hasViewSlider) {
					offset = e.view.go.SliderOffset (true);
					e.AddViewSlider (barGUI.CreateHealthBar (), offset);
				}
			}else if (e.hasViewSlider) {
				Lean.LeanPool.Despawn (e.viewSlider.bar.gameObject);
				e.RemoveViewSlider ();
			}
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.AllOf (Matcher.Wound).OnEntityAddedOrRemoved ();
		}
	}
	#endregion
}
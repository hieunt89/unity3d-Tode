using UnityEngine;
using System.Collections;
using Entitas;

public class TowerProgressBarViewSystem : IReactiveSystem, IInitializeSystem{
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
			Debug.Log ("BarGUI not found");
			return;
		}

		Vector3 offset;
		Entity e;
		for (int i = 0; i < entities.Count; i++) {
			e = entities [i];
			if (e.hasTowerUpgradeProgress) {
				if(!e.hasViewSlider){
					offset = e.view.go.GetRendererOffset (false);
					e.AddViewSlider (barGUI.CreateProgressBar (), offset);
				}

				e.viewSlider.bar.transform.position = Camera.main.WorldToScreenPoint (e.position.value + e.viewSlider.offset);
				e.viewSlider.bar.value = e.towerUpgradeProgress.progress / e.towerUpgrade.upgradeTime;
			} else {
				if (e.hasViewSlider) {
					Lean.LeanPool.Despawn (e.viewSlider.bar.gameObject);
					e.RemoveViewSlider ();
				}
			}
		}
	}

	#endregion

	#region IReactiveSystem implementation

	public TriggerOnEvent trigger {
		get {
			return Matcher.TowerUpgradeProgress.OnEntityAddedOrRemoved ();
		}
	}

	#endregion


}

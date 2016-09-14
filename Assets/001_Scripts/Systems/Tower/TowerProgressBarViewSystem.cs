using UnityEngine;
using System.Collections;
using Entitas;

public class TowerProgressBarViewSystem : IReactiveSystem, IInitializeSystem{
	#region IInitializeSystem implementation
	ProgressBarGUI progressBarGUI;
	public void Initialize ()
	{
		progressBarGUI = GameObject.FindObjectOfType<ProgressBarGUI> ();
	}

	#endregion

	#region IReactiveExecuteSystem implementation

	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		if(progressBarGUI == null){
			Debug.Log ("ProgressBarGUI not found");
			return;
		}

		Vector3 offset;
		Entity e;
		Renderer rend;
		for (int i = 0; i < entities.Count; i++) {
			e = entities [i];
			if (e.hasTowerUpgradeProgress) {
				if(!e.hasViewSlider){
					e.AddViewSlider (progressBarGUI.CreateProgressBar ());
				}

				rend = e.view.go.GetComponent<Renderer> ();
				if(rend == null){
					offset = Vector3.up;
				}else{
					offset = new Vector3 (0f, -rend.bounds.extents.y, 0f);
				}

				e.viewSlider.bar.transform.position = Camera.main.WorldToScreenPoint (e.position.value + offset);

				e.viewSlider.bar.value = e.towerUpgradeProgress.progress / e.towerUpgrade.upgradeTime;
			} else {
				GameObject.Destroy (e.viewSlider.bar.gameObject);
				e.RemoveViewSlider ();
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

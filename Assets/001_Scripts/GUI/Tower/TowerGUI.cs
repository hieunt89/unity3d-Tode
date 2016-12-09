using UnityEngine;
using System.Collections;

public class TowerGUI : HandleTowerSelectGUI {
	CanvasGroup group;
	void Start(){
		group = GetComponent<CanvasGroup> ();
	}
	#region implemented abstract members of HandleTowerSelectGUI

	public override void HandleTowerClick (Entitas.Entity e)
	{
		HandleEmptyClick ();
		if (e.hasTower || e.isTowerBase || e.isTowerUpgrading) {
			if (group != null) {
				group.alpha = 1f;
//			group.interactable = true;
				group.blocksRaycasts = true;
			}
		}
	}

	public override void HandleEmptyClick ()
	{
		if (group != null) {
			group.alpha = 0f;
//			group.interactable = false;
			group.blocksRaycasts = false;
		}
	}

	#endregion
}

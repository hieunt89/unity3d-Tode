using UnityEngine;
using System.Collections;
using UnityEditor;

public class ActionBarView : ViewBase {

	public ActionBarView () : base () {
		Debug.Log ("construct action bar view");
	}

	public override void UpdateView ()
	{
		base.UpdateView ();
		Debug.Log ("update action bar");

		EditorGUILayout.BeginHorizontal ("box", GUILayout.Height (25));
		if (GUILayout.Button ("Add")) {
//			AddProjectileData ();
//			Messenger.Broadcast (Events.Editor.ADD);
		}

		GUILayout.FlexibleSpace ();

		if (GUILayout.Button (toggleEditMode ? "Done" : "Edit Mode")) {
			toggleEditMode = !toggleEditMode;
		}
		if (toggleEditMode) {
			if (GUILayout.Button (isSelectedAll ? "Deselect All" : "Select All")) {
				isSelectedAll = !isSelectedAll;
				for (int i = 0; i < selectedIndexes.Count; i++) {
					selectedIndexes[i] = isSelectedAll;
				}
			}
			if (GUILayout.Button ("Delete Selected")) {
				if (EditorUtility.DisplayDialog ("Are you sure?", 
					"Do you want to delete all selected data?",
					"Yes", "No")) {
					for (int i = selectedIndexes.Count - 1; i >= 0; i--) {
						if (selectedIndexes[i]) {
//							projectileList.projectiles.RemoveAt (i);
							// TODO: remove selected indexes
							selectedIndexes.RemoveAt (i);
						}
					}
					isSelectedAll = false;
					toggleEditMode = false;
				}
			}
		}

		EditorGUILayout.EndHorizontal ();
	}

	public override void ProcessEvent ()
	{
		base.ProcessEvent ();
	}
}

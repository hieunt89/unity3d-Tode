using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class TreeNodeListView : TreeViewBase {

	private bool toggleEditMode;
	private bool isSelectedAll;
	private List<bool> selectedIndexes;
	private Vector2 scrollPosition;

	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeGUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);
		var newRect = new Rect (viewRect.width / 2 - 200f, viewRect.height / 2 - 100f, 400, 200);
//		GUI.Box (newRect, currentWindow.workType.ToString ());
		GUILayout.BeginArea (newRect);

		GUILayout.Label (currentWindow.workType.ToString ());
		GUILayout.Space(10);

		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("<")) {
			currentWindow.viewIndex = ViewIndex.Menu;
		}
		if (GUILayout.Button ("Add")) {
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
//							towerList.towers.RemoveAt (i);
							selectedIndexes.RemoveAt (i);
						}
					}
					isSelectedAll = false;
					toggleEditMode = false;
				}
			}
		}

		EditorGUILayout.EndHorizontal ();


		EditorGUILayout.BeginVertical ();
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUILayout.Height (newRect.height - 40));
//		if (towerList.towers != null ) {
//			for (int i = 0; i < towerList.towers.Count; i++) {
//				EditorGUILayout.BeginHorizontal ();
//
//				var btnLabel = towerList.towers[i].Id;
//				if (GUILayout.Button (btnLabel)) {
//					towerIndex = i;
//					viewIndex = 1;
//					tower = towerList.towers [towerIndex];
//					projectileIndex = SetupProjectileIndex ();
//				}
//				GUI.enabled = toggleEditMode;
//				selectedTowerIndexes[i] = EditorGUILayout.Toggle (selectedTowerIndexes[i], GUILayout.Width (30));
//				GUI.enabled = true;
//				EditorGUILayout.EndHorizontal ();
//
//			}
//		}
		EditorGUILayout.EndScrollView ();
		EditorGUILayout.EndVertical ();
		GUILayout.EndArea ();
	}

	public override void ProcessEvent (Event e)
	{
		base.ProcessEvent (e);
	}


}

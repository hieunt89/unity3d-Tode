using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

public class TreeNodeListView : TreeViewBase {
	private bool toggleEditMode;
	private bool isSelectedAll;
	private Vector2 scrollPosition;
	protected List <bool> selectedTreeIndexes;


	public TreeNodeListView ()
	{
		if (selectedTreeIndexes == null) 
			selectedTreeIndexes = new List<bool> ();

		for (int i = 0; i < currentWindow.treeList.trees.Count ; i++) {
			selectedTreeIndexes.Add (false);
		}
	}
	
	public override void UpdateView (Rect _editorRect, Rect _percentageRect, Event _e, TreeGUI _currentTree)
	{
		base.UpdateView (_editorRect, _percentageRect, _e, _currentTree);

		var newRect = new Rect (viewRect.width / 2 - 200f, viewRect.height / 2 - 100f, 400, 200);

		GUILayout.BeginArea (newRect);

//		currentWindow.workingType = (TreeType) EditorGUILayout.EnumPopup ("Tree Type", currentWindow.workingType);
//
//		GUILayout.Space(10);
//
//		GUI.enabled = currentWindow.workingType != TreeType.None;
//
//		EditorGUILayout.BeginHorizontal ();
//
//		if (GUILayout.Button ("Add")) {
//			AddTreeData ();
//		}
//
//		GUILayout.FlexibleSpace ();
//
//		if (GUILayout.Button (toggleEditMode ? "Done" : "Edit Mode")) {
//			toggleEditMode = !toggleEditMode;
//		}
//
//		if (toggleEditMode) {
////			if (GUILayout.Button (isSelectedAll ? "Deselect All" : "Select All")) {
////				isSelectedAll = !isSelectedAll;
////				for (int i = 0; i < selectedTreeIndexes.Count; i++) {
////					selectedTreeIndexes[i] = isSelectedAll;
////				}
////			}
//
//			if (GUILayout.Button ("Delete Selected")) {
//				if (EditorUtility.DisplayDialog ("Are you sure?", 
//					"Do you want to delete all selected data?",
//					"Yes", "No")) {
//					for (int i = selectedTreeIndexes.Count - 1; i >= 0; i--) {
//						if (selectedTreeIndexes[i]) {
//							currentWindow.treeList.trees.RemoveAt (i);
//							selectedTreeIndexes.RemoveAt (i);
//						}
//					}
//					isSelectedAll = false;
//					toggleEditMode = false;
//				}
//			}
//		}
//
//		EditorGUILayout.EndHorizontal ();
//		GUI.enabled = true;
//
//		EditorGUILayout.BeginVertical ();
//		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUILayout.Height (viewRect.height - 40));
//		if (currentWindow.treeList.trees != null ) {
//			if (currentWindow.treeList.trees.Count > 0) {
//
//				for (int i = 0; i < currentWindow.treeList.trees.Count; i++) {
//					EditorGUILayout.BeginHorizontal ();
//					if (currentWindow.workingType == currentWindow.treeList.trees[i].treeType) {
//						var btnLabel = currentWindow.treeList.trees[i].id;
//						if (GUILayout.Button (btnLabel)) {
//							selectedTreeIndex = i;
//							currentWindow.viewIndex = ViewIndex.Detail;
//		//					tower = towerList.towers [towerIndex];
//		//					projectileIndex = SetupProjectileIndex ();
//						}
//						GUI.enabled = toggleEditMode;
//						selectedTreeIndexes[i] = EditorGUILayout.Toggle (selectedTreeIndexes[i], GUILayout.Width (30));
//						GUI.enabled = true;
//					}
//					EditorGUILayout.EndHorizontal ();
//
//				}
//			} else {
//				GUILayout.Label ("Tree List is Empty.");
//			}
//		}
//		EditorGUILayout.EndScrollView ();
//		EditorGUILayout.EndVertical ();
		GUILayout.EndArea ();

		if (GUI.changed) {
			EditorUtility.SetDirty (currentWindow.treeList);
		}
	}

	public override void ProcessEvent (Event e)
	{
		base.ProcessEvent (e);
	}

	private void AddTreeData () {
		Tree<string> newTree = new Tree<string> ();
		newTree.id = Guid.NewGuid().ToString();
		newTree.treeType = currentWindow.workingType;
		currentWindow.treeList.trees.Add (newTree);
		selectedTreeIndexes.Add (false);
		selectedTreeIndex = currentWindow.treeList.trees.Count;
	}

}

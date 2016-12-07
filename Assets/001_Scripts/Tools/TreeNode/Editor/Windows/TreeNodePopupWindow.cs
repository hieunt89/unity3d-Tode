//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//
//public class TreeNodePopupWindow : EditorWindow {
//
//	static TreeNodeEditorWindow mainWindow;
//	static TreeNodePopupWindow currentPopup;
//	string treeName = "Enter tree name ...";
//	TreeType treeType;
//	List <bool> selectedTreeIndexes;
//
//	bool toggleEditMode;
//	bool isSelectedAll;
//	private Vector2 scrollPosition;
//	protected int selectedTreeIndex;
//
//	public static void InitTreeNodePopup () {
//		currentPopup = (TreeNodePopupWindow) EditorWindow.GetWindow <TreeNodePopupWindow> ();
//		currentPopup.titleContent = new GUIContent ("Tree Node Popup");
//		mainWindow = (TreeNodeEditorWindow) EditorWindow.GetWindow <TreeNodeEditorWindow> ();
//	}
//	private void OnGUI () {
//
//		mainWindow.workingType = (TreeType) EditorGUILayout.EnumPopup ("Tree Type", mainWindow.workingType);
//
//		GUILayout.Space(10);
//
//		GUI.enabled = mainWindow.workingType != TreeType.None;
//
//		EditorGUILayout.BeginHorizontal ();
//
//		if (GUILayout.Button ("Add")) {
////			AddTreeData ();
//		}
//
//		GUILayout.FlexibleSpace ();
//
//		if (GUILayout.Button (toggleEditMode ? "Done" : "Edit Mode")) {
//			toggleEditMode = !toggleEditMode;
//		}
//
//		if (toggleEditMode) {
//
//			if (GUILayout.Button ("Delete Selected")) {
//				if (EditorUtility.DisplayDialog ("Are you sure?", 
//					"Do you want to delete all selected data?",
//					"Yes", "No")) {
//					for (int i = selectedTreeIndexes.Count - 1; i >= 0; i--) {
//						if (selectedTreeIndexes[i]) {
//							mainWindow.treeList.trees.RemoveAt (i);
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
//		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition, GUILayout.Height (position.height - 40));
//		if (mainWindow.treeList.trees != null ) {
//			if (mainWindow.treeList.trees.Count > 0) {
//
//				for (int i = 0; i < mainWindow.treeList.trees.Count; i++) {
//					EditorGUILayout.BeginHorizontal ();
//					if (mainWindow.workingType == mainWindow.treeList.trees[i].treeType) {
//						var btnLabel = mainWindow.treeList.trees[i].id;
//						if (GUILayout.Button (btnLabel)) {
//							selectedTreeIndex = i;
//							mainWindow.viewIndex = ViewIndex.Detail;
//							//					tower = towerList.towers [towerIndex];
//							//					projectileIndex = SetupProjectileIndex ();
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
//	}
//
//
//}

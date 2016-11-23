using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class DataEditorWindow : EditorWindow {

	public static DataEditorWindow dataEditorWindow;
	public ActionBarView actionBarView;
	public ItemListView itemListView; 
	public ItemDetailView itemDetailView; 

	int viewIndex = 0;
	bool toggleEditMode = false;
	Vector2 scrollPosition;
	bool isSelectedAll = false;
	List<bool> selectedIndexes;

	[MenuItem("Tode/Data Editor &D")]
	public static void DisplayWindow()
	{
		dataEditorWindow = EditorWindow.GetWindow <DataEditorWindow> ("Data Editor", true);
		dataEditorWindow.minSize = new Vector2 (400, 600); 
		CreateViews ();
	}

	void OnEnable () {
		Debug.Log (actionBarView);
		// actionbar view is null -> onenable call before displaywindow
	}

	void OnGUI()
	{
		
	}

	static void CreateViews () {
		if (dataEditorWindow != null) {
			ActionBarView actionBarView = new ActionBarView ();
		}
	}
}
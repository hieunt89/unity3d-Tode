using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class ViewBase {
	public DataEditorWindow currentWindow;

	protected Rect viewRect;


	protected bool toggleEditMode;
	protected bool isSelectedAll;
	protected List<bool> selectedIndexes;

	public ViewBase ()
	{
		Debug.Log ("construct view base");
		currentWindow = (DataEditorWindow)EditorWindow.GetWindow <DataEditorWindow> ();
	}

	public virtual void UpdateView () {
		Debug.Log ("update view base");
	}

	public virtual void ProcessEvent () {

	}
}

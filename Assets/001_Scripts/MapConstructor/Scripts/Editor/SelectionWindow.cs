using UnityEngine;
using UnityEditor;
using System;
// this just lets you use "myImplementation as ISelectionWindow" instead of going through reflection to convert to a real selectionwindow
// i value the code legibility over the extra interface
public interface ISelectionWindow {
	void OnGUI();
}

public class SelectionWindow<T> : ISelectionWindow {
	// your code here
	public void OnGUI() {
		GUILayout.Label("I'm of type " + typeof(T));
	}
}

public class SelectionWindowWrapper : EditorWindow {
	Type _type = null;
	// I wasn't able to quickly find a solution to "have a variable that could represent a specific generic class of any type"
	// but object works fine - note the little o, this is C#'s object not Unity's Object
	object myImplementation = null;

	[MenuItem("Window/Custom Tools/Selection Window")] // for testing purposes
	public static void Init() {
		SelectionWindowWrapper sww = EditorWindow.CreateInstance<SelectionWindowWrapper>();
		sww.Show();
	}

	public Type MyType {
		get { return _type.GetGenericArguments()[0]; }
		set {
			// These two lines are how you get your specific implementation
			_type = typeof(SelectionWindow<>).MakeGenericType(value);
			myImplementation = System.Activator.CreateInstance(_type);
		}
	}

	void OnGUI() {
		if (null == myImplementation ) { // provide selection options or prevent this condition
			if ( GUILayout.Button("GameObject") ) 
				MyType = typeof(GameObject); // notice capital T, we're using the setter
			if ( GUILayout.Button("Int") ) 
				MyType = typeof(int); // notice capital T, we're using the setter
			return;
		}
		(myImplementation as ISelectionWindow).OnGUI();
	}
}
using UnityEditor;
using UnityEngine;
using System.Collections;

public class GUIWindowDemo2 : EditorWindow {
	// The position of the window
	public Rect windowRect = new Rect(100, 100, 200, 200);
	// Scroll position
	public Vector2 scrollPos = Vector2.zero;
	void OnGUI() {
		// Set up a scroll view
		scrollPos = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollPos, new Rect(0, 0, 1000, 1000));

		// Same code as before - make a window. Only now, it's INSIDE the scrollview
		BeginWindows();
		windowRect = GUILayout.Window(1, windowRect, DoWindow, "Hi There");
		EndWindows();
		// Close the scroll view
		GUI.EndScrollView();
	}
	void DoWindow(int unusedWindowID) {
		GUILayout.Button("Hi");
		GUI.DragWindow();
	}
	[MenuItem("Test/GUIWindow Demo 2")]
	static void Init() {
		EditorWindow.GetWindow(typeof(GUIWindowDemo2));
	}
}
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class MyEditor : EditorWindow {

	GUITextField username;
	GUITextField realname;
	GUIButton registerButton;

	// Optional, but may be convenient.
	List<IGUI> gui = new List<IGUI>();

	[MenuItem("Example/Show Window")]
	public static void ShowWindow () {
		GetWindow<MyEditor> ().Show ();
	}

	void OnEnable() {
		username = new GUITextField ();
		username.label.text = "Username";
		username.text = "JDoe";

		realname = new GUITextField ();
		realname.label.text = "Real name";
		realname.text = "John Doe";

		registerButton = new GUIButton ();
		registerButton.label.text = "Register";
		registerButton.Clicked += RegisterUser;

		gui.Add (username);
		gui.Add (realname);
		gui.Add (registerButton);
	}

	void RegisterUser()
	{
		var msg = "Registering " + realname.text + " as " + username.text; 
		Debug.Log (msg);
	}

	void OnGUI() {
		foreach (var item in gui)
			item.OnGUI();
	}
}

public interface IGUI {
	void OnGUI();
}

public class GUITextField : IGUI {
	public string text = "";
	public GUIContent label = new GUIContent();

	// Unused in my example, but you may want to check if
	// a textbox becomes empty for example.
	public event System.Action<string> TextChanged;

	public void OnGUI() {
		// Also I wanted to show you BeginChangeCheck and EndChangeCheck 
		// which is the Unity GUI way of checking if a GUI control changed...
		EditorGUI.BeginChangeCheck ();
		text = EditorGUILayout.TextField (label, text);
		if (EditorGUI.EndChangeCheck () && TextChanged != null)
			TextChanged (text);
	}
}

public class GUIButton : IGUI {
	public GUIContent label = new GUIContent();
	public event System.Action Clicked;

	public void OnGUI() {
		if (GUILayout.Button (label) && Clicked != null)
			Clicked ();
	}
}
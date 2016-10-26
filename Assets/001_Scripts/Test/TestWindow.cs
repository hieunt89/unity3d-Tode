using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;

[Serializable]
public class TestData : ScriptableObject {
	public int id = 9;
	public string name = "fdj";
	public List<string> languages = new List<string> { "c#", "python" };



	public TestData (int id, string name, List<string> languages)
	{
		this.id = id;
		this.name = name;
		this.languages = languages;
	}
		
}
#if UNITY_EDITOR
public class TestWindow : EditorWindow {

	TestData data;

	SerializedObject so;
	private List<string> props;
	[MenuItem ("Test Window/Open")]
	public static void OpenWindow () {
		var window = (TestWindow)EditorWindow.GetWindow <TestWindow> ("Test Window", true);
	}

	void OnEnable () {
		data = new TestData (0, "fdj", new List<string> { "c#", "python" });

		data = ScriptableObject.CreateInstance <TestData> ();
		data.id = 0;
		data.name = "fdj";
		data.languages = new List<string> { "c#", "python" };
			
		FieldInfo[] fields = data.GetType ().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		so = new SerializedObject (data);
		props = new List<string> ();
		for (int i = 0; i < fields.Length; i++) {
			props.Add (fields [i].Name);
		}
	}

	void OnGUI () {

		if (data == null)
			return;

		so.Update ();
	
		for (int i = 0; i < props.Count; i++) {
			var sp = so.FindProperty (props [i]);
			EditorGUILayout.PropertyField (sp, true);

//			if (sp.hasVisibleChildren) {
//				Debug.Log (props [i]);
//				for (int j = 0; j < sp.arraySize; j++) {
//					var item = sp.GetArrayElementAtIndex (j);
//					EditorGUILayout.PropertyField (item, true);
//				}
//			}
		}
	}
}
#endif

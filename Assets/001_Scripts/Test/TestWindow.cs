using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Reflection;
using System;

[Serializable]
public class TestData : ScriptableObject {
	public int id;
	public string name;
	public Gender gender;
	public List<Skill> skills;
}

[Serializable]
public class Skill {
	public int id;
	public LANGUAGE language;
	public float year;

	public Skill (int id, LANGUAGE language, float year)
	{
		this.id = id;
		this.language = language;
		this.year = year;
	}
	
}

#if UNITY_EDITOR

public enum Gender {
	MALE,
	FEMALE
}

public enum LANGUAGE {
	C,
	CPP,
	PYTHON,
	JAVA
}
public class TestWindow : EditorWindow {

	TestData data;

	SerializedObject so;
	private List<string> props;
	private List<string> nestedProps;

	bool toggle;

	[MenuItem ("Test Window/Open")]
	public static void OpenWindow () {
		var window = (TestWindow)EditorWindow.GetWindow <TestWindow> ("Test Window", true);
	}

	void OnEnable () {
		data = ScriptableObject.CreateInstance <TestData> ();
		data.id = 0;
		data.name = "fdj";
		data.gender = Gender.MALE;
		data.skills = new List<Skill> ();

		data.skills.Add (new Skill (0, LANGUAGE.C, 4.5f));
		data.skills.Add (new Skill (1, LANGUAGE.PYTHON, 1.5f));

		so = new SerializedObject (data);

		FieldInfo[] fields = data.GetType ().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		props = new List<string> ();
		for (int i = 0; i < fields.Length; i++) {
			props.Add (fields [i].Name);
		}

		FieldInfo[] skillFields = data.skills[0].GetType ().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		nestedProps = new List<string> ();
		for (int i = 0; i < skillFields.Length; i++) {
			nestedProps.Add (skillFields [i].Name);
		}
	}

	void OnGUI () {

		if (data == null)
			return;

		so.Update ();
	
		for (int propIndex = 0; propIndex < props.Count; propIndex++) {
			var sp = so.FindProperty (props [propIndex]);
			if (!sp.hasVisibleChildren) {
				EditorGUILayout.PropertyField (sp);
			} else {
				toggle = EditorGUILayout.Foldout (toggle, sp.displayName);
				if (toggle) {
					for (int spIndex = 0; spIndex < sp.arraySize; spIndex++) {
						GUILayout.BeginVertical ("box");
						var item = sp.GetArrayElementAtIndex (spIndex);
						for (int itemIndex = 0; itemIndex < nestedProps.Count; itemIndex++) {
							var nestedSP = item.FindPropertyRelative (nestedProps[itemIndex]);
							EditorGUILayout.PropertyField (nestedSP);
						}
						GUILayout.EndVertical ();
					}
				}
			}
		}
		so.ApplyModifiedProperties ();
	}
}
#endif

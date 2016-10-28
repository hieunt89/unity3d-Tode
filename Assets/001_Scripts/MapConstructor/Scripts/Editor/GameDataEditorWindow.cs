using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

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

[Serializable]
public class CustomData : ScriptableObject {
	public int id;
	public string name;
	public Gender gender;
	[SerializeField] public List<Skill> skills;
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

public interface IGameDataWindow {
	void OnInit ();
	void OnGUI ();
	void ResetGUI ();
}

public class GameDataWindow <T> : IGameDataWindow where T : ScriptableObject {
	
	T data;

	SerializedObject so;
	List<string> props;
	List<string> nestedProps;

	bool toggle;

	public void OnInit ()
	{
		data = ScriptableObject.CreateInstance<T> ();

		so = new SerializedObject (data);

		FieldInfo[] fields = data.GetType().GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);


		props = new List<string> ();
		for (int i = 0; i < fields.Length; i++) {
			props.Add (fields [i].Name);
			if (typeof(IList).IsAssignableFrom(fields[i].FieldType)) {	// check if this is a list
				Debug.Log (fields[i].Name + " / " + data.GetType().GetField(fields[i].Name));
//				fields [i].FieldType.GetGenericArguments () [0];
//				data.GetType ().GetField (fields [i].Name) = new List <> (); //typeof(data.GetType().GetField(fields[i].Name).GetType())> ();
				nestedProps = new List<string> ();
				FieldInfo[] nestedFields = fields[i].FieldType.GetGenericArguments()[0].GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);	// get fields of items in the list
				for (int j = 0; j < nestedFields.Length; j++) {
					nestedProps.Add (nestedFields [j].Name);
				}
			}
		}
		Debug.Log (props.Count);
		Debug.Log (nestedProps.Count);

	}

	public void OnGUI () {
		if (data == null) {
			data = DataManager.Instance.LoadData <T> ();
			return;
		}	
		so.Update ();

		for (int propIndex = 0; propIndex < props.Count; propIndex++) {
			var sp = so.FindProperty (props [propIndex]);
			if (!sp.hasVisibleChildren) {
				EditorGUILayout.PropertyField (sp);
			} else {
				toggle = EditorGUILayout.Foldout (toggle, sp.displayName);
				if (toggle) {
//					Debug.Log ();
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

		if (GUILayout.Button("Save")) {
			DataManager.Instance.SaveData <T> (data);
		}
		if (GUILayout.Button("Load")) {
			data = DataManager.Instance.LoadData <T> ();
		}
		so.ApplyModifiedProperties();
	}

	public void ResetGUI () {
		data = null;
	}
}

public class GameDataEditorWindow : EditorWindow {

	Type _type = null;
	object genericWindow = null;

	public Type Type {
		get { return _type.GetGenericArguments()[0]; }
		set {
			_type = typeof(GameDataWindow <>).MakeGenericType(value);

			genericWindow = System.Activator.CreateInstance(_type);
		}
	}

	[MenuItem("Game Data/Open Editor &T")]
	public static void ShowWindow()
	{
		var window = EditorWindow.GetWindow <GameDataEditorWindow> ("Game Data Editor",true);
		window.minSize = new Vector2 (500, 400);
	}

	void OnGUI () {

//		EditorGUILayout.
		if (genericWindow == null) {
			GUILayout.Label ("Select Game Data");
			if (GUILayout.Button("CustomData")) {
				Type = typeof(CustomData);
				(genericWindow as IGameDataWindow).OnInit ();
			}
//			if (GUILayout.Button("TowerData")) {
//				Type = typeof(TowerData);
//				(genericWindow as IGameDataWindow).OnInit ();
//			}
//			if (GUILayout.Button("CharacterData")) {
//				Type = typeof(CharacterData);
//				(genericWindow as IGameDataWindow).OnInit ();
//			}
//			if (GUILayout.Button("ProjectileData")) {
//				Type = typeof(ProjectileData);
//				(genericWindow as IGameDataWindow).OnInit ();
//			}
//			if (GUILayout.Button("CombatSkillData")) {
//				Type = typeof(CombatSkillData);
//				(genericWindow as IGameDataWindow).OnInit ();
//			}
//			if (GUILayout.Button("SummonSkillData")) {
//				Type = typeof(SummonSkillData);
//				(genericWindow as IGameDataWindow).OnInit ();
//			}
			return;
		}
		(genericWindow as IGameDataWindow).OnGUI ();
		if (GUILayout.Button("Reset")) {
			(genericWindow as IGameDataWindow).ResetGUI ();
			genericWindow = null;
		}

	}
}

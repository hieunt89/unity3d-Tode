using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;

[Serializable]
public enum Gender {
	MALE,
	FEMALE
}

[Serializable]
public enum LANGUAGE {
	C,
	CPP,
	PYTHON,
	JAVA
}

public class FinalCustomData {
	public string id;
	public string name;
 	public Gender gender;
 	public List<Skill> skills;

}

[Serializable]
public class CustomData : ScriptableObject {
	FinalCustomData finalData;
}

[Serializable]
public class Skill {
	[SerializeField] public int id;
//	[SerializeField] public LANGUAGE language;
	[SerializeField] public float year;

}

public interface IGameDataWindow {
	void OnInit ();
	void OnGUI ();
	void ResetGUI ();
}

public class GameDataWindow <T> : IGameDataWindow, IInjectDataUtils where T : ScriptableObject {
	
	T data;

	SerializedObject so;
	List<string> props;
	List<string> nestedProps;

	bool toggle;

	IDataUtils dataUtils;

	public void SetDataUtils (IDataUtils dataUtils)
	{
		this.dataUtils = dataUtils;
	}

	public void OnInit ()
	{
		data = ScriptableObject.CreateInstance<T> ();
		
		Type dataType = data.GetType ();
		FieldInfo[] dataFields = dataType.GetFields (BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		props = new List<string> ();
		for (int i = 0; i < dataFields.Length; i++) {
			props.Add (dataFields [i].Name);
			if (typeof(IList).IsAssignableFrom(dataFields[i].FieldType)) {	// check if this is a list
				var listType = typeof (List<>).MakeGenericType(dataFields [i].FieldType.GetGenericArguments () [0]);	// get list type from field
				Debug.Log(listType);
				var fieldValue = dataFields[i].GetValue (data);		// 
				fieldValue = Activator.CreateInstance (listType);	// create instance of list with list type

				if (listType.GetGenericArguments()[0].IsPrimitive || listType.GetGenericArguments()[0] == typeof(Decimal) || listType.GetGenericArguments()[0] == typeof(String)){
//					Debug.Log ("is primitive");
				} else {
					nestedProps = new List<string> ();
					FieldInfo[] nestedFields = dataFields[i].FieldType.GetGenericArguments()[0].GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);	// get fields of items in the list
					for (int j = 0; j < nestedFields.Length; j++) {
						nestedProps.Add (nestedFields [j].Name);
					}
				}
			}
		}
		so = new SerializedObject (data);
	}

	public void OnGUI () {
		if (data == null) {
//			data = DataManager.Instance.LoadData <T> ();
			return;
		}	
		so.Update ();

		EditorGUILayout.LabelField (typeof(T).Name);
		for (int propIndex = 0; propIndex < props.Count; propIndex++) {
			var sp = so.FindProperty (props [propIndex]);
			if (!sp.hasVisibleChildren) {
				EditorGUILayout.PropertyField (sp);
			} else {
				toggle = EditorGUILayout.Foldout (toggle, sp.displayName + " (" +  sp.arraySize + " items)");
				if (toggle) {
					if (GUILayout.Button ("Add Item")) {
						sp.InsertArrayElementAtIndex (sp.arraySize);
					}
					for (int spIndex = 0; spIndex < sp.arraySize; spIndex++) {
						GUILayout.BeginVertical ("box");
						var item = sp.GetArrayElementAtIndex (spIndex);
						if (nestedProps != null) {
							for (int itemIndex = 0; itemIndex < nestedProps.Count; itemIndex++) {
								var nestedSP = item.FindPropertyRelative (nestedProps[itemIndex]);
								EditorGUILayout.PropertyField (nestedSP);
							} 
						} else {
							EditorGUILayout.PropertyField (item);
						}
						if (GUILayout.Button ("Remove Item")) {
							sp.DeleteArrayElementAtIndex (spIndex);
							continue;
						}
						GUILayout.EndVertical ();
					}
				}
			}
		}

		if (GUILayout.Button("Save")) {
			dataUtils.CreateData <T> (data);
		}
		if (GUILayout.Button("Load")) {
			data = dataUtils.LoadData <T> ();
		}
		so.ApplyModifiedProperties();
	}

	public void ResetGUI () {
		data = null;
	}
}

public class GenericGameDataEditorWindow : EditorWindow {

	Type _type = null;
	object genericWindow = null;

	public Type Type {
		get { return _type.GetGenericArguments()[0]; }
		set {
			_type = typeof(GameDataWindow <>).MakeGenericType(value);

			genericWindow = System.Activator.CreateInstance(_type);
		}
	}

	[MenuItem("Beta/Game Data Editor &G")]
	public static void ShowWindow()
	{
		var window = EditorWindow.GetWindow <GenericGameDataEditorWindow> ("Game Data Editor",true);
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
			if (GUILayout.Button("TowerData")) {
				Type = typeof(TowerData);
				(genericWindow as IGameDataWindow).OnInit ();
			}
			if (GUILayout.Button("CharacterData")) {
				Type = typeof(CharacterData);
				(genericWindow as IGameDataWindow).OnInit ();
			}
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

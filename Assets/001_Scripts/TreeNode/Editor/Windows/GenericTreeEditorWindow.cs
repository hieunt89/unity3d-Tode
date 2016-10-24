using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CustomData {
	int id;
}

public interface IGenericTreeWindow {
	void InitGUI (string _treeName);
	void OnGUI ();
}

public class GenericTreeWindow <T> : IGenericTreeWindow {

	#region Singleton
	private static GenericTreeWindow <T> instance = null;
	public static GenericTreeWindow <T> Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GenericTreeWindow <T>();
			}
			return instance;
		}
	}
	#endregion

	public WorkView workView;
	public PropertiesView propertiesView;

	public GenericTree<T> currentTree;

	public void InitGUI (string _treeName)
	{
		workView = new WorkView();
		propertiesView = new PropertiesView();

		currentTree = new GenericTree<T>(_treeName);
	}

	public void OnGUI ()
	{
		if (workView == null || propertiesView == null) {
			return;
		}

		Event e = Event.current;

		ProcessEvent (e);

		workView.UpdateView <T> (new Rect (0, 0, 600, 400), new Rect (0f, 0f, 1f, 1f) , e, currentTree);

	}

	public void ProcessEvent (Event _event) {

	}


}

public enum TreeType {
	None,
	Test,
	Tower,
	CombatSkill,
	SummonSkill
}

public class GenericTreeEditorWindow : EditorWindow {

	object genericTreeWindow = null;
	Type type = null;

	string treeName = "Enter tree name ...";
	TreeType treeType;

	private Vector2 scrollPosition;
	private Rect virtualRect;
	private float virtualPadding = 50f;
	private float minX, minY, maxX, maxY;

	public static void InitGenericEditorWindow () {
		var currentWindow = (GenericTreeEditorWindow)EditorWindow.GetWindow <GenericTreeEditorWindow> ();
		currentWindow.titleContent = new GUIContent ("Generic Tree");
		currentWindow.minSize = new Vector2 (600, 400);
//		CreateViews ();
	}

	void OnEnable () {
		virtualRect = new Rect (0f, 0f, position.width, position.height);
//		minX = minY = maxX = maxY = 0f;
	}

	void OnGUI () {
//		scrollPosition =  GUI.BeginScrollView(new Rect(0f, 0f, position.width, position.height), scrollPosition, virtualRect); // <-- need to customize this viewrect (expandable by nodes + offset)
//		BeginWindows ();
		if (genericTreeWindow == null) {
			treeName = EditorGUILayout.TextField ("Tree Name", treeName);
			treeType = (TreeType) EditorGUILayout.EnumPopup (treeType);

			GUI.enabled = !string.IsNullOrEmpty (treeName) && treeName != "Enter tree name ..." && treeType != TreeType.None ? true : false;
			if (GUILayout.Button ("Create Tree")) {
				switch(treeType){
				case TreeType.Test:
					type = typeof(GenericTreeWindow <>).MakeGenericType(typeof(CustomData));
					break;
				case TreeType.Tower:
					type = typeof(GenericTreeWindow <>).MakeGenericType(typeof(TowerData));
					break;
				case TreeType.CombatSkill:
					type = typeof(GenericTreeWindow <>).MakeGenericType(typeof(CombatSkillData));
					break;
				case TreeType.SummonSkill:
					type = typeof(GenericTreeWindow <>).MakeGenericType(typeof(SummonSkillData));
					break;
				}
				genericTreeWindow = Activator.CreateInstance(type);
				(genericTreeWindow as IGenericTreeWindow).InitGUI (treeName);
			}
			GUI.enabled = true;
			return;
		}
		(genericTreeWindow as IGenericTreeWindow).OnGUI ();
//		EndWindows ();
//		GUI.EndScrollView ();

		Repaint ();
	}
}
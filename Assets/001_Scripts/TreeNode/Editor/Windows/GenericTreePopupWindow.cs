using UnityEngine;
using UnityEditor;
using System;

public interface IGenericTreePopup {
	void InitGUI ();
	void OnGUI ();
}

public class GenericTreePopup <T> : IGenericTreePopup {
	public void InitGUI ()
	{
	}

	public void OnGUI ()
	{
	}
}

public class GenericTreePopupWindow : EditorWindow {

	object genericPopup;
	Type type;
	Type givenType;

	public static void InitGenericTreePopup (Type _type) {
//		type = typeof(GenericTreePopup <>).MakeGenericType(typeof(CustomData));
//		genericPopup = Activator.CreateInstance(type);
//		(genericPopup as IGenericTreeWindow).InitGUI ();
		var currentPopup = (GenericTreePopupWindow)EditorWindow.GetWindow <GenericTreePopupWindow> ("Root", true);

	}

	private void OnGUI () {
		if (genericPopup == null) {
			type = typeof(GenericTreeWindow <>).MakeGenericType(typeof(CustomData));
		}
	}
}

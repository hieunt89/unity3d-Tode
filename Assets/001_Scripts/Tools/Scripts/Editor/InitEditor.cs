using UnityEngine;
using System.Collections;
using UnityEditor;

#if UNITY_EDITOR
[InitializeOnLoad]
public class InitEditor {
	static InitEditor()
	{
		Debug.Log("Up and running");
		DIContainer.BindModules ();
	}

	public void Quit () {
		AssetDatabase.SaveAssets ();
	}
}
#endif

using UnityEngine;
using System.Collections;
using UnityEditor;

[InitializeOnLoad]
public class InitEditor {
	static InitEditor()
	{
		Debug.Log("Up and running");
		DIContainer.BindModules ();

	}
}

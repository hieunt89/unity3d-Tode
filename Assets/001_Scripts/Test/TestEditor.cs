using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
[CustomEditor (typeof (Test))]
public class TestEditor : Editor {


	void Start () {
//		Tree<string> t = new Tree<string> ("root");
//		t.Root.AddChild ("node1").AddChild ("node2");
//
//		t.Root.Children [0].AddChild ("node3").AddChild ("node4");
//		t.Root.Children [0].Children[0].AddChild ("node5").AddChild("node6");
//
//		Node<string> n = t.Root.FindChildNodeByData ("node3");
//		Debug.Log (n.Data + " " + n.Children.Count);
	}

	void OnEnable () {

	}
	void OnSceneGUI () {
//		GUILayout.BeginArea ();
		Test t = target as Test;

		Handles.DrawBezier (new Vector3(0f, 0f, 0f), t.transform.position, t.startTangent, t.endTangent, Color.white, null, 1f);

//		GUILayout.EndArea ();

	}
}

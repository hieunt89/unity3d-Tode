using UnityEngine;
using System.Collections.Generic;
using System;
using Entitas;
using UnityEditor;

public class TestData1 : ScriptableObject {
	public Tree<string> tree;
}
public class Test : MonoBehaviour {
	TestData1 mData;
	void Start () {
		mData = ScriptableObject.CreateInstance <TestData1> ();

		mData.tree = new Tree<string> ("root");
		mData.tree.Root.AddChild ("child1");

		Debug.Log (mData.tree.Root.children[0].data);
	}
}


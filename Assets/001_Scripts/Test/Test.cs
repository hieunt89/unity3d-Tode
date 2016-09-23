﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Test : MonoBehaviour {
	void Start () {
		Tree<string> t = new Tree<string> ("root");
		t.Root.AddChild ("node1").AddChild ("node2");

		t.Root.Children [0].AddChild ("node3").AddChild ("node4");
		t.Root.Children [0].Children[0].AddChild ("node5").AddChild("node6");

		Node<string> n = t.Root.FindChildNodeByData ("node3");
		Debug.Log (n.Data + " " + n.Children.Count);
	}
}

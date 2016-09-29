using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Test : MonoBehaviour {
	Tree<string> tree;
	void Start(){
		tree = new Tree<string> ("root");
	}

	public void AddNode(){
		Node<string> n = new Node<string> ("test1");
		Node<string> n2 = new Node<string> ("test2");


	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class Test : MonoBehaviour {
	Tree<string> tree;
	void Start(){
		tree = new Tree<string> ();
		Node<string> root = new Node<string> ("root");
		tree.Root = root;

		Node<string> n = new Node<string> ("test1");
		Node<string> n2 = new Node<string> ("test2");

		n.AddParent (root);
	}
}

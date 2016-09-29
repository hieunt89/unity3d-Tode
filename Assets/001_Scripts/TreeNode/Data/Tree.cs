using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Tree <T> where T : class {
	public Node <T> Root;

//	public Tree (TreeType treeType, string treeName){
//		this.treeType = treeType;
//		this.treeName = treeName;
//		this.Root = new Node<T> ();
//	}
//
	public Tree (T rootData)
	{
		this.Root = new Node<T> (rootData);
	}
}


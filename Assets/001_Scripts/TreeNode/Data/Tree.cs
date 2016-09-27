using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tree <T> where T : class {
	public TreeType treeType;
	public string treeName;
	public Node <T> Root;

	public Tree () {

	}

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

	public Tree (TreeType treeType, string treeName, T rootData)
	{
		this.treeType = treeType;
		this.treeName = treeName;
		this.Root = new Node<T> (rootData);
	}
}


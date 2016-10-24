using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Tree <T> {
	public string treeName;
	public Node <T> Root;

	public Tree(){
	}

	public Tree (T rootData)
	{
		this.Root = new Node<T> (rootData);
	}

	public Tree (string treeName, Node<T> root)
	{
		this.treeName = treeName;
		this.Root = root;
	}
}


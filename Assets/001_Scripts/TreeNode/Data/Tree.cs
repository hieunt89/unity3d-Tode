using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Tree <T> where T : class {
	public Node <T> Root;

	public Tree (T rootData)
	{
		this.Root = new Node<T> (rootData);
	}

	public Tree ()
	{
		
	}
}


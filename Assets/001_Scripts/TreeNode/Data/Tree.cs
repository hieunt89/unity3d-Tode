using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tree <T> where T : class {
	public Node <T> Root;

	public Tree (T rootData)
	{
		this.Root = new Node<T> (rootData);
	}
}


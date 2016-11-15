using UnityEngine;
using System;
using System.Collections.Generic;

public enum TreeType {
	Towers,
	CombatSkills,
	SummonSkills
}

[Serializable]
public class Tree <T> where T : class {
	public TreeType treeType;
	public string id;
	public Node <T> Root;

	public Tree(){
	}

	public Tree (T rootData)
	{
		this.Root = new Node<T> (rootData);
	}

	public Tree (TreeType treeType, string id)
	{
		this.treeType = treeType;
		this.id = id;
	}
}


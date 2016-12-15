using UnityEngine;
using System;
using System.Collections.Generic;

public enum TreeType {
	None,
	Towers,
	CombatSkills,
	SummonSkills
}

[Serializable]
public class Tree <T> : ScriptableObject where T : class{
	public TreeType treeType;
	public string id;
	public string name;
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


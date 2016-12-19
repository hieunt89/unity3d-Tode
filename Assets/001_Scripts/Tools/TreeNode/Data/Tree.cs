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
public class Tree <T> {
	public string id;
	public TreeType treeType;
	public string name;
	public Node <T> Root;

	public Tree(){
	}


	public Tree (string id, TreeType treeType, string name, Node<T> root)
	{
		this.id = id;
		this.treeType = treeType;
		this.name = name;
		this.Root = root;
	}
	
}


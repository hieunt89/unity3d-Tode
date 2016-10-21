using UnityEngine;
using System;
using System.Collections.Generic;

public enum TreeType {
	Towers,
	CombatSkills,
	SummonSkills
}
	
[Serializable]
public class Tree <T> { //where T : class {
	public TreeType treeType;
	public string treeName;
	public Node <T> Root;

	public Tree(){
	}

//	public Tree (T rootData)
//	{
//		this.Root = new Node<T> (rootData);
//	}
	public Tree (string treeName, Node<T> root)
	{
		this.treeName = treeName;
		this.Root = root;
	}
	
	public Tree (TreeType treeType, string treeName, Node<T> root)
	{
		this.treeType = treeType;
		this.treeName = treeName;
		this.Root = root;
	}
	

}


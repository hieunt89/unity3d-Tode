using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node <T> where T : class {
	public T Data;
	public Node<T> parent;
	public List<Node<T>> Children;

	public Node (){
		Children = new List<Node<T>> ();
	}

	public Node (T data){
		Data = data;
		Children = new List<Node<T>> ();
	}

	public Node <T> AddChild(T data){
		Node<T> n = new Node<T> (data);
		Children.Add (n);
		n.parent = this;
		return this;
	}

	public void AddParent(Node<T> n){
		this.parent = n;
		n.Children.Add(this);
	}

	public Node <T> FindChildNodeByData (T data){
		return FindChildNodeByData (this, data);
	}

	Node <T> FindChildNodeByData(Node<T> node, T data){
		for (int i = 0; i < node.Children.Count; i++) {
			if(node.Children[i].Data == data){
				return node.Children [i];
			}
			var result = node.Children [i].FindChildNodeByData (node.Children [i], data);
			if(result != null){
				return result;
			}
		}
		return null;
	}
}

public class Tree <T> where T : class {
	public TreeType treeType;
	public string treeName;
	public Node <T> Root;

	public Tree () {

	}

	public Tree (TreeType treeType, string treeName){
		this.treeType = treeType;
		this.treeName = treeName;
		this.Root = new Node<T> ();
	}

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


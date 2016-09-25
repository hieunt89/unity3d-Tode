using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Node <T> where T : class {
	public T Data;
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
		return this;
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

public class Tree <T> where T : class{
	public Node <T> Root;

	public Tree (){
		Root = new Node<T> ();
	}

	public Tree (T rootData)
	{
		Root = new Node<T> (rootData);
	}
}


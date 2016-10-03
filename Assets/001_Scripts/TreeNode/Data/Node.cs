using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Node <T> where T : class  {

	public T data;
	public Node<T> parent;
	public List<Node<T>> children;

	public Node (T data) {
		this.data = data;
		this.children = new List<Node<T>> ();
	}

	public Node <T> AddRelationship(T data){
		Node<T> n = new Node<T> (data);
		children.Add (n);
		n.parent = this;
		return this;
	}

	public Node <T> AddRelationship(Node<T> nodeData){
		children.Add (nodeData);
		nodeData.parent = this;
		return this;
	}

	public Node <T> FindChildNodeByData (T data){
		return FindChildNodeByData (this, data);
	}

	Node <T> FindChildNodeByData(Node<T> node, T data){
		for (int i = 0; i < node.children.Count; i++) {
			if(node.children[i].data == data){
				return node.children [i];
			}
			var result = node.children [i].FindChildNodeByData (node.children [i], data);
			if(result != null){
				return result;
			}
		}
		return null;
	}
}

using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Node <T> where T : class  {

	public T Data;
	public Node<T> parent;
	public List<Node<T>> Children;
	public int depth;

	public Node (){
		Children = new List<Node<T>> ();
		depth = 0;
	}

	public Node (T data){
		Data = data;
		Children = new List<Node<T>> ();
		depth = 0;
	}

	public Node <T> AddChild(T data){
		Node<T> n = new Node<T> (data);
		Children.Add (n);
		n.parent = this;
		UpdateDepth (this);
		return this;
	}

	public Node <T> AddChild(Node<T> child){
		Children.Add (child);
		child.parent = this;
		UpdateDepth (this);
		return this;
	}

	public void RemoveChild(Node<T> child){
		child.parent.Children.Remove (child);
		child.parent = null;
	}

	public Node <T> FindChildNodeByData (T data){
		return FindChildNodeByData (this, data);
	}

	void UpdateDepth(Node<T> parent){
		for (int i = 0; i < parent.Children.Count; i++) {
			parent.Children [i].depth = parent.depth + 1;
			UpdateDepth (parent.Children [i]);
		}
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

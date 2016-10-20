using UnityEngine;
using System;
using System.Collections.Generic;

public enum NodeType {
	RootNode,
	Node
}

[Serializable]
public class Node <T> { //where T : class  {

	public T data;
	public Node<T> parent;
	public List<Node<T>> children;
	public int depth;

	public Node (T data){
		this.data = data;
		this.children = new List<Node<T>> ();
		this.depth = 0;
	}

	public Node <T> AddChild(T data){
		Node<T> n = new Node<T> (data);
		this.children.Add (n);
		n.parent = this;
		UpdateDepth (this);
		return this;
	}
		
	public Node <T> AddChild(Node<T> child){
		this.children.Add (child);
		child.parent = this;
		UpdateDepth (this);
		return this;
	}

	public void RemoveChild (Node<T> child) {
		child.parent.children.Remove (child);
		child.parent = null;
	}

//	public Node <T> FindChildNodeByData (T data){
//		return FindChildNodeByData (this, data);
//	}

	void UpdateDepth(Node<T> parent){
		for (int i = 0; i < parent.children.Count; i++) {
			parent.children [i].depth = parent.depth + 1;
			UpdateDepth (parent.children [i]);
		}
	}

//	Node <T> FindChildNodeByData(Node<T> node, T data){
//		for (int i = 0; i < node.children.Count; i++) {
//			if(node.children[i].data == data){
//				return node.children [i];
//			}
//			var result = node.children [i].FindChildNodeByData (node.children [i], data);
//			if(result != null){
//				return result;
//			}
//		}
//		return null;
//	}
}

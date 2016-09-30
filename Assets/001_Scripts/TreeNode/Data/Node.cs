using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Node <T> where T : class  {

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

	public Node <T> AddRelationship(T data){
		Node<T> n = new Node<T> (data);
		Children.Add (n);
		n.parent = this;
		return this;
	}

	public Node <T> AddRelationship(Node<T> nodeData){
		Children.Add (nodeData);
		nodeData.parent = this;
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

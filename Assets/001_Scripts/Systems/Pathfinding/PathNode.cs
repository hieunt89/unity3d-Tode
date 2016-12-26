using UnityEngine;
using System.Collections;
using Priority_Queue;
using System.Collections.Generic;

public class PathNode : FastPriorityQueueNode{
	public Vector3 position;
	public float moveCost;
	public PathNode cameFrom;

	public PathNode (Vector3 position)
	{
		this.position = position;
	}

	public PathNode (Vector3 position, float cost)
	{
		this.position = position;
		this.moveCost = cost;
	}

	public static readonly Vector3[] neighbors = {
		new Vector3(-1, 0, 1), //TopLeft
		new Vector3( 0, 0, 1), //TopMid
		new Vector3( 1, 0, 1), //TopRight
		new Vector3( 1, 0, 0), //MidRight
		new Vector3( 1, 0,-1), //BotRight
		new Vector3( 0, 0,-1), //BotMid
		new Vector3(-1, 0,-1), //BotLeft
		new Vector3(-1, 0, 0)  //Midleft
	};
}

public class PathNodeList{
	public Dictionary<Vector3, PathNode> nodes;
	public PathNodeList (){
		nodes = new Dictionary<Vector3, PathNode> ();
	}

	public void Add(PathNode node){
		nodes.Add (node.position, node);
	}

	public void AddOrUpdate(PathNode node){
		if (nodes.ContainsKey (node.position)) {
			nodes [node.position] = node;
		} else {
			nodes.Add (node.position, node);
		}
	}

	public void Clear(){
		nodes.Clear ();
	}

	public bool NotContainsOrHasHigherCost(PathNode node, float newCost){
		if (nodes.ContainsKey (node.position)) {
			if (nodes [node.position].moveCost > newCost) {
				return true;
			} else {
				return false;
			}
		} else {
			return true;
		}
	}
}

using UnityEngine;
using System.Collections;
using Priority_Queue;
using System.Collections.Generic;

public class PathNode : FastPriorityQueueNode{
	public Vector3 position;
	public int moveCost;
	public PathNode cameFrom;

	public PathNode (Vector3 position)
	{
		this.position = position;
	}

	public PathNode (Vector3 position, int cost)
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

	public static readonly Dictionary<Vector3, int> neighbors2 = new Dictionary<Vector3, int>(){
		{new Vector3(-1, 0, 1), 2}, //TopLeft
		{new Vector3( 0, 0, 1), 1}, //TopMid
		{new Vector3( 1, 0, 1), 2}, //TopRight
		{new Vector3( 1, 0, 0), 1}, //MidRight
		{new Vector3( 1, 0,-1), 2}, //BotRight
		{new Vector3( 0, 0,-1), 1}, //BotMid
		{new Vector3(-1, 0,-1), 2}, //BotLeft
		{new Vector3(-1, 0, 0), 1}  //Midleft
	};

}

public class PathNodeList{
	public List<PathNode> nodes;
	public PathNodeList (){
		nodes = new List<PathNode> ();
	}

	public void Add(PathNode node){
		nodes.Add (node);
	}

	public void AddOrUpdate(PathNode node){
		for (int i = 0; i < nodes.Count; i++) {
			if (nodes[i].position == node.position) {
				nodes [i] = node;
				return;
			}
		}
		nodes.Add (node);
	}

	public bool NotContainsOrHasHigherCost(PathNode node, int newCost){
		for (int i = 0; i < nodes.Count; i++) {
			if (node.position == nodes[i].position) {
				if (nodes [i].moveCost > newCost) {
					return true;
				} else {
					return false;
				}
			}
		}
		return true;
	}
}

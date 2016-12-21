using UnityEngine;
using System.Collections;
using Priority_Queue;

public class PathNode : FastPriorityQueueNode{
	public Vector3 position;
	public PathNode parent;
	public float GScore;
	public float HScore;
	public float FScore;

	public PathNode (Vector3 position)
	{
		this.position = position;
	}
}

using UnityEngine;
using System.Collections;
using Priority_Queue;

public class PathNode : FastPriorityQueueNode{
	public Vector3 position;
	public int moveCost;

	public PathNode (Vector3 position)
	{
		this.position = position;
	}

	public PathNode (Vector3 position, int cost)
	{
		this.position = position;
		this.moveCost = cost;
	}
}

public static class Neighbors{
	public static readonly Vector3 TopLeft  = new Vector3(-1, 0, 1);
	public static readonly Vector3 TopMid   = new Vector3( 0, 0, 1);
	public static readonly Vector3 TopRight = new Vector3( 1, 0, 1);
	public static readonly Vector3 MidLeft  = new Vector3(-1, 0, 0);
	public static readonly Vector3 MidRight = new Vector3( 1, 0, 0);
	public static readonly Vector3 BotLeft  = new Vector3(-1, 0,-1);
	public static readonly Vector3 BotMid   = new Vector3( 0, 0,-1);
	public static readonly Vector3 BotRight = new Vector3( 1, 0,-1);
}

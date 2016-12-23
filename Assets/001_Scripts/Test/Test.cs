using UnityEngine;
using System.Collections.Generic;
using System;
using Entitas;
using UnityEditor;
using Priority_Queue;
using System.Linq;
using System.Collections;

public class Test : MonoBehaviour {

	void Start () {
		StartCoroutine (FindPath (Vector3.zero, new Vector3(0,0,20)));
	}

	List<PathNode> GetNeighbors(PathNode current){
		List<PathNode> neighbors = new List<PathNode> ();

		foreach (var item in PathNode.neighbors2) {
			var node = GetNeighborNode (current.position, item.Key);
			if (node != null) {
				node.moveCost = item.Value;
				neighbors.Add (node);
			}
		}

//		for (int i = 0; i < PathNode.neighbors.Length; i++) {
//			var node = GetNeighborNode (current.position, PathNode.neighbors[i]);
//			if (node != null) {
//				neighbors.Add (node);
//			}
//		}

		return neighbors;
	}

	PathNode GetNeighborNode(Vector3 current, Vector3 next){
		var nextPos = current + next;

		if (Physics.Raycast(current, next, Vector3.Distance(current, next))) {
			return null;
		}

		return new PathNode (nextPos, 1);
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 goalPos){
		var start = new PathNode (startPos, 0);
		SimplePriorityQueue<PathNode> frontier = new SimplePriorityQueue<PathNode> ();
		frontier.Enqueue (start, 0);

		PathNodeList exploredNodes = new PathNodeList ();
		exploredNodes.Add (start);

		PathNode current;
		List<PathNode> neighbors;
		while (frontier.Count > 0) {
			current = frontier.Dequeue ();

			neighbors = GetNeighbors (current);
			for (int i = 0; i < neighbors.Count; i++) {
				var next = neighbors [i];
				var newCost = current.moveCost + next.moveCost;
				if (exploredNodes.NotContainsOrHasHigherCost(next, newCost)) {
					DebugDrawNode (next, newCost.ToString());
					Debug.DrawLine (current.position, next.position, Color.red, 1f);
					next.moveCost = newCost;
					next.cameFrom = current;
					var priority = newCost + GetHScore(next.position, goalPos) * 2;
					exploredNodes.AddOrUpdate (next);

					if (IsReachedGoal(goalPos, next)) { //Reached goal
						var goal = new PathNode (goalPos);
						goal.cameFrom = next;
						DebugDrawPath (ReconstructPath(goal, ref start), startPos);
						yield break;
					}

					frontier.Enqueue (next, priority); //smaller priority go first
					yield return null;
				}


			}
		}
	}

	Queue<PathNode> ReconstructPath(PathNode goal, ref PathNode start){
		PathNode current = goal;
		Queue<PathNode> path = new Queue<PathNode> ();
		path.Enqueue (current);

		while (current != start) {
			current = current.cameFrom;
			path.Enqueue (current);
		}

		path = new Queue<PathNode> (path.Reverse ());

		return path;
	}

	bool IsReachedGoal(Vector3 goal, PathNode current){
		if (Vector3.Distance (current.position, goal) <= ConstantData.CLOSE_COMBAT_RANGE) {
			return true;
		} else {
			return false;
		}
	}

	float GetHScore(Vector3 a, Vector3 b){
		return Vector3.Distance(a, b);
	}

	void DebugDrawPath(Queue<PathNode> path, Vector3 start){
		int i = 0;
		while (path.Count > 0) {
			var node = path.Dequeue ();
			DebugDrawNode (node, i.ToString());
			Debug.DrawLine (start, node.position, Color.blue, Mathf.Infinity);
			start = node.position;
			i++;
		}
	}

	GameObject DebugDrawNode(PathNode node, string name){
		var go = GameObject.CreatePrimitive (PrimitiveType.Cube);
		go.transform.position = node.position;
		go.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		go.name = "nodePath " + name;
		return go;
	}
}


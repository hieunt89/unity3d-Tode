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
		StartCoroutine (FindPath (Vector3.zero, new Vector3(5,5,5)));
	}

	List<PathNode> GetNeighbors(PathNode current){
		List<PathNode> neighbors = new List<PathNode> ();

		neighbors.Add (new PathNode (current.position + Neighbors.TopLeft,  2));
		neighbors.Add (new PathNode (current.position + Neighbors.TopMid,   1));
		neighbors.Add (new PathNode (current.position + Neighbors.TopRight, 2));
		neighbors.Add (new PathNode (current.position + Neighbors.MidRight, 1));
		neighbors.Add (new PathNode (current.position + Neighbors.BotRight, 2));
		neighbors.Add (new PathNode (current.position + Neighbors.BotMid,   1));
		neighbors.Add (new PathNode (current.position + Neighbors.BotLeft,  2));
		neighbors.Add (new PathNode (current.position + Neighbors.MidLeft,  1));

		return neighbors;
	}

	IEnumerator FindPath(Vector3 startPos, Vector3 goalPos){
		var start = new PathNode (startPos);
		SimplePriorityQueue<PathNode> frontier = new SimplePriorityQueue<PathNode> ();
		frontier.Enqueue (start, 0);

		Dictionary<PathNode, PathNode> cameFrom = new Dictionary<PathNode, PathNode> ();
		cameFrom.Add (start, null);

		Dictionary<PathNode, int> costSoFar = new Dictionary<PathNode, int> ();
		costSoFar.Add (start, 0);

		PathNode current;
		List<PathNode> neighbors;
		while (frontier.Count > 0) {
			current = frontier.Dequeue ();

			neighbors = GetNeighbors (current);
			for (int i = 0; i < neighbors.Count; i++) {
				var next = neighbors [i];
				var newCost = costSoFar [current] + next.moveCost;
				if (!costSoFar.ContainsKey(next) || newCost < costSoFar[next]) {
					DebugDrawNode (next, newCost.ToString());
					Debug.DrawLine (current.position, next.position, Color.red, 2f);
					costSoFar[next] = newCost;
					cameFrom[next] = current;

					//					if (IsReachedGoal(goalPos, next)) { //Reached goal
					//						var goal = new PathNode (goalPos);
					//						cameFrom.Add (goal, next);
					//						return ReconstructPath (goal, ref cameFrom, ref start);
					//					}

					var priority = newCost;
					frontier.Enqueue (next, priority); //smaller priority go first
				}

				yield return new WaitForSeconds (2f);
			}
		}
	}

	Queue<PathNode> ReconstructPath(PathNode goal, ref Dictionary<PathNode, PathNode> cameFrom, ref PathNode start){
		PathNode current = goal;
		Queue<PathNode> path = new Queue<PathNode> ();
		path.Enqueue (current);

		while (current != start) {
			current = cameFrom [current];
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
		return Mathf.Abs (a.x - b.x) + Mathf.Abs (a.y - b.y);
	}

	void DebugDrawPath(Queue<PathNode> path, Vector3 start){
		int i = 0;
		while (path.Count > 0) {
			var node = path.Dequeue ();
			var go = DebugDrawNode (node, i.ToString());
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


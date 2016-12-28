﻿using UnityEngine;
using System.Collections.Generic;
using System;
using Entitas;
using UnityEditor;
using Priority_Queue;
using System.Linq;
using System.Collections;

public class Test : MonoBehaviour {

	void Start () {
		StartCoroutine (FindPath (Vector3.zero, new Vector3(0,0,20), 1f));
	}

	List<PathNode> GetNeighbors(PathNode current, float step){
		List<PathNode> neighbors = new List<PathNode> ();

		for (int i = 0; i < PathNode.neighbors.Length; i++) {
			var node = GetNeighborNode (current.position, PathNode.neighbors[i], step);
			if (node != null) {
				node.moveCost = (PathNode.neighbors[i] * step).magnitude ;
				neighbors.Add (node);
			}
		}

		return neighbors;
	}

	PathNode GetNeighborNode(Vector3 current, Vector3 next, float step){
		var nextPos = current + next * step;

		if (Physics.Raycast(current, next, (next * step).magnitude)) {
			return null;
		}

		if (!IsNeighborValid(nextPos, ConstantData.CLOSE_COMBAT_RANGE)) {
			return null;
		}

		return new PathNode (nextPos);
	}

	bool IsNeighborValid(Vector3 pos, float distance){
		for (int i = 0; i < PathNode.neighbors.Length; i++) {
			if (Physics.Raycast(pos, PathNode.neighbors[i], distance)) {
				return false;
			}
		}
		return true;
	}

	bool CanGoStraight(Vector3 pos, Vector3 des){
		Debug.DrawLine(pos, des, Color.green, 0.5f);
		if (Physics.Raycast (pos, des - pos, Vector3.Distance (pos, des))) {
			return false;
		} else {
			return true;
		}
	}

	SimplePriorityQueue<PathNode> frontier = new SimplePriorityQueue<PathNode> ();
	PathNodeSet exploredNodes = new PathNodeSet ();
	List<PathNode> neighbors;
	IEnumerator FindPath(Vector3 startPos, Vector3 goalPos, float step){
		var start = new PathNode (startPos, 0);
		frontier.Clear ();
		frontier.Enqueue (start, 0);

		exploredNodes.Clear ();
		exploredNodes.Add (start);

		PathNode current;
		while (frontier.Count > 0) {
			current = frontier.Dequeue ();

			neighbors = GetNeighbors (current, step);
			for (int i = 0; i < neighbors.Count; i++) {
				var next = neighbors [i];
				var newCost = current.moveCost + next.moveCost;
				if (exploredNodes.NotContainsOrHasHigherCost(next, newCost)) {
					Debug.DrawLine (current.position, next.position, Color.red, 1f);
					next.moveCost = newCost;
					next.cameFrom = current;
					exploredNodes.AddOrUpdate (next);

					if (IsReachedGoal(goalPos, next.position)) { //Reached goal
						var goal = new PathNode (goalPos);
						goal.cameFrom = next;
						DebugDrawPath (ReconstructPath(goal, ref start), startPos);
						yield break;
					}

					var priority = newCost + GetHScore(next.position, goalPos) * 1.5f;
					frontier.Enqueue (next, priority); //smaller priority go first
					yield return null;
				}
			}
		}
	}

	Queue<Vector3> ReconstructPath(PathNode goal, ref PathNode start){
		PathNode current = goal;
		Queue<Vector3> path = new Queue<Vector3> ();
		path.Enqueue (current.position);

		while (current != start) {
			current = current.cameFrom;
			path.Enqueue (current.position);

			if (current == null) {
				return null;
			}
		}

		path = new Queue<Vector3> (path.Reverse ());

		return path;
	}

	bool IsReachedGoal(Vector3 goal, Vector3 current){
		if (Vector3.Distance (current, goal) <= ConstantData.CLOSE_COMBAT_RANGE ) {
			return true;
		} else {
			return false;
		}
	}

	float GetHScore(Vector3 a, Vector3 b){
		return Vector3.Distance(a, b);
	}

	void DebugDrawPath(Queue<Vector3> path, Vector3 start){
		int i = 0;
		while (path.Count > 0) {
			var node = path.Dequeue ();
			DebugDrawNode (node, i.ToString());
			Debug.DrawLine (start, node, Color.blue, Mathf.Infinity);
			start = node;
			i++;
		}
	}

	GameObject DebugDrawNode(Vector3 node, string name){
		var go = GameObject.CreatePrimitive (PrimitiveType.Cube);
		go.transform.position = node;
		go.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
		go.name = "nodePath " + name;
		return go;
	}
}


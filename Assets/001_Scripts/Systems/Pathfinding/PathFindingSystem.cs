using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

public class PathFindingSystem : IReactiveSystem, IEnsureComponents{
	#region IEnsureComponents implementation

	public IMatcher ensureComponents {
		get {
			return Matcher.AllOf (Matcher.Ally, Matcher.Active);
		}
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		Queue<Vector3> path;
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];

			if (CanGoStraight (e.position.value, e.destination.value)) {
				path = new Queue<Vector3> ();
				path.Enqueue (e.destination.value);
			} else {
				path = FindPath (e.position.value, e.destination.value, 1f);
				if (GameManager.ShowDebug) {
					DebugDrawPath (path, e.position.value);
				}
			}

			e.ReplacePathQueue (path);
			e.ReplaceMoveTo (path.Dequeue());
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.Destination.OnEntityAdded ();
		}
	}
	#endregion

	bool CanGoStraight(Vector3 pos, Vector3 des){
		Debug.DrawLine(pos, des, Color.green, Mathf.Infinity);
		if (Physics.Raycast (pos, des - pos, Vector3.Distance (pos, des))) {
			return false;
		} else {
			return true;
		}
	}

	#region Pathfinding
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

//		if (!IsNeighborValid(nextPos, ConstantData.CLOSE_COMBAT_RANGE)) {
//			return null;
//		}

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

	SimplePriorityQueue<PathNode> frontier = new SimplePriorityQueue<PathNode> ();
	PathNodeSet exploredNodes = new PathNodeSet ();
	List<PathNode> neighbors;
	Queue<Vector3> FindPath(Vector3 startPos, Vector3 goalPos, float step){
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
					next.moveCost = newCost;
					next.cameFrom = current;
					exploredNodes.AddOrUpdate (next);

					if (IsReachedGoal(goalPos, next)) { //Reached goal
						var goal = new PathNode (goalPos);
						goal.cameFrom = next;
						return ReconstructPath (goal, ref start);

					}
						
					var priority = newCost + GetHScore(next.position, goalPos) * 1.5f;
					frontier.Enqueue (next, priority); //smaller priority go first
				}
			}
		}

		return null;
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

	void DebugDrawPath(Queue<Vector3> path, Vector3 start){
		int i = 0;
		var tempPath = new Queue<Vector3> (path);
		while (tempPath.Count > 0) {
			var node = tempPath.Dequeue ();
			if (GameManager.ShowDebug) {
				Debug.DrawLine (start, node, Color.blue, Mathf.Infinity);
			}
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
	#endregion
}

using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;

public class PathFindingSystem : IReactiveSystem, ISetPool, IEnsureComponents{
	#region ISetPool implementation
	Group _groupPathFinding;
	public void SetPool (Pool pool)
	{
		_groupPathFinding = pool.GetGroup (Matcher.AllOf(Matcher.Ally, Matcher.Active));
	}

	#endregion

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
//		if (_groupPathFinding.count <= 0) {
//			return;
//		}

//		var ens = _groupPathFinding.GetEntities ();
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
			DebugDrawPath (FindPath (e.position.value, e.destination.value, 1f), e.position.value);
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

	List<PathNode> GetNeighbors(PathNode current, float step){
		List<PathNode> neighbors = new List<PathNode> ();

		for (int i = 0; i < PathNode.neighbors.Length; i++) {
			var node = GetNeighborNode (current.position, PathNode.neighbors[i] * step);
			if (node != null) {
				node.moveCost = (PathNode.neighbors[i] * step).magnitude ;
				neighbors.Add (node);
			}
		}

		return neighbors;
	}

	PathNode GetNeighborNode(Vector3 current, Vector3 next){
		var nextPos = current + next;

		if (Physics.Raycast(current, next, Vector3.Distance(current, nextPos))) {
			return null;
		}

		return new PathNode (nextPos);
	}

	SimplePriorityQueue<PathNode> frontier = new SimplePriorityQueue<PathNode> ();
	PathNodeList exploredNodes = new PathNodeList ();
	List<PathNode> neighbors;
	Queue<PathNode> FindPath(Vector3 startPos, Vector3 goalPos, float step){
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

					var priority = newCost + GetHScore(next.position, goalPos);
					frontier.Enqueue (next, priority); //smaller priority go first
				}
			}
		}

		return null;
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

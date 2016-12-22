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
			DebugDrawPath (FindPath (e.position.value, e.destination.value), e.position.value);
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

	List<PathNode> GetNeighbors(PathNode current){
		List<PathNode> neighbors = new List<PathNode> ();
		
		neighbors.Add (new PathNode (current.position + Neighbors.TopLeft,  2));
		neighbors.Add (new PathNode (current.position + Neighbors.TopMid,   1));
		neighbors.Add (new PathNode (current.position + Neighbors.TopRight, 2));
		neighbors.Add (new PathNode (current.position + Neighbors.MidLeft,  1));
		neighbors.Add (new PathNode (current.position + Neighbors.MidRight, 1));
		neighbors.Add (new PathNode (current.position + Neighbors.BotLeft,  2));
		neighbors.Add (new PathNode (current.position + Neighbors.BotMid,   1));
		neighbors.Add (new PathNode (current.position + Neighbors.BotRight, 2));

		return neighbors;
	}

	Queue<PathNode> FindPath(Vector3 startPos, Vector3 goalPos){
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
					Debug.DrawLine (current.position, next.position, Color.red, Mathf.Infinity);
					costSoFar[next] = newCost;
					cameFrom[next] = current;

					if (IsReachedGoal(goalPos, next)) { //Reached goal
						var goal = new PathNode (goalPos);
						cameFrom.Add (goal, next);
						return ReconstructPath (goal, ref cameFrom, ref start);
					}

					var priority = newCost;
					frontier.Enqueue (next, priority); //smaller priority go first
				}
			}
		}

		return null;
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

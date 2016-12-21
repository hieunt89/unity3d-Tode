using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;
using System.Linq;

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

			DrawDebug (FindPath (e.position.value, e.destination.value), e.position.value);
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
		
		neighbors.Add (new PathNode (current.position + new Vector3( 1, 0,  0)));
		neighbors.Add (new PathNode (current.position + new Vector3(-1, 0,  0)));
		neighbors.Add (new PathNode (current.position + new Vector3( 0, 0,  1)));
		neighbors.Add (new PathNode (current.position + new Vector3( 0, 0, -1)));
		neighbors.Add (new PathNode (current.position + new Vector3( 1, 0,  1)));
		neighbors.Add (new PathNode (current.position + new Vector3( 1, 0, -1)));
		neighbors.Add (new PathNode (current.position + new Vector3(-1, 0,  1)));
		neighbors.Add (new PathNode (current.position + new Vector3(-1, 0, -1)));

		return neighbors;
	}

	Queue<PathNode> FindPath(Vector3 startPos, Vector3 goalPos){
		var start = new PathNode (startPos);
		Queue<PathNode> frontier = new Queue<PathNode> ();
		frontier.Enqueue (start);

		Dictionary<PathNode, PathNode> cameFrom = new Dictionary<PathNode, PathNode> ();
		cameFrom.Add (start, null);

		PathNode current;
		List<PathNode> neighbors;
		while (frontier.Count > 0) {
			current = frontier.Dequeue ();

			neighbors = GetNeighbors (current);
			for (int i = 0; i < neighbors.Count; i++) {
				var next = neighbors [i];

				if (!cameFrom.ContainsKey(next)) {
					cameFrom.Add (next, current);

					if (IsReachedGoal(goalPos, next)) {
						var goal = new PathNode (goalPos);
						cameFrom.Add (goal, next);
						return ReconstructPath (goal, ref cameFrom, ref start);
					}

					frontier.Enqueue (next);
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

	void DrawDebug(Queue<PathNode> path, Vector3 start){
		int i = 0;
		while (path.Count > 0) {
			var node = path.Dequeue ();
			var go = GameObject.CreatePrimitive (PrimitiveType.Cube);
			go.transform.position = node.position;
			go.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			go.name = "cube " + i;
			Debug.DrawLine (start, node.position, Color.blue, Mathf.Infinity);
			start = node.position;
			i++;
		}
	}
}

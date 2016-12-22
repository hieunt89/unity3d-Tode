using UnityEngine;
using System.Collections.Generic;
using System;
using Entitas;
using UnityEditor;
using Priority_Queue;

public class Test : MonoBehaviour {
	void Start () {
		SimplePriorityQueue<PathNode> q = new SimplePriorityQueue<PathNode> ();

		q.Enqueue (new PathNode (Vector3.up), 1);
		q.Enqueue (new PathNode (Vector3.down), 0);

		Debug.Log (q.Dequeue().position);
	}
}


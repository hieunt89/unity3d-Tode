using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class PathQueue : IComponent {
	public Queue<Vector3> queue;
}

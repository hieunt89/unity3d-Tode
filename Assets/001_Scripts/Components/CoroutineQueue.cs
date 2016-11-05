using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class CoroutineQueue : IComponent {
	public Queue<IEnumerator> Queue;
}

using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileThrowingDestination : IComponent {
	public Vector3 destination;
	public float initAngle;
	public float initHeight;
}

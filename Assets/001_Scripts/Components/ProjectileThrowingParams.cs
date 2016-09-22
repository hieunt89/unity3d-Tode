using UnityEngine;
using System.Collections;
using Entitas;

public class ProjectileThrowingParams : IComponent {
	public Vector3 start;
	public Vector3 end;
	public float height;
	public float initVelocity;
}

using UnityEngine;
using System.Collections;
using Entitas;

public class Attack : IComponent {
	public int minDamage;
	public int maxDamage;

	public float attackSpeed;
}

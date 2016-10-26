using Entitas;
using UnityEngine;
using System.Collections.Generic;


public enum ProjectileType{
	homing,
	throwing,
	laser
}

public class Projectile : IComponent {
	public string projectileId;
}
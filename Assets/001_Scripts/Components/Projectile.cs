using Entitas;
public enum ProjectileType{
	homing,
	throwing,
	laser
}

public class Projectile : IComponent {
	public string projectileId;
}

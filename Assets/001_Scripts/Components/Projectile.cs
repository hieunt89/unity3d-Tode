using Entitas;
public enum ProjectileType{
	homing,
	throwing
}
public class Projectile : IComponent {
	public string projectileId;
}

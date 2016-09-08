using Entitas;

public enum ProjectileType{
	type1,
	type2
}

public class Projectile : IComponent {
	public ProjectileType type;
}

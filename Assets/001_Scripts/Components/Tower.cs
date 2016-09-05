using Entitas;

public enum TowerType{
	none,
	type1,
	type2
}
public class Tower : IComponent {
	public TowerType type;
}

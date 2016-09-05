using Entitas;

public enum EnemyType{
	type1, 
	type2	
}

public class Enemy : IComponent {
	public EnemyType type;
}

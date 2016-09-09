using Entitas;

public enum EnemyType{
	type1, 
	type2
}

public enum EnemyClass{
	Cho,
	Meo
}

public class Enemy : IComponent {
	public EnemyClass eClass;
	public EnemyType eType;
}

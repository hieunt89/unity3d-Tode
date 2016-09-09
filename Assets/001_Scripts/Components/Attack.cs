using Entitas;

public enum AttackType{
	type1,
	type2
}

public class Attack : IComponent {
	public AttackType attackType;
}

using Entitas;

public enum AttackType{
	physical,
	magical, 
	trueDmg
}

public class Attack : IComponent {
	public AttackType attackType;
}

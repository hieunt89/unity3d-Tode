using Entitas;

public enum ArmorType{
	type1,
	type2
}

public class Armor : IComponent {
	public ArmorType armorType;
}

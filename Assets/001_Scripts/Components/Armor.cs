using Entitas;
using System.Collections.Generic;

public enum ArmorRating{
	none, low, medium, high, immume
}

public class ArmorData {
	public AttackType type;
	public ArmorRating rating;

	public ArmorData (AttackType type, ArmorRating rating)
	{
		this.type = type;
		this.rating = rating;
	}
}

public class Armor : IComponent {
	public List<ArmorData> armorList;
}

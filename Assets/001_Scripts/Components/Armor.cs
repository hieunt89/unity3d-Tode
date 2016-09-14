using Entitas;
using System.Collections.Generic;

public enum ArmorRating{
	none = 0, 
	low = 20, 
	medium = 40, 
	high = 70, 
	immume = 100
}

public class ArmorData {
	public AttackType type;
	public ArmorRating rating;

	public ArmorData (){
		
	}
	public ArmorData (AttackType type, ArmorRating rating)
	{
		this.type = type;
		this.rating = rating;
	}
}

public class Armor : IComponent {
	public List<ArmorData> armorList;
}

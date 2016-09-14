using System.Collections.Generic;

[System.Serializable]
public class EnemyData {
	public string id;
	public string name;
	public float moveSpeed;
	public float turnSpeed;
	public int lifeCount;
	public int goldWorth;
	public AttackType atkType;
	public float atkSpeed;
	public int minAtkDmg;
	public int maxAtkDmg;
	public float atkRange;
	public List<ArmorData> armors;
	public int hp;

	public EnemyData (List<ArmorData> armors) {
		this.armors = armors;
	}

	public EnemyData (string id, string name, float moveSpeed, float turnSpeed, int lifeCount, int goldWorth, AttackType atkType, float atkSpeed, int minAtkDmg, int maxAtkDmg, float atkRange, List<ArmorData> armors, int hp)
	{
		this.id = id;
		this.name = name;
		this.moveSpeed = moveSpeed;
		this.turnSpeed = turnSpeed;
		this.lifeCount = lifeCount;
		this.goldWorth = goldWorth;
		this.atkType = atkType;
		this.atkSpeed = atkSpeed;
		this.minAtkDmg = minAtkDmg;
		this.maxAtkDmg = maxAtkDmg;
		this.atkRange = atkRange;
		this.armors = armors;
		this.hp = hp;
	}
}

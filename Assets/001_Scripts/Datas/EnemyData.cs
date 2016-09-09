public class EnemyData {
	public float moveSpeed;
	public int lifeCount;
	public AttackType atkType;
	public float atkSpeed;
	public int minAtkDmg;
	public int maxAtkDmg;
	public float atkRange;
	public ArmorType armorType;
	public int armorRating;
	public int hp;

	public EnemyData (float moveSpeed, int lifeCount, AttackType atkType, float atkSpeed, int minAtkDmg, int maxAtkDmg, float atkRange, ArmorType armorType, int armorRating, int hp)
	{
		this.moveSpeed = moveSpeed;
		this.lifeCount = lifeCount;
		this.atkType = atkType;
		this.atkSpeed = atkSpeed;
		this.minAtkDmg = minAtkDmg;
		this.maxAtkDmg = maxAtkDmg;
		this.atkRange = atkRange;
		this.armorType = armorType;
		this.armorRating = armorRating;
		this.hp = hp;
	}
}

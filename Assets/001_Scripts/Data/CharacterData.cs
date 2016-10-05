using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CharacterData {
	[SerializeField] public string id;

	[SerializeField] private string name;
	[SerializeField] private float moveSpeed;
	[SerializeField] private float turnSpeed;
	[SerializeField] private int lifeCount;
	[SerializeField] private int goldWorth;
	[SerializeField] private AttackType atkType;
	[SerializeField] private float atkSpeed;
	[SerializeField] private int minAtkDmg;
	[SerializeField] private int maxAtkDmg;
	[SerializeField] private float atkRange;
	[SerializeField] private List<ArmorData> armors;
	[SerializeField] private int hp;

	public string Id {
		get {
			return this.id;
		}
		set {
			id = value;
		}
	}

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public float MoveSpeed {
		get {
			return this.moveSpeed;
		}
		set {
			moveSpeed = value;
		}
	}

	public float TurnSpeed {
		get {
			return this.turnSpeed;
		}
		set {
			turnSpeed = value;
		}
	}

	public int LifeCount {
		get {
			return this.lifeCount;
		}
		set {
			lifeCount = value;
		}
	}

	public int GoldWorth {
		get {
			return this.goldWorth;
		}
		set {
			goldWorth = value;
		}
	}

	public AttackType AtkType {
		get {
			return this.atkType;
		}
		set {
			atkType = value;
		}
	}

	public float AtkSpeed {
		get {
			return this.atkSpeed;
		}
		set {
			atkSpeed = value;
		}
	}

	public int MinAtkDmg {
		get {
			return this.minAtkDmg;
		}
		set {
			minAtkDmg = value;
		}
	}

	public int MaxAtkDmg {
		get {
			return this.maxAtkDmg;
		}
		set {
			maxAtkDmg = value;
		}
	}

	public float AtkRange {
		get {
			return this.atkRange;
		}
		set {
			atkRange = value;
		}
	}

	public List<ArmorData> Armors {
		get {
			return this.armors;
		}
		set {
			armors = value;
		}
	}

	public int Hp {
		get {
			return this.hp;
		}
		set {
			hp = value;
		}
	}

	public CharacterData (string id, List<ArmorData> armors) {
		this.id = id;
		this.armors = armors;
	}

	public CharacterData (string id, string name, float moveSpeed, float turnSpeed, int lifeCount, int goldWorth, AttackType atkType, float atkSpeed, int minAtkDmg, int maxAtkDmg, float atkRange, List<ArmorData> armors, int hp)
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

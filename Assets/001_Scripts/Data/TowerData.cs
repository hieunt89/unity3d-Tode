using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TowerData {
	[SerializeField] public string id;
	[SerializeField] private string name;
	[SerializeField] private int prjTypeIndex;
	[SerializeField] private string prjType;
	[SerializeField] private AttackType atkType;
	[SerializeField] private float atkRange;
	[SerializeField] private int minDmg;
	[SerializeField] private int maxDmg;
	[SerializeField] private float atkSpeed;
	[SerializeField] private int goldRequired;
	[SerializeField] private float buildTime;
	[SerializeField] private float aoe;

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

	public int PrjTypeIndex {
		get {
			return this.prjTypeIndex;
		}
		set {
			prjTypeIndex = value;
		}
	}

	public string PrjType {
		get {
			return this.prjType;
		}
		set {
			prjType = value;
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

	public float AtkRange {
		get {
			return this.atkRange;
		}
		set {
			atkRange = value;
		}
	}

	public int MinDmg {
		get {
			return this.minDmg;
		}
		set {
			minDmg = value;
		}
	}

	public int MaxDmg {
		get {
			return this.maxDmg;
		}
		set {
			maxDmg = value;
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

	public int GoldRequired {
		get {
			return this.goldRequired;
		}
		set {
			goldRequired = value;
		}
	}

	public float BuildTime {
		get {
			return this.buildTime;
		}
		set {
			buildTime = value;
		}
	}

	public float Aoe {
		get {
			return aoe;
		}
		set{ 
			aoe = value;
		}
	}

	public TowerData (string id) {
		this.id = id;
	}

	public TowerData (string id, List<int> nextUpgradeIndexes, List<string> nextUpgrades){
		this.id = id;
	}
	
	public TowerData (string id, string name, string prjType, AttackType atkType, float atkRange, int minDmg, int maxDmg, float atkSpeed, int goldRequired, float buildTime)
	{
		this.id = id;
		this.name = name;
		this.prjType = prjType;
		this.atkType = atkType;
		this.atkRange = atkRange;
		this.minDmg = minDmg;
		this.maxDmg = maxDmg;
		this.atkSpeed = atkSpeed;
		this.goldRequired = goldRequired;
		this.buildTime = buildTime;
	}
}

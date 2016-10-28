using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TowerData{
	[SerializeField] public string id;
	[SerializeField] private string name;
	[SerializeField] private int projectileIndex;
	[SerializeField] private string projectileId;
	[SerializeField] private AttackType atkType;
	[SerializeField] private float atkRange;
	[SerializeField] private int minDmg;
	[SerializeField] private int maxDmg;
	[SerializeField] private float atkSpeed;
	[SerializeField] private float atkTime;
	[SerializeField] private int goldRequired;
	[SerializeField] private float buildTime;
	[SerializeField] private float aoe;
	private List<string> treeSkillNames;

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

	public int ProjectileIndex {
		get {
			return this.projectileIndex;
		}
		set {
			projectileIndex = value;
		}
	}

	public string ProjectileId {
		get {
			return this.projectileId;
		}
		set {
			projectileId = value;
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

	public float AtkTime {
		get {
			return atkTime;
		}set{ 
			atkTime = value;
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

	public List<string> TreeSkillNames {
		get {
			return treeSkillNames;
		}set{ 
			treeSkillNames = value;
		}
	}

	public TowerData (string id) {
		this.id = id;
	}
}

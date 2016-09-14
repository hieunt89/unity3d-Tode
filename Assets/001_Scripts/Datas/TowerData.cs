using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerData
{
	[SerializeField] public string id;
	[SerializeField] public string name;
	[SerializeField] public string prjType;
	[SerializeField] public AttackType atkType;
	[SerializeField] public float atkRange;
	[SerializeField] public int minDmg;
	[SerializeField] public int maxDmg;
	[SerializeField] public float atkSpeed;
	[SerializeField] public int goldRequired;
	[SerializeField] public List<string> nextUpgrade;
	[SerializeField] public float buildTime;

	public TowerData (string id, string name, string prjType, AttackType atkType, float atkRange, int minDmg, int maxDmg, float atkSpeed, int goldWorth, List<string> nextUpgrade, float buildTime)
	{
		this.id = id;
		this.name = name;
		this.prjType = prjType;
		this.atkType = atkType;
		this.atkRange = atkRange;
		this.minDmg = minDmg;
		this.maxDmg = maxDmg;
		this.atkSpeed = atkSpeed;
		this.goldRequired = goldWorth;
		this.nextUpgrade = nextUpgrade;
		this.buildTime = buildTime;
	}
}

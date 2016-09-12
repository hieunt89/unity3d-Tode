using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerData
{
	[SerializeField] public string prjType;
	[SerializeField] public AttackType atkType;
	[SerializeField] public float atkRange;
	[SerializeField] public int minDmg;
	[SerializeField] public int maxDmg;
	[SerializeField] public float atkSpeed;
	[SerializeField] public int goldWorth;
	[SerializeField] public List<string> nextUpgrade;

	public TowerData (string prjType, AttackType atkType, float atkRange, int minDmg, int maxDmg, float atkSpeed, int goldWorth, List<string> nextUpgrade)
	{
		this.prjType = prjType;
		this.atkType = atkType;
		this.atkRange = atkRange;
		this.minDmg = minDmg;
		this.maxDmg = maxDmg;
		this.atkSpeed = atkSpeed;
		this.goldWorth = goldWorth;
		this.nextUpgrade = nextUpgrade;
	}
	
}

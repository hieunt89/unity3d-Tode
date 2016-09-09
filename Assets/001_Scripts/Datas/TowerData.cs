using UnityEngine;
using System.Collections;

public class TowerData
{
	[SerializeField] public string prjType;
	[SerializeField] public AttackType atkType;
	[SerializeField] public float atkRange;
	[SerializeField] public int minDmg;
	[SerializeField] public int maxDmg;
	[SerializeField] public float atkSpeed;
	
	public TowerData (string prjType, AttackType atkType, float atkRange, int minDmg, int maxDmg, float atkSpeed)
	{
		this.prjType = prjType;
		this.atkType = atkType;
		this.atkRange = atkRange;
		this.minDmg = minDmg;
		this.maxDmg = maxDmg;
		this.atkSpeed = atkSpeed;
	}
}

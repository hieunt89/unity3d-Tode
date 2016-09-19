using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ArmorData {
	[SerializeField] private AttackType type;
	[SerializeField] private float reduction;

	public AttackType Type {
		get {
			return this.type;
		}
		set {
			type = value;
		}
	}

	public float Reduction {
		get {
			return this.reduction;
		}
		set {
			reduction = value;
		}
	}

	public ArmorData (){

	}
	public ArmorData (AttackType type, float reduction)
	{
		this.type = type;
		this.reduction = reduction;
	}
}


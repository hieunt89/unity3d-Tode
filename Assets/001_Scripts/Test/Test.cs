using UnityEngine;
using System.Collections.Generic;
using System;

public class Test : MonoBehaviour {

	void Start () {
//		DataManager dm = new DataManager ();
		SkillData s = DataManager.Instance.GetSkillData ("fireball1");
		CombatSkillData cb = null;
		if(s is CombatSkillData){
			cb = s as CombatSkillData;
		}
		Debug.Log (cb.projectileId);
	}
}

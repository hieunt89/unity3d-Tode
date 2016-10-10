using UnityEngine;
using System.Collections.Generic;
using System;

public class Test : MonoBehaviour {

	void Start () {
		DataManager dm = new DataManager ();
		Skill s = DataManager.Instance.GetSkillData ("fireball1");
		CombatSkill cb = null;
		if(s is CombatSkill){
			cb = s as CombatSkill;
		}
		Debug.Log (cb.prjId);
	}
}

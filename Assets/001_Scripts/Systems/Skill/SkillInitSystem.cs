using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class SkillInitSystem : IReactiveSystem, ISetPool {
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IReactiveExecuteSystem implementation
	public void Execute (System.Collections.Generic.List<Entity> entities)
	{
		for (int i = 0; i < entities.Count; i++) {
			var e = entities [i];
				
			var eList = new List<Entity> ();
			for (int j = 0; j < e.skillList.skillTrees.Count; j++) {
				eList.Add (CreateSkill(e.skillList.skillTrees[j].Root, e));
			}
			e.AddSkillEntityList (eList);
			e.RemoveSkillList ();
		}
	}
	#endregion

	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.SkillList.OnEntityAdded ();
		}
	}
	#endregion

	Entity CreateSkill(Node<string> skillNode, Entity origin){
		var data = DataManager.Instance.GetSkillData (skillNode.data);
		if (data == null) {
			return null;
		}

		var e = _pool.CreateEntity ()
			.AddSkill (skillNode)
			.AddOrigin (origin)
			.AddAttackSpeed(data.cooldown)
			.AddAttackRange(data.castRange)
			.AddAttackTime(data.castTime)
			.AddGold(data.cost)
			.IsActive(true)
			;

		if(data is CombatSkillData){
			CombatSkillData s = data as CombatSkillData;
			e.AddSkillCombat (s.effectList)
				.AddAttack(s.attackType)
				.AddAttackDamage(s.damage)
				.AddAoe (s.aoe)
				.AddProjectile (s.projectileId);
		}else if(data is SummonSkillData){
			SummonSkillData s = data as SummonSkillData;
			e.AddSkillSummon (s.summonId, s.summonCount)
				.AddDuration (s.duration);
		}

		return e;
	}
}

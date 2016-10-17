using UnityEngine;
using System.Collections;
using Entitas;
using System.Collections.Generic;

public class Skill_InitSystem : IReactiveSystem, ISetPool {
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

			if (e.hasSkillEntityList) {
				for (int j = 0; j < e.skillEntityList.skillEntities.Count; j++) {
					e.skillEntityList.skillEntities [j].IsActive(false).IsMarkedForDestroy (true);
				}
				e.RemoveSkillEntityList ();
			}
				
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

		if(data is CombatSkill){
			CombatSkill s = data as CombatSkill;
			e.AddSkillCombat (s.effectList)
				.AddAttack(s.damageType)
				.AddAttackDamage(s.damage)
				.AddAoe (s.aoe)
				.AddProjectile (s.prjId);
		}else if(data is SummonSkill){
			SummonSkill s = data as SummonSkill;
			e.AddSkillSummon (s.summonId, s.summonCount)
				.AddDuration (s.duration);
		}

		return e;
	}
}

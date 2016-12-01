using UnityEngine;
using System.Collections;
using Entitas;

public class SkillUpgradeSystem : IReactiveSystem, ISetPool {
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
			var skill = entities [i];

			var skillData = DataManager.Instance.GetSkillData (skill.skillUpgrade.upgradeNode.data);

			if (skillData != null) {
				var cost = skillData.goldCost;
				if (_pool.goldPlayer.value >= cost) {
					if (!skill.isActive) {
						skill.IsActive (true);
					} else {
						skill.ReplaceSkill (skill.skillUpgrade.upgradeNode)
							.ReplaceSkillStats (skillData);
					}
					_pool.ReplaceGoldPlayer (_pool.goldPlayer.value - cost);
					skill.origin.e.ReplaceGold (skill.origin.e.gold.value + cost);
				}
			}

			skill.RemoveSkillUpgrade ();

			_pool.ReselectEntity (skill.origin.e);
		}
	}
	#endregion
	#region IReactiveSystem implementation
	public TriggerOnEvent trigger {
		get {
			return Matcher.SkillUpgrade.OnEntityAdded ();
		}
	}
	#endregion
}

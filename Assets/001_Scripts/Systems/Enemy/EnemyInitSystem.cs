using System.Collections.Generic;
using Entitas;
using UnityEngine;

public class EnemyInitSystem : IReactiveSystem, ISetPool
{
	Pool _pool;

	public void SetPool(Pool pool)
	{
		_pool = pool;
	}

    public TriggerOnEvent trigger
    {
        get
        {
			return Matcher.AllOf(Matcher.Wave, Matcher.Active).OnEntityAdded ();
        }
    }

	void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
		for (int waveIndex = 0; waveIndex < entities.Count; waveIndex++) { // loop throu all active waves
			var e = entities [waveIndex];
			float activeTime = _pool.tick.time;
			WaveGroupData waveGroup;
			Entity ePath;
			CharacterData enemyData;
			for (int groupIndex = 0; groupIndex < e.wave.groups.Count; groupIndex++) { //loop throu all wave group datas in wave
				waveGroup = e.wave.groups[groupIndex];
				activeTime = activeTime + waveGroup.GroupDelay;
				ePath = _pool.GetEntityById(waveGroup.PathId);
				if (ePath == null) { //continue if path not found
					if (GameManager.debug) {
						Debug.Log ("Path with id " + waveGroup.PathId + " is null");
					}
					continue;
				}

					for (int enemyIndex = 0; enemyIndex < waveGroup.Amount; enemyIndex++) { //loop throu all enemies in wave group data
					enemyData = DataManager.Instance.GetCharacterData (waveGroup.EnemyId);
					if(enemyData == null){ //break if enemy data is null
						if (GameManager.debug) {
							Debug.Log ("Enemy with id " + waveGroup.EnemyId + " is null");
						}
						break;
					}

					if (enemyIndex != 0) { //do not add spawn interval on the first enemy in group
						activeTime = activeTime + waveGroup.SpawnInterval;
					}

					_pool.CreateEntity ()
						.AddEnemy (waveGroup.EnemyId)
						.AddId (e.id.value + "_group" + groupIndex + "_enemy" + enemyIndex)
						.AddPathReference (ePath)
						.AddMarkedForActive (activeTime)
						.AddDestination (ePath.path.wayPoints [0])
						.AddPosition (ePath.path.wayPoints [0])
						.AddMovable (enemyData.MoveSpeed)
						.AddTurnSpeed(enemyData.TurnSpeed)
						.AddLifeCount (enemyData.LifeCount)
						.AddGold (enemyData.GoldWorth)
						.AddAttack (enemyData.AtkType)
						.AddAttackSpeed(enemyData.AtkSpeed)
						.AddAttackDamage(enemyData.MinAtkDmg, enemyData.MaxAtkDmg)
						.AddAttackRange(enemyData.AtkRange)
						.AddArmor(enemyData.Armors)
						.AddHp (enemyData.Hp)
						.AddHpTotal(enemyData.Hp)
						;
				}
			}

			_pool.DestroyEntity (e);
		}
    }

}

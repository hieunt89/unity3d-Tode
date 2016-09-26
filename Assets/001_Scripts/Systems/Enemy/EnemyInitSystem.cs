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
            return Matcher.Wave.OnEntityAdded ();
        }
    }

	void IReactiveExecuteSystem.Execute(List<Entity> entities)
    {
		var e = entities.SingleEntity ();
		float activeTime = _pool.tick.time;
		WaveGroupData waveGroup;
		Entity ePath;
		EnemyData enemyData;
		for (int i = 0; i < e.wave.groups.Count; i++) { //loop throu all wave group datas in wave
			waveGroup = e.wave.groups[i];
			activeTime = activeTime + waveGroup.WaveDelay;
			ePath = _pool.GetEntityById(waveGroup.PathId);
			if (ePath == null) { //continue if path not found
				if (GameManager.debug) {
					Debug.Log ("Path with id " + waveGroup.PathId + " is null");
				}
				continue;
			}

			for (int j = 0; j < waveGroup.Amount; j++) { //loop throu all enemies in wave group data
				enemyData = DataManager.Instance.GetEnemyData (waveGroup.EnemyId);
				if(enemyData == null){ //break if enemy data is null
					if (GameManager.debug) {
						Debug.Log ("Enemy with id " + waveGroup.EnemyId + " is null");
					}
					break;
				}

				if (j != 0) { //do not add spawn interval on the first enemy in group
					activeTime = activeTime + waveGroup.SpawnInterval;
				}

				_pool.CreateEntity ()
					.AddEnemy (waveGroup.EnemyId)
					.AddId (e.id.value + "_group" + i + "_enemy" + j)
					.AddPathReference (ePath)
					.AddMarkedForActive (activeTime)
					.IsInteractable (true)
					.AddDestination (ePath.path.wayPoints [0])
					.AddPosition (ePath.path.wayPoints [0])
					.AddMovable (enemyData.MoveSpeed)
					.AddTurnable(enemyData.TurnSpeed)
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

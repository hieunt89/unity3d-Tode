using System.Collections.Generic;
using Entitas;

public class SpawnEnemySystem : IReactiveSystem, IExecuteSystem, ISetPool
{
	Pool _pool;

    public TriggerOnEvent trigger
    {
        get
        {
            return Matcher.Wave.OnEntityAdded ();
        }
    }

    public void Execute(List<Entity> entities)
    {
        for (int i = 0; i < entities.Count; i++)	// loop throu all waves
		{
			for (int j = 0; j < entities[i].wave.datas.Count; j++) 	// loop throu all wave datas 
			{
				_pool.CreateEntity().AddEnemy(EnemyType.type1).AddId("e"+j).AddMovable(1.0f);	// create enemy entities
			} 
		}
    }

    public void SetPool(Pool pool)
    {
        _pool = pool;
    }

    void IExecuteSystem.Execute()
    {
        // check tick 
		// if current tick - last tick >= interval in data

    }
}

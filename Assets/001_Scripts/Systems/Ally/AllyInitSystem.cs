using UnityEngine;
using System.Collections;
using Entitas;

public class AllyInitSystem : IInitializeSystem, ISetPool{
	#region ISetPool implementation
	Pool _pool;
	public void SetPool (Pool pool)
	{
		_pool = pool;
	}

	#endregion

	#region IInitializeSystem implementation

	public void Initialize ()
	{
		CreateAlly ("enemy0", new Vector3(9f, 0f, 0f));
	}

	#endregion

	Entity CreateAlly(string charId, Vector3 pos){
		var charData = DataManager.Instance.GetCharacterData (charId);
		var e = _pool.CreateCharacter (charData);
		e.AddAlly (charId)
			.AddId ("ally")
			.AddPosition (pos)
			.IsTargetable (true)
			.IsAttackable (true)
			.IsActive (true);
		return e;
	}
}

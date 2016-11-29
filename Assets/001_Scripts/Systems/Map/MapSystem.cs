using UnityEngine;
using System.Collections;
using Entitas;

public class MapSystem : IInitializeSystem, ISetPool {
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
		var data = DataManager.Instance.GetMapData (UserManager.Instance.userData.GetSelectedMap());
		if(data != null){
			_pool.ReplaceMap (data);
		}
	}
	#endregion
	
}

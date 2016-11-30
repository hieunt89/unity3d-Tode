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
		MapData data;
		var selectedMap = UserManager.Instance.userData.GetSelectedMap ();
		if (selectedMap != null) {
			data = DataManager.Instance.GetMapData (selectedMap);
		} else {
			data = DataManager.Instance.GetMapData (0);
		}
		if(data != null){
			_pool.ReplaceMap (data);
		}
	}
	#endregion
	
}

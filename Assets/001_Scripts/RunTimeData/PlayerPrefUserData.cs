using UnityEngine;
using System.Collections;

public class PlayerPrefUserData : IUserData{
	#region IUserData implementation

	public string GetSelectedMap ()
	{
		if (PlayerPrefs.HasKey (ConstantString.PPSelectedMap)) {
			return PlayerPrefs.GetString (ConstantString.PPSelectedMap);
		} else {
			return null;
		}
	}

	public void SetSelectedMap (string id)
	{
		PlayerPrefs.SetString (ConstantString.PPSelectedMap, id);
	}
	#endregion
}


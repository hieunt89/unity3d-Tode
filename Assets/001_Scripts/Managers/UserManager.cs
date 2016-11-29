using UnityEngine;
using System.Collections;

public class UserManager {
	#region Singleton
	private static UserManager instance = null;
	public static UserManager Instance
	{
		get
		{
			return instance;
		}
	}
	public static void Init(){
		if (instance == null)
		{
			instance = new UserManager();
		}
	}
	#endregion

	public IUserData userData;
	public UserManager(){
		userData = DIContainer.GetModule<IUserData> ();
	}
}

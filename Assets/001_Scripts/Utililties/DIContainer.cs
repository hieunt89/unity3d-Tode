using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class DIContainer {

	static Dictionary<Type, object> registeredModules = new Dictionary<Type, object>();

	public static void BindModules(){
//		DIContainer.SetModule<IDataUtils> (new JsonUtils ());
		DIContainer.SetModule<IDataUtils> (new GameData ());

		DIContainer.SetModule<IPrefabUtils> (new PrefabUtils ());
	}

	public static void SetModule<T>(object module) {
		registeredModules.Add (typeof(T), module);
	}

	public static T GetModule<T>() {
		Type t = typeof(T);
		if (registeredModules.ContainsKey (t)) {
			return (T)registeredModules [t];
		} else {
			return default(T);
		}
	}
}

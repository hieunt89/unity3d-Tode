using System.Collections.Generic;
using UnityEngine;

public interface IInjectDataUtils {
	void SetDataUtils (IDataUtils dataUtils);
}

public interface IDataUtils {
	void CreateData<T> (T data) where T : ScriptableObject;
	T LoadData<T> () where T : class;
	List<T> LoadAllData <T> ();
	void DeleteData (string path);
}
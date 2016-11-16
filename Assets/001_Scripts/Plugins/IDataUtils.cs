using System.Collections.Generic;

public interface IInjectDataUtils {
	void SetDataUtils (IDataUtils dataUtils);
}

public interface IDataUtils {
	void CreateData<T> (T data);
	T LoadData<T> ();
	List<T> LoadAllData <T> ();
	void DeleteData (string path);
}
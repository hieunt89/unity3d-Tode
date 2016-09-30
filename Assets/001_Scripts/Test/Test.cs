using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Data <T> where T : class{
	public ChildData <T> rootData;

	public Data (T data)
	{
		this.rootData = new ChildData<T> (data, null, new List<ChildData<T>>());
	}
	
}

[Serializable]
public class ChildData <T> where T : class {
	public T data;
	public ChildData <T> parentData;
	public List <ChildData<T>> childDatas;

	public ChildData (T childData, ChildData<T> parentData, List<ChildData<T>> childDatas)
	{
		this.data = childData;
		this.parentData = parentData;
		this.childDatas = childDatas;
	}

	public void AddChildData (ChildData<T> _childData) {
		childDatas.Add (_childData);
	}

	public ChildData <T> AddRelationship(ChildData<T> _childData){
		this.childDatas.Add (_childData);
		_childData.parentData = this;
		return this;
	}
}

public class Test : MonoBehaviour {

	void Start () {
		Data<string> data = new Data<string> ("this is root data");

		Debug.Log (data.rootData.data);
		Debug.Log (data.rootData.childDatas.Count);
		Debug.Log (JsonUtility.ToJson (data.rootData));

		ChildData<string> newChildData = new ChildData<string> ("this is new child data", data.rootData, new List<ChildData<string>> ());

		data.rootData.AddChildData (newChildData);

		Debug.Log (JsonUtility.ToJson (data.rootData.childDatas));
	}
}

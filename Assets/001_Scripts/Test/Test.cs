using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class Data <T> where T : class{
	public ChildData rootData;

	public Data (T data)
	{
		this.rootData = new ChildData (data.ToString(), null, new List<ChildData>());
	}
}

[Serializable]
public class ChildData {
	public string data;
	public ChildData parentData;
	public List <ChildData> childDatas;

	public ChildData (string childData, ChildData parentData, List<ChildData> childDatas)
	{
		this.data = childData;
		this.parentData = parentData;
		this.childDatas = childDatas;
	}

	public ChildData AddRelationship(ChildData _childData){
		this.childDatas.Add (_childData);
		_childData.parentData = this;
		return this;
	}
}

public class Test : MonoBehaviour {

	void Start () {
		Data<string> data = new Data<string> ("this is root data");

		Debug.Log (data.rootData.data);
		Debug.Log (JsonUtility.ToJson (data.rootData));


		ChildData newChildData = new ChildData ("this is new child data", null, new List<ChildData> ());
		data.rootData.AddRelationship (newChildData);
		Debug.Log (JsonUtility.ToJson (data.rootData.childDatas.Count));
		Debug.Log (JsonUtility.ToJson (data.rootData));

	}
}

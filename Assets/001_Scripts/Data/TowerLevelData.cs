
public class TowerLevelData {
	public string id;
	private TowerLevelData[] data;

	public string Id {
		get {
			return this.id;
		}
		set {
			id = value;
		}
	}

	public TowerLevelData[] Data {
		get {
			return this.data;
		}
		set {
			data = value;
		}
	}

	public TowerLevelData (string id)
	{
		this.id = id;
	}
	
	public TowerLevelData (string id, TowerLevelData[] data)
	{
		this.id = id;
		this.data = data;
	}
	
}

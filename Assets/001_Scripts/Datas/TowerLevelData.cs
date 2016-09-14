public class TowerLevelData {
	public string id;
	TowerLevelData[] data;

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

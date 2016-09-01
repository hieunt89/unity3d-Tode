using Entitas;

public enum TileType{
	none,
	movable,
	constructable
}

public class Tile : IComponent {
	public TileType tileType;
}

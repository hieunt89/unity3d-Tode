public struct AnimState{
	public static readonly string Fire = "fire";
	public static readonly string PostFire = "postFire";

	public static readonly string Die = "die";
	public static readonly string Move = "move";
	public static readonly string Idle = "idle";

	public static readonly string Cast = "cast";
	public static readonly string PostCast = "postCast";

	public static readonly string Wound = "wound";
}

public struct AnimLayer{
	public static readonly int Base = 0;
	public static readonly int Combat = 1;
}

public struct AnimParam{
	public static readonly float CrossTime = 0.1f;
}
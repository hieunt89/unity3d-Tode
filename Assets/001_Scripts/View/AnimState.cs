public struct AnimState{
	enum StateName {
		fire,
		die,
		move,
		idle
	}

	public static string Fire{
		get{ 
			return StateName.fire.ToString ();
		}
	}

	public static string Die{
		get{ 
			return StateName.die.ToString ();
		}
	}

	public static string Move{
		get{ 
			return StateName.move.ToString ();
		}
	}

	public static string Idle{
		get{ 
			return StateName.idle.ToString ();
		}
	}
}
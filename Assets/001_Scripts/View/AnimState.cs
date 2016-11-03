public struct AnimState{
	enum StateName {
		fire,
		cast,
		die,
		move,
		idle, 
		wound
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

	public static string Cast{
		get{ 
			return StateName.cast.ToString ();
		}
	}

	public static string Wound{
		get{ 
			return StateName.wound.ToString ();
		}
	}
}
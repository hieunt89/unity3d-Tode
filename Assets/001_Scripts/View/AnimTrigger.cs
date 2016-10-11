public struct AnimTrigger{
	enum Trigger {
		fire,
		die,
		move,
		idle
	}

	public static string Fire{
		get{ 
			return Trigger.fire.ToString ();
		}
	}

	public static string Die{
		get{ 
			return Trigger.die.ToString ();
		}
	}

	public static string Move{
		get{ 
			return Trigger.move.ToString ();
		}
	}

	public static string Idle{
		get{ 
			return Trigger.idle.ToString ();
		}
	}
}
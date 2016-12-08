public class Events {
	public enum Input{
		EMPTY_SELECT,
		TOWER_SELECT,
		RESELECT,
		CLICK_HIT_POS,
		CLICK_HIT_SOMETHING,
		CLICK_HIT_NOTHING,
		PAN_CAM_X,
		PAN_CAM_Y,
		ROTATE_CAM,
		ZOOM_CAM
	}

	public enum Game{
		GOLD_CHANGE,
		LIFE_CHANGE,
		TIME_CHANGE
	}

	public enum Loading{
		DONE_INIT
	}
}

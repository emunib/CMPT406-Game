public class XBoxOneControllerInfo : InputControllerInfo {
	private const string xbox_horizontal_lstick = "Horizontal_X";
	private const string xbox_horizontal_lstick_pc = "Horizontal_X_PC";
	private const string xbox_vertical_lstick = "Vertical_X";
	private const string xbox_vertical_lstick_pc = "Vertical_X_PC";
	private const string xbox_horizontal_rstick = "Horizontal_GX";
	private const string xbox_horizontal_rstick_pc = "Horizontal_GX_PC";
	private const string xbox_vertical_rstick = "Vertical_GX";
	private const string xbox_vertical_rstick_pc = "Vertical_GX_PC";
	private const string xbox_jump = "Jump_X";
	private const string xbox_jump_pc = "Jump_X_PC";
	private const string xbox_left_trigger = "LT_X";
	private const string xbox_leftpc_trigger = "LT_X_PC";
	private const string xbox_right_trigger = "RT_X";
	private const string xbox_rightpc_trigger = "RT_X_PC";
	
	/* uisng preprocessor directives we have diffrentiated the OSX controls from the windows */
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
	public override string controller_type() {
		return "Xbox Controller (Mac)";
	}

	public override string Horizontal_LStick() {
		return xbox_horizontal_lstick;
	}

	public override string Vertical_LStick() {
		return xbox_vertical_lstick;
	}

	public override string Horizontal_RStick() {
		return xbox_horizontal_rstick;
	}

	public override string Vertical_RStick() {
		return xbox_vertical_rstick;
	}
	
	public override string Jump() {
		return xbox_jump;
	}

	public override string LeftTrigger() {
		return xbox_left_trigger;
	}

	public override string RightTrigger() {
		return xbox_right_trigger;
	}
#endif
	
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
	public override string controller_type() {
		return "Xbox (Windows)");
	}

	public override string Horizontal_LStick() {
		return xbox_horizontal_lstick_pc;
	}

	public override string Vertical_LStick() {
		return xbox_vertical_lstick_pc;
	}

	public override string Horizontal_RStick() {
		return xbox_horizontal_rstick_pc;
	}

	public override string Vertical_RStick() {
		return xbox_vertical_rstick_pc;
	}
	
	public override string Jump() {
		return xbox_jump_pc;
	}

	public override string LeftTrigger() {
		return xbox_leftpc_trigger;
	}

	public override string RightTrigger() {
		return xbox_rightpc_trigger;
	}
#endif
}

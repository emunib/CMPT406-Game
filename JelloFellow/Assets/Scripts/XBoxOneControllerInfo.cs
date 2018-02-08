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
		
	public override string controller_type() {
		return "Xbox One" + (isMac() ? " (Mac)" : " (Windows)");
	}

	public override string Horizontal_LStick() {
		return isMac() ? xbox_horizontal_lstick : xbox_horizontal_lstick_pc;
	}

	public override string Vertical_LStick() {
		return isMac() ? xbox_vertical_lstick : xbox_vertical_lstick_pc;
	}

	public override string Horizontal_RStick() {
		return isMac() ? xbox_horizontal_rstick : xbox_horizontal_rstick_pc;
	}

	public override string Vertical_RStick() {
		return isMac() ? xbox_vertical_rstick : xbox_vertical_rstick_pc;
	}
	
	public override string Jump() {
		return isMac() ? xbox_jump : xbox_jump_pc;
	}
}

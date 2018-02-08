public class Ps4ControllerInfo : InputControllerInfo {
  private const string ps4_horizontal_lstick = "Horizontal_P";
  private const string ps4_vertical_lstick = "Vertical_P";
  private const string ps4_horizontal_rstick = "Horizontal_GP";
  private const string ps4_vertical_rstick = "Vertical_GP";
  private const string ps4_jump = "Jump_P";

  public override string controller_type() {
    return "PS4";
  }

  public override string Horizontal_LStick() {
    return ps4_horizontal_lstick;
  }

  public override string Vertical_LStick() {
    return ps4_vertical_lstick;
  }

  public override string Horizontal_RStick() {
    return ps4_horizontal_rstick;
  }

  public override string Vertical_RStick() {
    return ps4_vertical_rstick;
  }

  public override string Jump() {
    return ps4_jump;
  }
}

public class XBoxOneControllerInfo : InputControllerInfo {
	/* uisng preprocessor directives we have diffrentiated the OSX controls from the windows */
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
	private const string appender = "_X";
  
	public override string controller_type() {
		return "Xbox Controller (MAC)";
	}

	public override string Horizontal_LStick() {
		return leftstickx + appender;
	}

	public override string Vertical_LStick() {
		return leftsticky + appender;
	}

	public override string Horizontal_RStick() {
		return rightstickx + appender;
	}

	public override string Vertical_RStick() {
		return rightsticky + appender;
	}

	public override string Button1() {
		return button1 + appender;
	}

	public override string Button2() {
		return button2 + appender;
	}

	public override string Button3() {
		return button3 + appender;
	}

	public override string Button4() {
		return button4 + appender;
	}

	public override string LeftTrigger() {
		return lefttrigger + appender;
	}

	public override string RightTrigger() {
		return righttrigger + appender;
	}

	public override string LeftBumper() {
		return leftbumper + appender;
	}

	public override string RightBumper() {
		return rightbumper + appender;
	}

	public override string Button_LStick() {
		return leftstickclick + appender;
	}

	public override string Button_RStick() {
		return rightstickclick + appender;
	}
	
	public override string StartButton() {
		return startbutton + appender;
	}
#endif
	
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
	private const string appender = "_XPC";
  
  public override string controller_type() {
    return "Xbox Controller (Windows)";
  }

  public override string Horizontal_LStick() {
    return leftstickx + appender;
  }

  public override string Vertical_LStick() {
    return leftsticky + appender;
  }

  public override string Horizontal_RStick() {
    return rightstickx + appender;
  }

  public override string Vertical_RStick() {
    return rightsticky + appender;
  }

  public override string Button1() {
    return button1 + appender;
  }

  public override string Button2() {
    return button2 + appender;
  }

  public override string Button3() {
    return button3 + appender;
  }

  public override string Button4() {
    return button4 + appender;
  }

  public override string LeftTrigger() {
    return lefttrigger + appender;
  }

  public override string RightTrigger() {
    return righttrigger + appender;
  }

  public override string LeftBumper() {
    return leftbumper + appender;
  }

  public override string RightBumper() {
    return rightbumper + appender;
  }

  public override string Button_LStick() {
    return leftstickclick + appender;
  }

  public override string Button_RStick() {
    return rightstickclick + appender;
  }
	
	public override string StartButton() {
    return startbutton + appender;
  }
#endif
}

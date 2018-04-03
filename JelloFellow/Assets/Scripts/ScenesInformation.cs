using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScenesInformation {
	public Dictionary<string, SceneInfo> SceneInfos;

	public ScenesInformation() {
		SceneInfos = new Dictionary<string, SceneInfo>();
	}
}
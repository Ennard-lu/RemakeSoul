using Godot;
using System;

public partial class SceneInit : Node2D
{
	public override void _Ready() {
		if (GameMgr.Instance.initLeave != null) {
			GameMgr.Instance.initLeave(this);
		}
	}
}

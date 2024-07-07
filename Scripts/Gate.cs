using Godot;
using Godot.NativeInterop;
using System;

public partial class Gate : Area2D {
	// Called when the node enters the scene tree for the first time.
	[Export] public Godot.Collections.Array<BurdenType> lack;

	public override void _Ready() {
		Connect("body_entered", new Callable(this, "CheckWin")); 
		GameMgr.Instance.CurrentGate = this;
	}

	public void OnPlayerGetBurden(BurdenBase burden) {
		GetNode<BurdenIcon>(burden.GetName()).Light();
	}

	public void CheckWin(Node2D node) {
		if (node is Player player) {
			if (player.MeetWinConditions()) {
				GD.Print("you win.");

				AudioMgr.Instance.Play("DoorOpen");

				GameMgr.Instance.currentWinCount = 3;
				GameMgr.Instance.ToTheNextLevel();
			}
		}

	}
}

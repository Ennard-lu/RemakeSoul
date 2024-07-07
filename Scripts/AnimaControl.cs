using Godot;
using System;

public partial class AnimaControl : AnimatedSprite2D {
	private Player player;
	private enum State { idle, running };
	private State state = State.idle;
	private int burdenCount = 0;

	public override void _Ready() {
		player = GetParent() as Player;
	}

    public override void _Process(double delta) {
		burdenCount = player.burdens.Count;
		String anima;

        if (player.Velocity.LengthSquared() != 0) {
			anima = "run";
		} else {
			anima = "idle";
		}
		anima += burdenCount.ToString();

		if (!anima.Equals(Animation)) {
			Play(anima);
		}
    }
}

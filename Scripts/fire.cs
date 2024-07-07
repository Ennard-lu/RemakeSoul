using Godot;
using System;

public partial class fire : Area2D {
    private PointLight2D light;
    private AnimatedSprite2D anima;

    public override void _Ready() {
        light = GetNode<PointLight2D>("PointLight2D");
        anima = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        Connect("area_entered", new Callable(this, "OnPlayerEntered"));
        Connect("area_exited", new Callable(this, "OnPlayerExited"));
    }

    public void OnPlayerEntered(Area2D area) {
        if (area.GetParent()!= null && area.GetParent() is Player player) {
            if (player.burdens.Count <= 2) {
                light.Enabled = true;
                anima.Play("default");
                AudioMgr.Instance.CheckPlay("FireFlame");
            }
        }
    }

    public void OnPlayerExited(Area2D area) {
        if (area.GetParent() != null && area.GetParent() is Player player) {
            light.Enabled = false;
            anima.Pause();
        }
    }
}

using Godot;
using System;

public partial class GhostFire : Area2D {
    [Export] public float speed = 50;

    private PointLight2D light;
    private AnimatedSprite2D anima;
    private PathFollow2D path;

    Tween onTween, offTween;


    public override void _Ready() {
        light = GetNode<PointLight2D>("%PointLight2D");
        anima = GetNode<AnimatedSprite2D>("%AnimatedSprite2D");
        path = GetNode<PathFollow2D>("%PathFollow2D");

        Connect("area_entered", new Callable(this, "OnPlayerEntered"));
        Connect("area_exited", new Callable(this, "OnPlayerExited"));

        light.Enabled = false;
        anima.Visible = false;

        anima.SelfModulate = new Color(1, 1, 1, 0);
    }

    public override void _Process(double delta) {
        path.Progress += (float)delta * speed;
    }

    public void OnPlayerEntered(Area2D area) {
        if (area.GetParent() != null && area.GetParent() is Player player) {
            if (player.burdens.Count <= 2) {
                light.Enabled = true;
                anima.Visible = true;
            }
        }
    }

    public void OnPlayerExited(Area2D area) {
        if (area.GetParent() != null && area.GetParent() is Player player) {
            light.Enabled = false;

            anima.Visible = false;
        }
    }
}

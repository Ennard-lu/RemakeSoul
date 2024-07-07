using Godot;
using System;
using System.Collections.Generic;

public partial class Skeleton : Area2D {
    private PointLight2D light;
    private Label label;

    private static bool said = false;

    private static List<String> words = new List<string>(new String[] {
        "What did the beach say as the tide came in? \nLong time, no sea." ,
        "Why won't the elephant use the computer?\nHe's afraid of the mouse!",
        "why is 6 afraid of 7?\nBecause 7,8,9.",
        "Why can't a bike stand up on it's own?\nBecause it's two tired.",
        "Press J to get part of your flesh!"});

    public override void _Ready() {
        light = GetNode<PointLight2D>("PointLight2D");
        label = GetNode<Label>("Label");
        Connect("area_entered", new Callable(this, "OnPlayerEntered"));
        Connect("area_exited", new Callable(this, "OnPlayerExited"));
    }

    public void OnPlayerEntered(Area2D area) {
        if (area.GetParent() != null && area.GetParent() is Player player) {
            if (player.burdens.Count <= 3) {
                light.Enabled = true;

                label.Visible = true;

                if (!said) {
                    label.Text = words[words.Count - 1];
                    said = true;
                } else label.Text = words[Random.Shared.Next() % words.Count];

            }
        }
    }

    public void OnPlayerExited(Area2D area) {
        if (area.GetParent() != null && area.GetParent() is Player player) {
            light.Enabled = false;
            label.Text = string.Empty;
            label.Visible = false;
        }
    }

}

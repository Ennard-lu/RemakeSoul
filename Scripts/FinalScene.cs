using Godot;
using System;

public partial class FinalScene : Control {
	private Label label;
	public override void _Ready() {

		label = GetNode<Label>("%Label");
		var tween = GetTree().CreateTween();
		label.Text = "Thanks for your playing!\nThe souls found their afterlife!";
		tween.TweenInterval(2);
		tween.TweenCallback(Callable.From(() => {
			label.Text = "Attributors : \nShuiMuJuLuo";
		}));
        tween.TweenInterval(2);
        tween.TweenCallback(Callable.From(() => {
            label.Text = "Free assets are used from:\nitch.io & godotshaders.com";
        }));
        tween.TweenInterval(2);
        tween.TweenCallback(Callable.From(() => {
            label.Text = "The demo cannot be accomplished\nwithout the contributors above.";
        }));
        tween.TweenInterval(2);
    }

}

using Godot;
using System;

public partial class EventSystem : Node {
    private TextureButton startButton;
    private TextureButton settingsButton;
    private SubViewport viewport;
    private Control uiRoot;

    public override void _Ready() {
        startButton = GetNode<TextureButton>("%StartButton");
        settingsButton = GetNode<TextureButton>("%SettingsButton");
        uiRoot = GetNode<Control>("%UIRoot");


        startButton.Connect("button_down", new Callable(this, "OnStartButtonPressed"));
        settingsButton.Connect("button_down", new Callable(this, "OnSettingsButtonPressed"));
    }

    public void OnStartButtonPressed() {
        AudioMgr.Instance.Play("ButtonClick");

        startButton.Disconnect("button_down", new Callable(this, "OnStartButtonPressed"));
        settingsButton.Disconnect("button_down", new Callable(this, "OnSettingsButtonPressed"));

        var canvas = ResourceLoader.Load<PackedScene>("res://Prefabs/curtain.tscn").Instantiate<CanvasLayer>();
        var curtain = canvas.GetNode<ColorRect>("Curtain");
        AddChild(canvas);

        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(curtain, "color:a", 255, 1.5).SetTrans(Tween.TransitionType.Quart);
        tween.TweenCallback(new Callable(this, "ChangeToGameScene"));
    }
    public void ChangeToGameScene() {
        AudioMgr.Instance.Play("SceneEnter");

        GameMgr.Instance.currentWinCount = 1;
        GameMgr.Instance.initLeave = (node) => {
            var canvas = node.GetNode<CanvasLayer>("Curtain");
            var curtain = canvas.GetNode<ColorRect>("Curtain");
            var color = curtain.Color;
            curtain.Color = new Color(color.R, color.G, color.B, 255);

            Tween tween = canvas.GetTree().CreateTween();
            tween.TweenProperty(curtain, "color:a", 0, 1.5).SetTrans(Tween.TransitionType.Quart);
            tween.TweenCallback(Callable.From(() => {
                canvas.QueueFree();
            }));

        };
        GetTree().ChangeSceneToFile("res://Scenes/game_scene_0.tscn");
    }

    public void OnSettingsButtonPressed() {
        AudioMgr.Instance.Play("ButtonClick");
        GD.Print("Hello2");

        var canvas = ResourceLoader.Load<PackedScene>("res://Prefabs/volume_panel.tscn").Instantiate<CanvasLayer>();
        AddChild(canvas);
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class GameMgr : Singleton<GameMgr> {

    private Node root;

    public const int ScreenWidth = 1152;
    public const int ScreenHeight = 648;
    public Gate CurrentGate;

    public int currentScene = 0;
    public const int sceneCount = 4;

    public int currentWinCount = 3;

    public Action<Node> initLeave;

    public void SetRoot(Node node) {
        root = node;
    }
    public void CreateBurden(BurdenBase burden, Vector2 position) {
        var tmp = MResource.Instance.LoadPrefab<BurdenInstance>($"res://Prefabs/{Helper.PascalToSnakeCase(burden.GetName())}.tscn");
        tmp.Position = position;
        root.AddChild(tmp);
    }

    public void RenewScene(Node node) {
        var canvas = node.GetNode<CanvasLayer>("Curtain");
        var curtain = canvas.GetNode<ColorRect>("Curtain");
        var color = curtain.Color;
        curtain.Color = new Godot.Color(color.R, color.G, color.B, 255);

        AudioMgr.Instance.Play("SceneEnter");

        Tween tween = canvas.GetTree().CreateTween();
        tween.TweenProperty(curtain, "color:a", 0, 1.5).SetTrans(Tween.TransitionType.Quart);
        tween.TweenCallback(Callable.From(() => {
            canvas.QueueFree();
        }));
    }

    public void EncloseScene(Node node, String newSceneName) {
        var canvas = ResourceLoader.Load<PackedScene>("res://Prefabs/curtain.tscn").Instantiate<CanvasLayer>();
        var curtain = canvas.GetNode<ColorRect>("Curtain");
        node.AddChild(canvas);

        Tween tween = node.GetTree().CreateTween();
        tween.TweenProperty(curtain, "color:a", 255, 1.5).SetTrans(Tween.TransitionType.Quart);
        tween.TweenCallback(Callable.From(() => {
            canvas.QueueFree();
            node.GetTree().ChangeSceneToFile($"res://Scenes/{newSceneName}.tscn");
        }));
        GD.Print($"Loading res://Scenes/{newSceneName}.tscn");
    }

    private bool getOver = false;
    public void ToTheNextLevel() {
        if (currentScene + 1 < sceneCount) {
            currentScene++;
            initLeave = (node) => {
                if (currentScene != 3)
                    RenewScene(node);
                if (currentScene == 1) {
                    var player = node.GetNode<Player>("%Player");
                    player.PutOnBurden(new Gravity());
                }
            };

            EncloseScene(root, $"game_scene_{currentScene}");
        } else if (!getOver) {
            var tmp = ResourceLoader.Load<PackedScene>("res://Prefabs/placeholder.tscn").Instantiate<CanvasLayer>();
            root.AddChild(tmp);
            getOver = true;
        }
    }
}


public partial class InputMgr : Singleton<GameMgr> {
    public bool Enabled { get; set; } = true;

    public bool IsActionJustPressed(String action) {
        return Enabled && Input.IsActionJustPressed(action);
    }

}

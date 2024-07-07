using Godot;
using System;

public partial class UIMgr : HBoxContainer {

	private Button resetButton;
	private Button settingsButton;

	public override void _Ready() {
		

		resetButton = GetNode<Button>("ResetButton");
		settingsButton = GetNode<Button>("SettingsButton");

		resetButton.Connect("button_down", Callable.From(() => {
			AudioMgr.Instance.CheckPlay("ButtonClick");

			GameMgr.Instance.EncloseScene(this, $"game_scene_{GameMgr.Instance.currentScene}");
		}));

        settingsButton.Connect("button_down", Callable.From(() => {
            AudioMgr.Instance.CheckPlay("ButtonClick");

            var canvas = ResourceLoader.Load<PackedScene>("res://Prefabs/volume_panel.tscn").Instantiate<CanvasLayer>();
            GetParent().AddChild(canvas);
        }));
    }
}

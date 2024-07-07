using Godot;
using System;

public partial class VolumePanel : CanvasLayer {
    private HSlider MusicSlider;
    private HSlider SFXSlider;

    private TextureButton OKButton;
    private TextureButton CancelButton;

    private const float min = -6;
    private const float max = 72;

    public override void _Ready() {
        MusicSlider = GetNode<HSlider>("%MusicSlider");
        SFXSlider = GetNode<HSlider>("%SFXSlider");
        OKButton = GetNode<TextureButton>("%OKButton");
        CancelButton = GetNode<TextureButton>("%CancelButton");

        MusicSlider.Value = AudioServer.GetBusVolumeDb(0);
        SFXSlider.Value = AudioServer.GetBusVolumeDb(1);

        GetTree().Paused = true;
        MusicSlider.Connect("value_changed", new Callable(this, "OnMusicSlide"));
        SFXSlider.Connect("value_changed", new Callable(this, "OnSFXSlide"));
        OKButton.Connect("button_down", Callable.From(() => {
            AudioMgr.Instance.RenewVolume();
            AudioMgr.Instance.Play("ButtonClick");

            QueueFree();
        }));

        CancelButton.Connect("button_down", Callable.From(() => {
            AudioMgr.Instance.SetVolume();
            AudioMgr.Instance.Play("ButtonClick");

            QueueFree();
        }));

    }

    public void OnMusicSlide(float value) {
        AudioServer.SetBusVolumeDb(0, value);
    }
    public void OnSFXSlide(float value) {
        AudioServer.SetBusVolumeDb(1, value);
    }

    public override void _ExitTree() {
        GetTree().Paused = false;
    }
}

using Godot;
using System;

public partial class WaterEffect : TextureRect {
    [Export] public Camera2D targetCamera;

    private SubViewport _viewport;
    private Camera2D _cam;

    public override void _Ready() {
        _viewport = GetNode<SubViewport>("SubViewport");
        _cam = GetNode<Camera2D>("SubViewport/Camera2D");
        _viewport.World2D = GetViewport().World2D;
        _viewport.Size = (Vector2I)(targetCamera.GetViewportRect().Size / targetCamera.Zoom);

        _cam.Transform = targetCamera.GlobalTransform;
        _cam.AnchorMode = targetCamera.AnchorMode;
    }
}
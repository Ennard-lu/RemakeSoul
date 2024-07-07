using Godot;
using System;

public partial class Edge : Node {
    private CollisionShape2D leftBound, rightBound, upBound, downBound;

    public static Edge Instance;

    public override void _Ready() {
        leftBound = GetNode<CollisionShape2D>("LeftBound/CollisionShape2D");
        rightBound = GetNode<CollisionShape2D>("RightBound/CollisionShape2D");
        upBound = GetNode<CollisionShape2D>("UpBound/CollisionShape2D");
        downBound = GetNode<CollisionShape2D>("DownBound/CollisionShape2D");

        EnableEdge(true);
        Instance = this;
    }


    public void EnableEdge(bool enable) {
        leftBound.Disabled = !enable;
        rightBound.Disabled = !enable;
        upBound.Disabled = !enable;
        downBound.Disabled = !enable;
    }
}

using Godot;
using System;

public partial class Edge : Node {
    private CollisionShape2D leftBound, rightBound, upBound, downBound;
    private float groundY;

    public static Edge Instance;

    public override void _Ready() {
        leftBound = GetNode<CollisionShape2D>("LeftBound/CollisionShape2D");
        rightBound = GetNode<CollisionShape2D>("RightBound/CollisionShape2D");
        upBound = GetNode<CollisionShape2D>("UpBound/CollisionShape2D");
        downBound = GetNode<CollisionShape2D>("DownBound/CollisionShape2D");

        groundY = GetNode<StaticBody2D>("GravityBase").Position.Y + 0.01f;

        EnableEdge(true);
        Instance = this;
    }

    public bool AboveGround(Vector2 position) => position.Y < groundY;


    public void EnableEdge(bool enable) {
        leftBound.Disabled = !enable;
        rightBound.Disabled = !enable;
        upBound.Disabled = !enable;
        downBound.Disabled = !enable;
    }
}

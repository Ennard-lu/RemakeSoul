using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Godot;
using static Godot.TextServer;

public enum BurdenType {
    WallThrough, BoudaryEcho, Gravity,
}

public partial class BurdenBase{
    public virtual String GetName() => "";
    public virtual String Description() => "";

    public virtual bool CanPickUp(Player play, Vector2 pos) => true;
    public virtual bool CanDispose(Player player) => true;
    public virtual void OnApplyBurden(Player player) { }
    public virtual void OnCancelBurden(Player player) { }

    public virtual void OnTestingMovement(Player player) { }

    public static BurdenBase GetInstance(BurdenType type) => type switch {
        BurdenType.WallThrough => new WallThrough(),
        BurdenType.BoudaryEcho => new BoundaryEcho(),
        BurdenType.Gravity => new Gravity(),
        _ => throw new Exception($"GetInstance out of range {(int) type}"),
    };
}

public class WallThrough : BurdenBase {
    public override String GetName() => "WallThrough";
    public override String Description() => "You cannot walk through the walls any more.";
    public override bool CanPickUp(Player player, Vector2 pos) {
        return !player.InWall();
    }
    public override bool CanDispose(Player player) {
        return !player.InWall();
    }
    public override void OnApplyBurden(Player player) {
        player.EnableWallThrough(false);
    }
    public override void OnCancelBurden(Player player) {
        player.EnableWallThrough(true);
    }

    public override void OnTestingMovement(Player player) { }
}

public class BoundaryEcho : BurdenBase {
    public override String GetName() => "BoundaryEcho";
    public override String Description() => "You cannot walk into the boundary and return from the other side any more.";
    public override bool CanPickUp(Player player, Vector2 pos) {
        return player.HasNoCopy();
    }
    public override bool CanDispose(Player player) {
        return true;
    }
    public override void OnApplyBurden(Player player) {
        player.EnableBoudaryEcho(false);
    }
    public override void OnCancelBurden(Player player) {
        player.EnableBoudaryEcho(true);
    }

    public override void OnTestingMovement(Player player) { }
}

public class Gravity : BurdenBase {
    public override String GetName() => "Gravity";
    public override String Description() => "You are confined to the ground now.";
    public override bool CanPickUp(Player player, Vector2 pos) {
        return Edge.Instance.AboveGround(player.Position);
    }
    public override bool CanDispose(Player player) {
        return true;
    }
    public override void OnApplyBurden(Player player) {
        player.EnableGravity(false);
    }
    public override void OnCancelBurden(Player player) {
        player.EnableGravity(true);
    }

    public override void OnTestingMovement(Player player) {
        Vector2 velocity = player.Velocity;
        if (!player.IsOnFloor())
            velocity.Y += player.gravity * (float)player.deltaTime;

        if (Input.IsActionJustPressed("jump") && player.IsOnFloor())
            velocity.Y = player.JumpVelocity;

        if (player.direction != Vector2.Zero) {
            velocity.X = player.direction.X * player.Speed;
        } else {
            velocity.X = Mathf.MoveToward(player.Velocity.X, 0, player.Speed);
        }

        player.velocity = velocity;
    }
}

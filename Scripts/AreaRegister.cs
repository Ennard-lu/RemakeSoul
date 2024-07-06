using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;


public partial class AreaRegister : Area2D {
    public List<Area2D> collidedAreas = new();
    public List<Node2D> collidedBodies = new();

    public override void _Ready() {
        Connect("area_entered", new Callable(this, "OnAreaEntered"));
        Connect("area_exited", new Callable(this, "OnAreaExited"));
        Connect("body_entered", new Callable(this, "OnBodyEntered"));
        Connect("body_exited", new Callable(this, "OnBodyExited"));
    }

    public void OnAreaEntered(Area2D body) {
        collidedAreas.Add(body);
        GD.Print($"Area enter detected from {Name}, {Position}: {body.GetType().Name}, {body.Name}");
    }

    public void OnAreaExited(Area2D body) {
        collidedAreas.Remove(body);
        GD.Print($"Area exit detected from {Name}, {Position}: {body.GetType().Name}, {body.Name}");
    }

    public void OnBodyEntered(Node2D body) {
        collidedBodies.Add(body);
        GD.Print($"Body enter detected from {Name}, {Position}: {body.GetType().Name}, {body.Name}");
    }

    public void OnBodyExited(Node2D body) {
        collidedBodies.Remove(body);
        GD.Print($"Body exit detected from {Name}, {Position}: {body.GetType().Name}, {body.Name}");
    }

    public bool CheckGroupOfAreas(String label) {
        foreach (var area in collidedAreas) {
            if (area.IsInGroup(label)) return true;
        }
        return false;
    }

    public List<Area2D> GetGroupOfAreas(String label) {
        List<Area2D> ret = new();
        foreach (var area in collidedAreas) {
            if (area.IsInGroup(label)) {
                ret.Add(area);
            }
        }
        return ret;
    }

    public bool CheckGroupOfBodies(String label) {
        foreach (var area in collidedBodies) {
            if (area.IsInGroup(label)) return true;
        }
        return false;
    }

    public List<Node2D> GetGroupOfBodies(String label) {
        List<Node2D> ret = new();
        foreach (var area in collidedBodies) {
            if (area.IsInGroup(label)) {
                ret.Add(area);
            }
        }
        return ret;
    }
}

using Godot;
using System;

public class MResource : Singleton<MResource> {
    public T LoadPrefab<T>(String path)
        where T : Node {
        return ResourceLoader.Load<PackedScene>(path).Instantiate() as T;
    }
}

public interface IFactory<T> {
    public static T Produce() => default;
}

public class DefaultFactory<T> : IFactory<T> where T : new() {
    public static T Produce() {
        return new T();
    }
}

/// <summary>
/// Provide the factory a default way to manufacture Nodes. Noting that the behavior of T with a constructor
/// is not expected.
/// </summary>
/// <typeparam name="T">product type</typeparam>
public class PrefabFactory<T> : IFactory<T> where T : Node {
    private static readonly String path = $"res://Prefabs/{Helper.PascalToSnakeCase(typeof(T).Name)}.tscn";
    private static readonly T prototype = MResource.Instance.LoadPrefab<T>(path);
    public static T Produce() {
        return prototype.Duplicate() as T;
    }
}

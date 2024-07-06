using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class GameMgr : Singleton<GameMgr> {

    private Node root;

    public void SetRoot(Node node) {
        root = node;
    }
    public void CreateBurden(BurdenBase burden, Vector2 position) {
        var tmp = MResource.Instance.LoadPrefab<BurdenInstance>($"res://Prefabs/{Helper.PascalToSnakeCase(burden.GetName())}.tscn");
        tmp.Position = position;
        root.AddChild(tmp);
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class BurdenIcon : Sprite2D {
    private Sprite2D exciteImg;

    public override void _Ready() {
        exciteImg = GetNode<Sprite2D>("ExciteImg");
        exciteImg.Visible = false;
    }

    public void Light() {
        this.Texture = exciteImg.Texture;
        exciteImg.Visible = true;
    }
}

using Godot;
using System;
using System.Collections.Generic;
using System.Drawing;

public partial class Player : CharacterBody2D
{
	[Export] public float Speed = 170f;
	[Export] public float JumpVelocity = -280f;

	public Vector2 direction;
	public List<BurdenBase> burdens = new();
	private CollisionShape2D closeCollsion;
	private AreaRegister closeArea;
	private AnimatedSprite2D anima;

	private Vector2 screenOffset;
	private Rect2 screenRect;
	private Rect2 screenRectEdge;
	private float enlargeRate = 20f;
	private (int, int)?[] copyPositions = new (int, int)?[3];
	private List<CollisionShape2D> collsionCopy = new();
	private List<CollisionShape2D> areaCopy = new();
	private List<AnimatedSprite2D> animaCopy = new();

    // Get the gravity from the project settings to be synced with RigidBody nodes.
    public float gravity = ProjectSettings.GetSetting("physics/2d/default_gravity").AsSingle();
	public double deltaTime { get; private set; } = 0;
	public Vector2 velocity;
	
	public override void _Ready() {
		closeCollsion = GetNode<CollisionShape2D>("CollisionShape2D");
		closeArea = GetNode("CloseArea") as AreaRegister;
		anima = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GameMgr.Instance.SetRoot(GetTree().Root);
		EnableWallThrough(true);

		for (int i = 0; i < 3; i++) {
			collsionCopy.Add(closeCollsion.Duplicate() as CollisionShape2D);
			areaCopy.Add(closeArea.GetNode("CollisionShape2D").Duplicate() as CollisionShape2D);
			animaCopy.Add(anima.Duplicate() as AnimatedSprite2D);
			areaCopy[i].
			AddChild(collsionCopy[i]);
			closeArea.AddChild(areaCopy[i]);
			AddChild(animaCopy[i]);
		}
		DisableAllCopy();
		CollisionMask = 0;

		screenOffset = GetViewport().GetVisibleRect().Abs().Size / GetViewport().GetCamera2D().Zoom;
		screenRect.Position = Vector2.Zero;
		screenRect.Size = screenOffset;

        GD.Print(screenOffset);

		//screenRectEdge.Position = -screenOffset / 2.0f * enlargeRate;
		//screenRectEdge.Size = screenOffset * enlargeRate;
		screenRectEdge = screenRect.Grow(enlargeRate);

        GD.Print(screenRectEdge);
    }

    public override void _Process(double delta) {
        if (Input.IsActionJustPressed("pickup")) {
			var burdens = closeArea.GetGroupOfAreas("Burden");
			GD.Print(burdens.Count);
			foreach (var burden in burdens) {
				if (burden.GetParent() is BurdenInstance burdenInst) {
					var burdenGen = BurdenBase.GetInstance(burdenInst.burden);
					GD.Print(burdenGen.GetName());
                    if (burdenGen.CanPickUp(this, Position)) {
						//if (GameMgr.Instance.CurrentGate == null) GD.Print("NULL");
						GameMgr.Instance.CurrentGate.OnPlayerGetBurden(burdenGen);
                        PutOnBurden(burdenGen);

						AudioMgr.Instance.Play("PickUp");
						

                        GD.Print($"Player get burden {burdenGen.GetName()}");

                        burdenInst.QueueFree();
                    }
                } else {
					GD.Print("Wrong in Player _Process!");
				}
			}
		}

		/*
		if (Input.IsActionJustPressed("dispose") && burdens.Count !=0 ) {
			if (burdens[0].CanDispose(this)) {
				GameMgr.Instance.CreateBurden(burdens[0], Position);
				burdens[0].OnCancelBurden(this);
				burdens.RemoveAt(0);
			}
		} */
		CheckCopies();

		if (Velocity.X != 0) {
			anima.FlipH = Velocity.X < 0;
        }
    }
	public void PutOnBurden(BurdenBase burdenGen) {
		burdenGen.OnApplyBurden(this);
		burdens.Add(burdenGen);
    }
    public override void _PhysicsProcess(double delta) {
		deltaTime = delta;

		direction = Input.GetVector("left", "right", "up", "down");
        if (direction != Vector2.Zero) {
            velocity.X = direction.X * Speed;
            velocity.Y = direction.Y * Speed;
        } else {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Y = Mathf.MoveToward(Velocity.Y, 0, Speed);
        }

        foreach (var burden in burdens) {
			burden.OnTestingMovement(this);
		}
		
		var validPositions = CheckValidPosition(Position, screenRect);
		if (validPositions.Count !=0) Position = validPositions[0].Item1;

		Velocity = velocity;
		MoveAndSlide();
	}

	public void EnableWallThrough(bool enable) {
		if (enable)
			RemoveCollsionLayerHelper(2);
		else
			AddCollsionLayerHelper(2);
	}

	public void EnableBoudaryEcho(bool enable) {
        if (enable)
            RemoveCollsionLayerHelper(8);
        else
            AddCollsionLayerHelper(8);
    }

    public void EnableGravity(bool enable) {
        if (enable) {
			RemoveCollsionLayerHelper(1);
            RemoveCollsionLayerHelper(9);
        } else {
			AddCollsionLayerHelper(1);
			AddCollsionLayerHelper(9);
		} 
    }

    public void CheckCopies() {
		var validPosition = CheckValidPosition(Position, screenRectEdge);

		for (int i = 0; i < 3; ++i) {
			if (copyPositions[i] == null) continue;
			
			bool isFound = false;
			for (int j = 0; j < validPosition.Count; ++j) {
                (_, int x, int y) = validPosition[j];
                if (copyPositions[i].Value == (x, y)) {
					validPosition.RemoveAt(j);
					isFound = true;
					break;
				}
			}
			if (!isFound) {
				copyPositions[i] = null;
				EnableCopy(i, false);
			}
		}

        for (int i = 0; i < 3; ++i) {
			if (validPosition.Count == 0) break;
			if (!copyPositions[i].HasValue) {
                (_, int x, int y) = validPosition[0];
                copyPositions[i] = (x, y);
				EnableCopy(i, true);

				var offset = new Vector2(screenOffset.X * x , screenOffset.Y * y);

				collsionCopy[i].Position = closeCollsion.Position + offset;
				areaCopy[i].Position = offset;
				animaCopy[i].Position = anima.Position + offset;

				validPosition.RemoveAt(0);
			}
		}

		if (validPosition.Count != 0) {
			GD.PrintErr("Too much Copy");
		}
	}

	private List<(Vector2, int, int)> CheckValidPosition(Vector2 position, Rect2 rect) {
		List<(Vector2, int, int)> validPositions = new();

		for (int offsetX = -1; offsetX <= 1; ++offsetX) {
			for (int offsetY = -1; offsetY <= 1; ++offsetY) {
				if (offsetX == 0 && offsetY == 0) continue;

				var newPosition = new Vector2(position.X + offsetX * screenOffset.X,
					position.Y + offsetY * screenOffset.Y);
				if (rect.HasPoint(newPosition)) {
					validPositions.Add((newPosition, offsetX, offsetY));
				}
			}
		}
		return validPositions;
	}

	private void DisableAllCopy() {
		for (int i = 0; i < 3; ++i) {
			EnableCopy(i, false);
		}
	}
    private void EnableCopy(int i, bool enable) {
		GD.Print($"{i} {(enable ? "enable" : "disable")}");
		collsionCopy[i].Disabled = !enable;
		areaCopy[i].Disabled = !enable;
		animaCopy[i].Visible = enable;
    }
	
	public bool HasNoCopy() {
		return !closeArea.CheckGroupOfBodies("Edge");
	}

	public void AddCollsionLayerHelper(int num) {
		CollisionMask |= (uint)(1 << (num-1));
		GD.Print(CollisionMask);
	}
    public void RemoveCollsionLayerHelper(int num) {
        CollisionMask &= ~(uint)(1 << (num-1));
        GD.Print(CollisionMask);
    }

	public bool MeetWinConditions() => burdens.Count >= GameMgr.Instance.currentWinCount;

    public bool InWall() {
		return closeArea.CheckGroupOfBodies("Wall");
	}
}

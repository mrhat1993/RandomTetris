using UnityEngine;
using System.Collections;
using System;
using System.Linq;

public enum ShapeType
{
    I,
    T,
    O,
    L,
    J,
    S,
    Z
}

public class TetrisShape : MonoBehaviour
{
    [HideInInspector]
    public ShapeType type;

    private ShapeMovementController _movementController;
    public ShapeMovementController MovementController => _movementController ? _movementController : _movementController = GetComponent<ShapeMovementController>();

    void Awake()
    {
        AssignRandomColor();
    }

    void Start()
    {
        // Default position not valid? Then it's game over
        if (!Managers.Grid.IsValidGridPosition(this.transform))
        {
            Managers.Game.SetState(typeof(GameOverState));
            Destroy(this.gameObject);
        }
    }

    void AssignRandomColor()
    {
        Color temp = Managers.Palette.TurnRandomColorFromTheme();
        foreach (SpriteRenderer renderer in GetComponentsInChildren<SpriteRenderer>().ToList())
            renderer.color = temp;
    }
}

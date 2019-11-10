using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
	public GameObject ShapeBlock;
	public int MaxBlocks = 5;
	public GameObject[] shapeTypes;

    public void Spawn()
	{
		GenerateNewShape();
        Managers.Input.isActive = true;
    }

    private void GenerateNewShape()
    {
	    var newShape = new GameObject("Shape_Random");
	    var countBlocks = Random.Range(1, MaxBlocks + 1);
	    var matrix = new List<(float, float)>();
	    var prevPos = Vector3.zero;
	    var minPivotX = 9999f;
	    var maxPivotX = 0f;
	    var minPivotY = 9999f;
	    var maxPivotY = 0f;
	    for (int i = 0; i < countBlocks; i++)
	    {
		    var hor = Random.value > 0.5f;
		    var pos = (prevPos.x, prevPos.y);
		    while (!EmptyInMatrix(pos))
		    {
			    pos = ((hor ? (Random.value > 0.5f ? 1f : -1f) : 0f) + prevPos.x, (!hor ? (Random.value > 0.5f ? 1f : -1f) : 0f) + prevPos.y);
		    }
		    matrix.Add(pos);
		    if (prevPos != Vector3.zero)
		    {
			    if (minPivotX > pos.x) minPivotX = pos.x;
			    if (maxPivotX < pos.x) maxPivotX = pos.x;
			    if (minPivotY > pos.y) minPivotY = pos.y;
			    if (maxPivotY < pos.y) maxPivotY = pos.y;
		    }
		    var posVector = new Vector3(pos.Item1, pos.Item2, 0f);
		    Instantiate(ShapeBlock, posVector, Quaternion.identity, newShape.transform);
		    prevPos = posVector;
	    }
	    var pivot = new GameObject("Pivot");
	    pivot.transform.parent = newShape.transform;
	    pivot.transform.localPosition = countBlocks > 1 ? new Vector3(Mathf.RoundToInt(Mathf.Sign(maxPivotX - minPivotX) * (Mathf.Abs(maxPivotX) - Mathf.Abs(minPivotX)) * 0.5f), Mathf.RoundToInt(Mathf.Sign(maxPivotY - minPivotY) * (Mathf.Abs(maxPivotY) - Mathf.Abs(minPivotY)) * 0.5f), 0f) : Vector3.zero;
	    var maxHeight = 0f;
	    for (int i = 0; i < matrix.Count; i++)
		    if (matrix[i].Item2 > maxHeight) maxHeight = matrix[i].Item2;
	    newShape.transform.position = new Vector3(4f, 15f-maxHeight);
	    var tetrisShape = newShape.AddComponent<TetrisShape>();
	    Managers.Game.currentShape = tetrisShape;
	    newShape.AddComponent<ShapeMovementController>().rotationPivot = pivot.transform;
	    newShape.transform.parent = Managers.Game.blockHolder;

	    bool EmptyInMatrix((float, float) checkPos) => !matrix.Contains(checkPos);
    }
}

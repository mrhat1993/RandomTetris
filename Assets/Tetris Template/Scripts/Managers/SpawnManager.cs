using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
	public GameObject ShapeBlock;
	public int MaxBlocks = 5;
	public GameObject[] shapeTypes;

    public GameObject blockPrefab;
    public GameObject emptyShapePrefab;

    public void Spawn()
	{
		/*var i = Random.Range(0, shapeTypes.Length);

		// Spawn Group at current Position
		var temp = GenerateShape(Random.Range(1,7));//Instantiate(shapeTypes[i]);
		Managers.Game.currentShape = temp.GetComponent<TetrisShape>();
		temp.transform.parent = Managers.Game.blockHolder;
		Managers.Input.isActive = true;*/
		
		GenerateNewShape();
        Managers.Input.isActive = true;
    }

    private void GenerateNewShape()
    {
	    var newShape = new GameObject("Shape_Random");
	    var countBlocks = Random.Range(1, MaxBlocks + 1);
	    var matrix = new List<(float, float)>();
	    var prevPos = Vector3.zero;
	    var minPivotX = 0f;
	    var maxPivotX = 0f;
	    var minPivotY = 0f;
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

    public TetrisShape GenerateShape(int blocksCount)
    {
        var shapeMatrixWidth = 5;
        var shapeMatrixHeight = 5;

        var shapeMatrix = new List<List<bool>>();

        for (var x = 0; x < shapeMatrixHeight; x++)
        {
            shapeMatrix.Add(new List<bool>());
            for (var y = 0; y < shapeMatrixWidth; y++)
            {
                shapeMatrix[x].Add(false);
            }
        }

        var generatedBlocks = new List<(int, int)>();

        var randomI = Random.Range(0, shapeMatrixWidth);
        var randomJ = Random.Range(0, shapeMatrixHeight);
        shapeMatrix[randomI][randomJ] = true;
        generatedBlocks.Add((randomI, randomJ));

        for (var k = 0; k < blocksCount-1; k++)
        {
            var randomGeneratedBlock = (0, 0);
            var availableDirections = new List<(int, int)>();
            do
            {
                randomGeneratedBlock = generatedBlocks[Random.Range(0, generatedBlocks.Count)];

                availableDirections.Clear();

                var left = (randomGeneratedBlock.Item1, randomGeneratedBlock.Item2 - 1);
                left.Item1 = Mathf.Clamp(left.Item1, 0, shapeMatrixWidth - 1);
                left.Item2 = Mathf.Clamp(left.Item2, 0, shapeMatrixHeight - 1);
              
                if (!shapeMatrix[left.Item1][left.Item2])
                {
                    availableDirections.Add(left);
                }

                var right = (randomGeneratedBlock.Item1, randomGeneratedBlock.Item2 + 1);
                right.Item1 = Mathf.Clamp(right.Item1, 0, shapeMatrixWidth - 1);
                right.Item2 = Mathf.Clamp(right.Item2, 0, shapeMatrixHeight - 1);
                if (!shapeMatrix[right.Item1][right.Item2])
                {
                    availableDirections.Add(right);
                }

                var up = (randomGeneratedBlock.Item1 - 1, randomGeneratedBlock.Item2);
                up.Item1 = Mathf.Clamp(up.Item1, 0, shapeMatrixWidth - 1);
                up.Item2 = Mathf.Clamp(up.Item2, 0, shapeMatrixHeight - 1);
                if (!shapeMatrix[up.Item1][up.Item2])
                {
                    availableDirections.Add(up);
                }

                var down = (randomGeneratedBlock.Item1 + 1, randomGeneratedBlock.Item2);
                down.Item1 = Mathf.Clamp(down.Item1, 0, shapeMatrixWidth - 1);
                down.Item2 = Mathf.Clamp(down.Item2, 0, shapeMatrixHeight - 1);
                if (!shapeMatrix[down.Item1][down.Item2])
                {
                    availableDirections.Add(down);
                }

            }
            while (availableDirections.Count <= 0);

            var randomDirection = availableDirections[Random.Range(0, availableDirections.Count)];

            print("randomDir" + randomDirection.Item1 + "-" + randomDirection.Item2);

            shapeMatrix[randomDirection.Item1][randomDirection.Item2] = true;
            generatedBlocks.Add((randomDirection.Item1, randomDirection.Item2));
        }
        var i = 0;
        while(i < shapeMatrix.Count)
        {
            var rowBlocksCount = shapeMatrix[i].Count(x => x);
            if (rowBlocksCount > 0) i++;
            else shapeMatrix.RemoveAt(i);
        }
        
        var j = 0;
        while (j < shapeMatrix[0].Count)
        {
            var columnHasBlocks = false;
            for (var k = 0; k < shapeMatrix.Count; k++)
            {
                if(shapeMatrix[k][j])
                {
                    columnHasBlocks = true;
                    break;
                }
            }

            if (columnHasBlocks) j++;
            else
            {
                for (var k = 0; k < shapeMatrix.Count; k++)
                {
                    shapeMatrix[k].RemoveAt(j);
                }
            }
        }

        var shape = Instantiate(emptyShapePrefab);

        for (var l = 0; l < shapeMatrix.Count; l++)
        {
            for (var m = 0; m < shapeMatrix[l].Count; m++)
            {
                if (shapeMatrix[l][m])
                {
                    var block = Instantiate(blockPrefab);
                    block.transform.parent = shape.transform;
                    block.transform.localPosition = new Vector3(l, m, 0);
                }
            }
        }

        var pivot = shape.transform.Find("Pivot");
        pivot.localPosition = new Vector3(Mathf.RoundToInt((shapeMatrix.Count - 1) / 2), Mathf.RoundToInt((shapeMatrix[0].Count - 1) / 2));

        return shape.GetComponent<TetrisShape>();
    }
}

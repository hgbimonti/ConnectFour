using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float xCellSize = 0.9f;
    [SerializeField]
    private float yCellSize = 0.8f;

    [SerializeField]
    private int gridSize = 7;

    [SerializeField]
    private bool showGridCells;

    public Vector3 GetNearestCellOnGrid(Vector3 position)
    {
        int xCount = Mathf.RoundToInt(position.x / xCellSize);
        int yCount = Mathf.RoundToInt(position.y / yCellSize);

        Vector3 result = new Vector3((float)xCount * xCellSize, (float)yCount * yCellSize, 0f);

        result += transform.position;

        return result;
    }

    public Vector3 GetCellByCoordinate(int x, int y) 
    {
        Vector3 result = new Vector3((float)x * xCellSize, (float)y * yCellSize, 0f);

        return result;
    }

    private void OnDrawGizmos()
    {
        if (showGridCells) 
        {
            Gizmos.color = Color.green;

            for (float x = 0; x < gridSize - 1; x += xCellSize)
            {
                for (float y = 0; y < gridSize - 1; y += yCellSize)
                {
                    if (y > 4)
                        break;

                    var point = GetNearestCellOnGrid(new Vector3(x, y, 0f));
                    Gizmos.DrawSphere(point, 0.1f);
                }
            }
        }
    }

}

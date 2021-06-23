using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField]
    private float _xCellSize = 0.9f;
    [SerializeField]
    private float _yCellSize = 0.8f;

    [SerializeField]
    private int _gridX = 7;
    [SerializeField]
    private int _gridY = 6;

    [SerializeField]
    private bool _showGridCells;

    public int GridWidth { get => _gridX; }
    public int GridHeight { get => _gridY; }

    private int[,] _gridMatrix;

    private void Start()
    {
        _gridMatrix = new int[_gridX, _gridY];
    }

    public Vector2 GetAvailableCellCoordinatesInColumn(int columnID) 
    {
        Vector2 result = new Vector2(0, 0);
        
        for (int x = 0; x < _gridMatrix.GetLength(0); x++)
        {
            for (int y = 0; y < _gridMatrix.GetLength(1); y++)
            {
                if (x == columnID) 
                {
                    result.x = x;

                    if (_gridMatrix[x, y] > 0)
                        result.y = y + 1;

                    if (result.y > _gridY - 1)
                        result.y = -1000;
                }
            }
        }

        return result;
    }

    public void SetCellAsUsedInCoordinates(Vector2 cellCoordinates, int playerID) 
    {
        _gridMatrix[(int)cellCoordinates.x, (int)cellCoordinates.y] = playerID;
    }

    public Vector2 GetNearestCellOnGrid(Vector2 position)
    {
        int xCount = Mathf.RoundToInt(position.x / _xCellSize);
        int yCount = Mathf.RoundToInt(position.y / _yCellSize);

        Vector3 result = new Vector2((float)xCount * _xCellSize, (float)yCount * _yCellSize);
        result += transform.position;

        return result;
    }

    public Vector2 GetNearestCellCoordinates(Vector2 position)
    {
        int xCount = Mathf.RoundToInt(position.x / _xCellSize);
        int yCount = Mathf.RoundToInt(position.y / _yCellSize);

        return new Vector2(xCount, yCount);
    }

    public Vector2 GetCellByCoordinates(Vector2 coordinates) 
    {
        Vector3 result = new Vector2((float)coordinates.x * _xCellSize, (float)coordinates.y * _yCellSize);
        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        if (_showGridCells) 
        {
            Gizmos.color = Color.green;

            for (float x = 0; x < _gridX - 1; x += _xCellSize)
            {
                for (float y = 0; y < _gridY - 1; y += _yCellSize)
                {
                    if (y > 4)
                        break;

                    var point = GetNearestCellOnGrid(new Vector2(x, y));
                    Gizmos.DrawSphere(point, 0.1f);
                }
            }
        }
    }

}

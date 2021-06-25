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

    public void SetCellAsUsedInCoordinates(Vector2 cellCoordinates, GameManager.Players player) 
    {
        _gridMatrix[(int)cellCoordinates.x, (int)cellCoordinates.y] = (int)player;
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

    public void LookForSequence() 
    {
        StartCoroutine(CheckSequencesInColumns());
    }

    IEnumerator CheckSequencesInColumns() 
    {
        int matches = 0;

        Debug.Log("CHECKING COLUMNS...");
        Debug.Log("-------------------");

        for (int x = 0; x < _gridMatrix.GetLength(0); x++)
        {
            if (GameManager.Instance.IsGameOver)
                break;
            
            int playerMatchID = 0;
            
            for (int y = 0; y < _gridMatrix.GetLength(1); y++)
            {
                if (_gridMatrix[x, y] != 0)
                {
                    yield return new WaitForSeconds(0f);

                    if (playerMatchID != _gridMatrix[x, y] || playerMatchID == 0)
                    {
                        matches = 1;

                        Debug.Log("(x: " + x + " y: " + y + ") Player" + _gridMatrix[x, y] + " FIRST CIRCLE!");
                    }
                    else 
                    {
                        playerMatchID = _gridMatrix[x, y];
                        matches++;

                        Debug.Log("(x: " + x + " y: " + y + ") Player" + playerMatchID + " CIRCLE MATCH = " + matches);
                    }

                    playerMatchID = _gridMatrix[x, y];

                    if(matches >= 4) 
                    {
                        Debug.Log("Player" + playerMatchID + " WINS!");
                        Debug.Log("GAME OVER");

                        GameManager.Instance.GameOver();

                        break;
                    }
                }
                else 
                {
                    //Debug.Log("(x: " + x + " y: " + y + ") empty cell.");
                    matches = 0;
                }
            }
        }

        if (!GameManager.Instance.IsGameOver) 
        {
            Debug.Log("NO WINNER FOUND IN COLUMNS.");

            StartCoroutine(CheckSequencesInLines());
        }
    }

    IEnumerator CheckSequencesInLines()
    {
        int matches = 0;

        Debug.Log("CHECKING LINES...");
        Debug.Log("-------------------");

        for (int y = 0; y < _gridMatrix.GetLength(0); y++)
        {
            if (GameManager.Instance.IsGameOver)
                break;

            int playerMatchID = 0;

            for (int x = 0; x < _gridMatrix.GetLength(1); x++)
            {
                //Debug.Log("(x: " + x + " y: " + y + ") current cell.");

                if (y == _gridY)
                    break;

                if (_gridMatrix[x, y] != 0)
                {
                    yield return new WaitForSeconds(0f);

                    if (playerMatchID != _gridMatrix[x, y] || playerMatchID == 0)
                    {
                        matches = 1;

                        Debug.Log("(x: " + x + " y: " + y + ") Player" + _gridMatrix[x, y] + " FIRST CIRCLE!");
                    }
                    else
                    {
                        playerMatchID = _gridMatrix[x, y];
                        matches++;

                        Debug.Log("(x: " + x + " y: " + y + ") Player" + playerMatchID + " CIRCLE MATCH = " + matches);
                    }

                    playerMatchID = _gridMatrix[x, y];

                    if (matches >= 4)
                    {
                        Debug.Log("Player" + playerMatchID + " WINS!");
                        Debug.Log("GAME OVER");

                        GameManager.Instance.GameOver();

                        break;
                    }
                }
                else
                {
                    //Debug.Log("(x: " + x + " y: " + y + ") empty cell.");
                    matches = 0;
                }
            }
        }

        if (!GameManager.Instance.IsGameOver)
        {
            Debug.Log("NO WINNER FOUND IN LINES.");
        }
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

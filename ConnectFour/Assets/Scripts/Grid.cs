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

        Debug.Log("-------------------");
        Debug.Log("CHECKING COLUMNS...");

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

        Debug.Log("-------------------");
        Debug.Log("CHECKING ROWS...");

        for (int y = 0; y < _gridMatrix.GetLength(1); y++)
        {
            if (GameManager.Instance.IsGameOver)
                break;

            int playerMatchID = 0;

            for (int x = 0; x < _gridMatrix.GetLength(0); x++)
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
                    matches = 0;
                }
            }
        }

        if (!GameManager.Instance.IsGameOver)
        {
            Debug.Log("NO WINNER FOUND IN ROWS.");

            StartCoroutine(CheckDiagonalsRight());
        }
    }

    IEnumerator CheckDiagonalsRight() 
    {
        int matches = 0;

        Debug.Log("-------------------");
        Debug.Log("CHECKING DIAGONALS RIGHT...");

        int rows = _gridMatrix.GetLength(0);
        int columns = _gridMatrix.GetLength(1);

        // number of secondary diagonals
        int d = columns + rows - 1;
        int x, y;

        // go through each diagonal
        for (int i = 0; i < d; i++)
        {
            // row to start
            if (i < columns)
                x = 0;
            else
                x = i - columns + 1;

            // column to start
            if (i < columns)
                y = i;
            else
                y = columns - 1;

            int playerMatchID = 0;

            do
            {
                if(_gridMatrix[x, y] != 0) 
                {
                    yield return new WaitForSeconds(0.0f);

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
                    matches = 0;
                }

                x++;
                y--;
            }
            while (x < rows && y >= 0);

            if (GameManager.Instance.IsGameOver)
                break;
        }

        if (!GameManager.Instance.IsGameOver)
        {
            Debug.Log("NO WINNER FOUND IN DIAGONALS RIGHT.");

            StartCoroutine(CheckDiagonalsLeft());
        }
    }

    IEnumerator CheckDiagonalsLeft()
    {
        int matches = 0;

        Debug.Log("-------------------");
        Debug.Log("CHECKING DIAGONALS LEFT...");

        int rows = _gridMatrix.GetLength(0);
        int columns = _gridMatrix.GetLength(1);

        // number of secondary diagonals
        int d = columns + rows - 1;
        int x, y;

        int columnsAux = 0;

        // go through each diagonal
        for (int i = 0; i < 12; i++)
        {
            // row to start
            if (i < columns) 
            {
                x = columns;
            }
            else 
            {
                x = (i - rows) + (columns - columnsAux);
                columnsAux += 2;
            }
                
            // column to start
            if (i < columns)
                y = i;
            else
                y = columns - 1;

            int playerMatchID = 0;

            do
            {
                if(_gridMatrix[x, y] != 0) 
                {
                    if (_gridMatrix[x, y] != 0)
                    {
                        yield return new WaitForSeconds(0.0f);

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
                        matches = 0;
                    }
                }

                x--;
                y--;
            }
            while (x >= 0 && y >= 0);

            if (GameManager.Instance.IsGameOver)
                break;
        }

        if (!GameManager.Instance.IsGameOver)
        {
            Debug.Log("NO WINNER FOUND IN DIAGONALS LEFT.");
            Debug.Log("--- END OF CHECK ---");
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

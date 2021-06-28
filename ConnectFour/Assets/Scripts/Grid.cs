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

    private GameObject _cellPin;

    public int GridWidth { get => _gridX; }
    public int GridHeight { get => _gridY; }

    private int[,] _gridMatrix;
    private List<Vector2> _matchesPositions;

    private void Start()
    {
        _gridMatrix = new int[_gridX, _gridY];

        _cellPin = GameManager.Instance.GetTestingPrefab();
        _cellPin.SetActive(false);
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
        if (GameManager.Instance.TestingMode) 
        {
            Debug.Log("-------------------");
            Debug.Log("CHECKING COLUMNS...");
        }
        
        for (int x = 0; x < _gridMatrix.GetLength(0); x++)
        {
            if (GameManager.Instance.IsGameOver)
                break;

            int matches = 0;

            int cellCircleID;
            int lastCircleID = 0;

            _matchesPositions = new List<Vector2>();

            for (int y = 0; y < _gridMatrix.GetLength(1); y++)
            {
                if (GameManager.Instance.TestingMode) 
                {
                    yield return new WaitForSeconds(0.05f);
                    _cellPin.transform.position = GetCellByCoordinates(new Vector2(x, y));
                    _cellPin.SetActive(true);
                }

                cellCircleID = _gridMatrix[x, y];

                if (_gridMatrix[x, y] != 0)
                {                    
                    if ((cellCircleID == lastCircleID) || (_gridMatrix[x, 0] != 0  && lastCircleID == 0))
                    {
                        matches++;

                        if (GameManager.Instance.TestingMode) 
                        {
                            Debug.Log("(x: " + x + " y: " + y + ") Circle for player" + cellCircleID + " CIRCLE MATCH = " + matches);

                            _matchesPositions.Add(GetCellByCoordinates(new Vector2(x, y)));
                        }
                            
                        if (matches >= 4)
                        {
                            if (GameManager.Instance.TestingMode) 
                            {
                                Debug.Log("Player" + cellCircleID + " WINS!");
                                Debug.Log("GAME OVER");

                                foreach (Vector2 v2 in _matchesPositions)
                                    GameManager.Instance.GetTestingPrefab(1).transform.position = v2;
                            }

                            GameManager.Instance.GameOver();

                            break;
                        }
                    }
                    else 
                    {
                        matches = 1;

                        if (GameManager.Instance.TestingMode) 
                        {
                            Debug.Log("(x: " + x + " y: " + y + ") Circle for player" + cellCircleID + " CIRCLE MATCH = " + matches);

                            _matchesPositions.Clear();
                            _matchesPositions.Add(GetCellByCoordinates(new Vector2(x, y)));
                        }
                    }
                }
                else 
                {
                    matches = 0;

                    if (GameManager.Instance.TestingMode) 
                    {
                        Debug.Log("(x: " + x + " y: " + y + ") Cell empty " + cellCircleID + " CIRCLE MATCH = " + matches);

                        _matchesPositions.Clear();
                    }
                }

                lastCircleID = cellCircleID;
            }
        }

        if (!GameManager.Instance.IsGameOver) 
        {
            if (GameManager.Instance.TestingMode)
                Debug.Log("NO WINNER FOUND IN COLUMNS.");

            StartCoroutine(CheckSequencesInRows());
        }
    }

    IEnumerator CheckSequencesInRows()
    {
        if (GameManager.Instance.TestingMode) 
        {
            Debug.Log("-------------------");
            Debug.Log("CHECKING ROWS...");
        }

        for (int y = 0; y < _gridMatrix.GetLength(1); y++)
        {
            if (GameManager.Instance.IsGameOver)
                break;

            int matches = 0;

            int cellCircleID;
            int lastCircleID = 0;

            _matchesPositions = new List<Vector2>();

            for (int x = 0; x < _gridMatrix.GetLength(0); x++)
            {
                if (GameManager.Instance.TestingMode) 
                {
                    yield return new WaitForSeconds(0.05f);
                    _cellPin.transform.position = GetCellByCoordinates(new Vector2(x, y));
                    _cellPin.SetActive(true);
                } 

                cellCircleID = _gridMatrix[x, y];

                if (_gridMatrix[x, y] != 0)
                {
                    if ((cellCircleID == lastCircleID) || (_gridMatrix[0, y] != 0 && lastCircleID == 0))
                    {
                        matches++;

                        if (GameManager.Instance.TestingMode) 
                        {
                            Debug.Log("(x: " + x + " y: " + y + ") Circle for player" + cellCircleID + " CIRCLE MATCH = " + matches);

                            _matchesPositions.Add(GetCellByCoordinates(new Vector2(x, y)));
                        }
                            
                        if (matches >= 4)
                        {
                            if (GameManager.Instance.TestingMode) 
                            {
                                Debug.Log("Player" + cellCircleID + " WINS!");
                                Debug.Log("GAME OVER");

                                foreach (Vector2 v2 in _matchesPositions)
                                    GameManager.Instance.GetTestingPrefab(1).transform.position = v2;
                            }

                            GameManager.Instance.GameOver();

                            break;
                        }
                    }
                    else
                    {
                        matches = 1;

                        if (GameManager.Instance.TestingMode) 
                        {
                            Debug.Log("(x: " + x + " y: " + y + ") Circle for player" + cellCircleID + " CIRCLE MATCH = " + matches);

                            _matchesPositions.Clear();
                            _matchesPositions.Add(GetCellByCoordinates(new Vector2(x, y)));
                        }
                    }
                }
                else
                {
                    matches = 0;

                    if (GameManager.Instance.TestingMode) 
                    {
                        Debug.Log("(x: " + x + " y: " + y + ") Cell empty " + cellCircleID + " CIRCLE MATCH = " + matches);

                        _matchesPositions.Clear();
                    }
                }

                lastCircleID = cellCircleID;
            }
        }

        if (!GameManager.Instance.IsGameOver)
        {
            if (GameManager.Instance.TestingMode)
                Debug.Log("NO WINNER FOUND IN ROWS.");

            if (GameManager.Instance.AllowDiagonals)
                StartCoroutine(CheckSequencesInDiagonalsRight());
            else
            {
                if (GameManager.Instance.TestingMode) 
                {
                    Debug.Log("--- END OF CHECK ---");
                    _cellPin.SetActive(false);
                }
            }
        }
    }

    IEnumerator CheckSequencesInDiagonalsRight() 
    {
        if (GameManager.Instance.TestingMode) 
        {
            Debug.Log("-------------------");
            Debug.Log("CHECKING DIAGONALS RIGHT...");
        }

        int rows = _gridMatrix.GetLength(0);
        int columns = _gridMatrix.GetLength(1);

        // number of secondary diagonals
        int d = columns + rows - 1;
        int x, y;

        int cellCircleID;
        int lastCircleID = 0;

        _matchesPositions = new List<Vector2>();

        // go through each diagonal
        for (int i = 0; i < d; i++)
        {
            int matches = 0;

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

            do
            {
                if (GameManager.Instance.IsGameOver)
                    break;

                if (GameManager.Instance.TestingMode) 
                {
                    yield return new WaitForSeconds(0.05f);
                    _cellPin.transform.position = GetCellByCoordinates(new Vector2(x, y));
                    _cellPin.SetActive(true);
                }   

                cellCircleID = _gridMatrix[x, y];

                if (_gridMatrix[x, y] != 0) 
                {                    
                    if ((cellCircleID == lastCircleID) || (_gridMatrix[x, y] != 0 && lastCircleID == 0))
                    {
                        matches++;

                        if (GameManager.Instance.TestingMode) 
                        {
                            Debug.Log("(x: " + x + " y: " + y + ") Circle for player" + cellCircleID + " CIRCLE MATCH = " + matches);

                            _matchesPositions.Add(GetCellByCoordinates(new Vector2(x, y)));
                        }

                        if (matches >= 4)
                        {
                            if (GameManager.Instance.TestingMode) 
                            {
                                Debug.Log("Player" + cellCircleID + " WINS!");
                                Debug.Log("GAME OVER");

                                foreach (Vector2 v2 in _matchesPositions)
                                    GameManager.Instance.GetTestingPrefab(1).transform.position = v2;
                            }

                            GameManager.Instance.GameOver();

                            break;
                        }
                    }
                    else
                    {
                        matches = 1;

                        if (GameManager.Instance.TestingMode) 
                        {
                            Debug.Log("(x: " + x + " y: " + y + ") Circle for player" + cellCircleID + " CIRCLE MATCH = " + matches);

                            _matchesPositions.Clear();
                            _matchesPositions.Add(GetCellByCoordinates(new Vector2(x, y)));
                        } 
                    }
                }
                else 
                {
                    matches = 0;

                    if (GameManager.Instance.TestingMode) 
                    {
                        Debug.Log("(x: " + x + " y: " + y + ") Cell empty " + cellCircleID + " CIRCLE MATCH = " + matches);

                        _matchesPositions.Clear();
                    }
                }

                lastCircleID = cellCircleID;

                x++;
                y--;
            }
            while (x < rows && y >= 0);

            if (GameManager.Instance.IsGameOver)
                break;
        }

        if (!GameManager.Instance.IsGameOver)
        {
            if (GameManager.Instance.TestingMode)
                Debug.Log("NO WINNER FOUND IN DIAGONALS RIGHT.");

            StartCoroutine(CheckSequencesInDiagonalsLeft());
        }
    }

    IEnumerator CheckSequencesInDiagonalsLeft()
    {
        if (GameManager.Instance.TestingMode) 
        {
            Debug.Log("-------------------");
            Debug.Log("CHECKING DIAGONALS LEFT...");
        }

        int rows = _gridMatrix.GetLength(0);
        int columns = _gridMatrix.GetLength(1);

        // number of secondary diagonals
        int d = columns + rows - 1;
        int x, y;

        int columnsAux = 0;

        int cellCircleID;
        int lastCircleID = 0;

        _matchesPositions = new List<Vector2>();

        // go through each diagonal
        for (int i = 0; i < 12; i++)
        {
            int matches = 0;

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

            do
            {
                if (GameManager.Instance.IsGameOver)
                    break;

                if (GameManager.Instance.TestingMode) 
                {
                    yield return new WaitForSeconds(0.05f);
                    _cellPin.transform.position = GetCellByCoordinates(new Vector2(x, y));
                    _cellPin.SetActive(true);
                } 

                cellCircleID = _gridMatrix[x, y];

                if (_gridMatrix[x, y] != 0) 
                {
                    if ((cellCircleID == lastCircleID) || (_gridMatrix[x, y] != 0 && lastCircleID == 0))
                    {
                        matches++;

                        if (GameManager.Instance.TestingMode) 
                        {
                            Debug.Log("(x: " + x + " y: " + y + ") Circle for player" + cellCircleID + " CIRCLE MATCH = " + matches);

                            _matchesPositions.Add(GetCellByCoordinates(new Vector2(x, y)));
                        }

                        if (matches >= 4)
                        {
                            if (GameManager.Instance.TestingMode) 
                            {
                                Debug.Log("Player" + cellCircleID + " WINS!");
                                Debug.Log("GAME OVER");

                                foreach (Vector2 v2 in _matchesPositions)
                                    GameManager.Instance.GetTestingPrefab(1).transform.position = v2;
                            } 

                            GameManager.Instance.GameOver();

                            break;
                        }
                    }
                    else
                    {
                        matches = 1;

                        if (GameManager.Instance.TestingMode) 
                        {
                            Debug.Log("(x: " + x + " y: " + y + ") Circle for player" + cellCircleID + " CIRCLE MATCH = " + matches);

                            _matchesPositions.Clear();
                            _matchesPositions.Add(GetCellByCoordinates(new Vector2(x, y)));
                        }
                    }
                }
                else 
                {
                    matches = 0;

                    if (GameManager.Instance.TestingMode) 
                    {
                        Debug.Log("(x: " + x + " y: " + y + ") Cell empty " + cellCircleID + " CIRCLE MATCH = " + matches);

                        _matchesPositions.Clear();
                    }
                }

                lastCircleID = cellCircleID;

                x--;
                y--;
            }
            while (x >= 0 && y >= 0);

            if (GameManager.Instance.IsGameOver)
                break;
        }

        if (!GameManager.Instance.IsGameOver && GameManager.Instance.TestingMode)
        {
            Debug.Log("NO WINNER FOUND IN DIAGONALS LEFT.");
            Debug.Log("--- END OF CHECK ---");
        }

        _cellPin.SetActive(false);
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

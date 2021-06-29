using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum Players { Player1 = 1, Player2 = 2 };
    public Players ActivePlayer { get; set; }

    public int Player1Matches { get; set; }
    public int Player2Matches { get; set; }

    public bool AllowDiagonals { get => _allowDiagonals; }
    [SerializeField]
    private bool _allowDiagonals = false;

    public bool IsGameOver { get => _isGameOver; }

    private bool _isGameOver = false;

    [SerializeField]
    private GameObject[] _playersCirclePrefabs;

    [SerializeField]
    private GameObject[] _testingCirclePrefabs;

    public Transform testingParent;

    public bool TestingMode { get => _testingMode; }
    [SerializeField]
    private bool _testingMode = false;

    public bool TestingAutoCheck { get => _testingAutoCheck; }
    [SerializeField]
    private bool _testingAutoCheck = false;

    private new void Awake()
    {
        ActivePlayer = Players.Player1;
        UIManager.Instance.SetActivePlayerUI(ActivePlayer);

        UIManager.Instance.ViewTestingGUI = _testingMode;
    }

    /// <summary>
    /// Returns the circle prefab of the current active player.
    /// </summary>
    public GameObject GetActivePlayerPrefab() 
    {
        GameObject playerPrefab = Instantiate(_playersCirclePrefabs[(int)ActivePlayer - 1]);
        playerPrefab.name = ActivePlayer.ToString();
        playerPrefab.tag = ActivePlayer.ToString();

        return playerPrefab;
    }

    /// <summary>
    /// Returns the dot prefab to show during testing (0 for blue dot, 1 for green dot).
    /// </summary>
    public GameObject GetTestingPrefab(int id = 0) 
    {
        if(id >= _testingCirclePrefabs.Length) 
        {
            Debug.LogError("Invalid prefab id");

            return null;
        }
        
        if(id == 0) 
        {
            GameObject testingPrefab = Instantiate(_testingCirclePrefabs[id], testingParent); 
            return testingPrefab;
        }
        else 
        {
            GameObject testingPrefab = Instantiate(_testingCirclePrefabs[id], testingParent.GetChild(0)); 
            return testingPrefab;
        }
            
    }

    /// <summary>
    /// Sets the next player based on the active player at the moment.
    /// </summary>
    public void NextPlayer() 
    {  
        if (ActivePlayer == Players.Player1)
            ActivePlayer = Players.Player2;
        else
            ActivePlayer = Players.Player1;

        UIManager.Instance.SetActivePlayerUI(ActivePlayer);
    }

    /// <summary>
    /// Sets the game as over.
    /// </summary>
    public void GameOver(bool withWinner = true) 
    {
        _isGameOver = true;

        UIManager.Instance.ShowGameOver(withWinner);
    }

    /// <summary>
    /// Reloads the game scene.
    /// </summary>
    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}

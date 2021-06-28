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

    private new void Awake()
    {
        ActivePlayer = Players.Player1;
    }

    public GameObject GetActivePlayerPrefab() 
    {
        GameObject playerPrefab = Instantiate(_playersCirclePrefabs[(int)ActivePlayer - 1]);
        playerPrefab.name = ActivePlayer.ToString();
        playerPrefab.tag = ActivePlayer.ToString();

        return playerPrefab;
    }

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

    public void NextPlayer() 
    {  
        if (ActivePlayer == Players.Player1)
            ActivePlayer = Players.Player2;
        else
            ActivePlayer = Players.Player1;
    }

    public void GameOver(int winnerPlayerID = 0) 
    {
        _isGameOver = true;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(0);
    }
}

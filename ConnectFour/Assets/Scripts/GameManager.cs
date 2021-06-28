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

    public bool IsGameOver { get => _isGameOver; }

    private bool _isGameOver = false;

    [SerializeField]
    private GameObject[] _playersCirclePrefab;

    private void Awake()
    {
        ActivePlayer = Players.Player1;
    }

    private void Update()
    {

    }

    public GameObject GetActivePlayerPrefab() 
    {
        GameObject playerPrefab = Instantiate(_playersCirclePrefab[(int)ActivePlayer - 1]);
        playerPrefab.name = ActivePlayer.ToString();
        playerPrefab.tag = ActivePlayer.ToString();

        return playerPrefab;
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

    public void Reset()
    {
        SceneManager.LoadScene(0);
    }
}

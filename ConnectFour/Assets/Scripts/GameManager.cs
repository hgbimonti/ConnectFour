using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public enum Players { Player1 = 1, Player2 = 2 };

    public Players ActivePlayer { get; set; }

    [SerializeField]
    private GameObject[] _playersCirclePrefab;

    private void Start()
    {
        ActivePlayer = Players.Player1;
    }

    public GameObject GetActivePlayerPrefab() 
    {
        GameObject playerPrefab = Instantiate(_playersCirclePrefab[(int)ActivePlayer - 1]);
        playerPrefab.name = ActivePlayer.ToString();

        return playerPrefab;
    }

    public void NextPlayer() 
    {
        if (ActivePlayer == Players.Player1)
            ActivePlayer = Players.Player2;
        else
            ActivePlayer = Players.Player1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using DG.Tweening;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField]
    private RectTransform _player1Label;

    [SerializeField]
    private RectTransform _player2Label;

    [SerializeField]
    private RectTransform _gameOverLabel;

    [SerializeField]
    private RectTransform _winnerLabel;

    [SerializeField]
    private GameObject _testingGUI;

    public bool ViewTestingGUI { get; set; }
    
    private void Start()
    {
        _testingGUI.SetActive(ViewTestingGUI);

        _gameOverLabel.gameObject.SetActive(false);
        _winnerLabel.gameObject.SetActive(false);
    }

    public void ShowGameOver(bool withWinner = true) 
    {
        _gameOverLabel.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _gameOverLabel.gameObject.SetActive(true);
        _gameOverLabel.DOScale(1.0f, 0.3f).SetEase(Ease.InOutQuint).OnComplete(() => 
        { 
            if (withWinner) 
            {
                _winnerLabel.gameObject.SetActive(true);

                _winnerLabel.GetComponent<TMP_Text>().text = GameManager.Instance.ActivePlayer.ToString() + " Win!";

                _winnerLabel.DOScale(1.2f, 0.5f).SetLoops(-1, LoopType.Yoyo);
            } 
        });
    }

    public void SetActivePlayerUI(GameManager.Players player) 
    {
        if(player == GameManager.Players.Player1) 
        {
            _player1Label.DOScale(1.3f, 0.3f);
            _player2Label.DOScale(1.0f, 0.3f);

            _player1Label.GetComponent<TMP_Text>().DOColor(Color.red, 0.3f);
            _player2Label.GetComponent<TMP_Text>().DOColor(Color.white, 0.3f);
        }
        else 
        {
            _player1Label.DOScale(1.0f, 0.3f);
            _player2Label.DOScale(1.3f, 0.3f);

            _player1Label.GetComponent<TMP_Text>().DOColor(Color.white, 0.3f);
            _player2Label.GetComponent<TMP_Text>().DOColor(Color.yellow, 0.3f);
        }
    }
}

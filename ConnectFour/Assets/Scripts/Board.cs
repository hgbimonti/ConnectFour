using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Grid _grid;

    private GameObject _activeCircle;
    
    private Transform _gridTransform;

    private bool _isDroppingCircle;

    private void Start()
    {
        _gridTransform = _grid.GetComponent<Transform>();
        _activeCircle = GameManager.Instance.GetActivePlayerPrefab();
    }

    private void Update()
    {
        Vector3 mousePositionToGrid = Camera.main.ScreenToWorldPoint(Input.mousePosition) - _gridTransform.position;

        if (_grid.GetNearestCellCoordinates(mousePositionToGrid).x >= 0 
            && _grid.GetNearestCellCoordinates(mousePositionToGrid).x < _grid.GridWidth 
            && _grid.GetNearestCellCoordinates(mousePositionToGrid).y == _grid.GridHeight 
            && !_isDroppingCircle)
        {
            if (_activeCircle == null || GameManager.Instance.IsGameOver)
                return;
            
            if (!_activeCircle.activeSelf)
                _activeCircle.SetActive(true);

            _activeCircle.transform.position = _grid.GetNearestCellOnGrid(mousePositionToGrid);

            if (Input.GetMouseButtonDown(0)) 
            {
                int columnID = (int)_grid.GetNearestCellCoordinates(mousePositionToGrid).x;
                Vector2 availableCellCoordinates = _grid.GetAvailableCellCoordinatesInColumn(columnID);
                
                if(availableCellCoordinates.y != -1000) 
                {
                    _grid.SetCellAsUsedInCoordinates(availableCellCoordinates, GameManager.Instance.ActivePlayer);

                    GameObject fallingCirclePrefab = GameManager.Instance.GetActivePlayerPrefab();
                    fallingCirclePrefab.name += " " + availableCellCoordinates.ToString();
                    fallingCirclePrefab.transform.position = _activeCircle.transform.position;

                    DropCircle(fallingCirclePrefab, _grid.GetCellByCoordinates(availableCellCoordinates).y);

                    _isDroppingCircle = true;
                }
            }
        }
        else
        {
            if (_isDroppingCircle)
                Destroy(_activeCircle);
        }
    }

    private void DropCircle(GameObject circle, float endValue) 
    {
        circle.transform.DOMoveY(endValue, 1.0f).SetEase(Ease.OutBounce).OnComplete(() => 
        { 
            _isDroppingCircle = false;

            _grid.LookForSequence();

            StartCoroutine(WaitAndSetNextPlayer());
        });
    }

    IEnumerator WaitAndSetNextPlayer() 
    {
        yield return new WaitForSeconds(0.2f);

        if (!GameManager.Instance.IsGameOver) 
        {
            GameManager.Instance.NextPlayer();

            _activeCircle = GameManager.Instance.GetActivePlayerPrefab();
        }
    }
}
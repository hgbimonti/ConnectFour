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

    private void Awake()
    {
        _gridTransform = _grid.GetComponent<Transform>();
    }

    private void Start()
    {
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
            if (!_activeCircle.activeSelf)
                _activeCircle.SetActive(true);

            _activeCircle.transform.position = _grid.GetNearestCellOnGrid(mousePositionToGrid);

            if (Input.GetMouseButtonDown(0)) 
            {
                Vector2 availableCellCoordinates = _grid.GetAvailableCellCoordinatesInColumn((int)_grid.GetNearestCellCoordinates(mousePositionToGrid).x);
                
                if(availableCellCoordinates.y != -1000) 
                {
                    _grid.SetCellAsUsedInCoordinates(availableCellCoordinates, 1);

                    DropCircle(_grid.GetCellByCoordinates(availableCellCoordinates).y);

                    _isDroppingCircle = true;
                }
            }
        }
        else
        {
            if (_activeCircle.activeSelf)
                _activeCircle.SetActive(false);
        }
    }

    private void DropCircle(float endValue) 
    {
        GameObject circle = Instantiate(_activeCircle, _activeCircle.transform.position, Quaternion.identity);
        
        circle.transform.DOMoveY(endValue, 1.0f).SetEase(Ease.OutBounce).OnComplete(() => 
        { 
            _isDroppingCircle = false; 
            
            GameManager.Instance.NextPlayer();

            _activeCircle = GameManager.Instance.GetActivePlayerPrefab();
        });
    }
}
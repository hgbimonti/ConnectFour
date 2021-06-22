using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class Board : MonoBehaviour
{
    [SerializeField]
    private Grid _grid;

    [SerializeField]
    private GameObject _redCirclePrefab;
    [SerializeField]
    private GameObject _yellowCirclePrefab;
    private GameObject _activeCircle;
    
    private Transform _gridTransform;

    private bool _isDroppingCircle;

    private void Awake()
    {
        _gridTransform = _grid.GetComponent<Transform>();

        _activeCircle = Instantiate(_redCirclePrefab);
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
                DropCircle(_grid.GetCellByCoordinates(new Vector2(_grid.GetNearestCellCoordinates(mousePositionToGrid).x, 0)).y);
                _isDroppingCircle = true;
            }
        }
        else
        {
            if(_activeCircle.activeSelf)
                _activeCircle.SetActive(false);
        }
    }

    private void DropCircle(float endValue) 
    {
        Debug.Log(endValue);
        
        GameObject circle = Instantiate(_redCirclePrefab, _activeCircle.transform.position, Quaternion.identity);
        
        circle.transform.DOMoveY(endValue, 1.0f).SetEase(Ease.OutBounce).OnComplete(() => { _isDroppingCircle = false; });
    }
}

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
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (_grid.GetNearestCellCoordinates(mousePosition - _gridTransform.position).x >= 0 
            && _grid.GetNearestCellCoordinates(mousePosition - _gridTransform.position).x < _grid.GridWidth 
            && _grid.GetNearestCellCoordinates(mousePosition - _gridTransform.position).y == _grid.GridHeight
            && !_isDroppingCircle)
        {
            if (!_activeCircle.activeSelf)
                _activeCircle.SetActive(true);

            _activeCircle.transform.position = _grid.GetNearestCellOnGrid(mousePosition - _gridTransform.position);

            if (Input.GetMouseButtonDown(0)) 
            {
                DropCircle(_activeCircle, _grid.GetCellByCoordinates(new Vector3(1, 0, 0)));
                _isDroppingCircle = true;
            }
        }
        else
        {
            if(_activeCircle.activeSelf)
                _activeCircle.SetActive(false);
        }
    }

    private void DropCircle(GameObject circle, Vector3 endPosition) 
    {
        
    }
}

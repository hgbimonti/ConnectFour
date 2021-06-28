using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField]
    private GameObject _testingGUI;

    public bool ViewTestingGUI { get; set; }
    
    private void Start()
    {
        _testingGUI.SetActive(ViewTestingGUI);
    }
}

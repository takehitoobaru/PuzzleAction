using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveUp : MonoBehaviour
{
    Button _button;
    public bool _clickButton = false;

    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(GiveUP);
    }

    void GiveUP()
    {
        _clickButton = true;
    }

}

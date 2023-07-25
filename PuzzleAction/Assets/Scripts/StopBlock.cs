using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StopBlock : MonoBehaviour
{
    [SerializeField]
    float _playerStopTime = 2f; //ストップする時間

    Player _player; //コンポーネント

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag != "Player") return;
        _player = col.GetComponent<Player>();
        _player._stopTime = _playerStopTime;
        _player._isStop = true;
    }
}

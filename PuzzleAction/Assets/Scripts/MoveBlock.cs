using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlock : MonoBehaviour
{
    float _minPos = 0f; //初期位値

    [SerializeField]
    float _maxPos = 0f; //この位置まで来たら折り返す

    [SerializeField]
    bool _isMoveSide = true;    //横に動くかどうか

    [SerializeField]
    float _moveSpeed = 1f;  //移動速度

    [SerializeField]
    float _waitTime = 2f;   //待ち時間

    Vector2 _moveDir = Vector2.right;   //動く向き
    Rigidbody2D _rb;    //RigidBody2Dのコンポーネント

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        //横に動く時
        if (_isMoveSide)
        {
            _moveDir = Vector2.right;
            _minPos = transform.position.x;
            _maxPos = transform.position.x + _maxPos;
        }
        //縦に動くとき
        else if (!_isMoveSide)
        {
            _moveDir = Vector2.up;
            _minPos = transform.position.y;
            _maxPos = transform.position.y + _maxPos;
        }

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        while (true)
        {
            //横に動くとき
            if (_isMoveSide)
            {
                _rb.velocity = _moveDir * _moveSpeed;
                //最大値に達したら
                if(transform.position.x >= _maxPos && _moveDir.x > 0)
                {
                    _rb.velocity = Vector2.zero;
                    yield return new WaitForSeconds(_waitTime);
                    _moveDir = -_moveDir;
                }

                //最小値に達したら
                if(transform.position.x <= _minPos && _moveDir.x < 0)
                {
                    _rb.velocity = Vector2.zero;
                    yield return new WaitForSeconds(_waitTime);
                    _moveDir = -_moveDir;
                }
            }
            //縦に動くとき
            else if (!_isMoveSide)
            {
                _rb.velocity = _moveDir * _moveSpeed;
                //最大値に達したら
                if (transform.position.y >= _maxPos && _moveDir.y > 0)
                {
                    _rb.velocity = Vector2.zero;
                    yield return new WaitForSeconds(_waitTime);
                    _moveDir = -_moveDir;
                }

                //最小値に達したら
                if (transform.position.y <= _minPos && _moveDir.y < 0)
                {
                    _rb.velocity = Vector2.zero;
                    yield return new WaitForSeconds(_waitTime);
                    _moveDir = -_moveDir;
                }
            }
            yield return null;
        }
    }
}

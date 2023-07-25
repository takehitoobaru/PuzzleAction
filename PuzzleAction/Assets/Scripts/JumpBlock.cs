using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBlock : MonoBehaviour
{
    [SerializeField]
    float _jumpHeight = 6f; //ジャンプの高さ

    [SerializeField]
    float _jumpAngle = 45f;   //ジャンプの角度

    Player _player;
    Rigidbody2D _playerRigidBody;

    float rad;  //ラジアン
    float _jumpTime = 1f;   //時間
    float _gravity = 9.81f; //重力加速度
    float _jumpSpeed = 0f; //ジャンプの初速度
    Vector2 _jumpDir;   //ジャンプのベクトル
    string _playerTag = "Player";   //プレイヤーのタグ
    bool _onPlayer = false; //プレイヤーが触れたかどうか

    void Start()
    {
        //変位y　初速度v0　角度θ　時間t　重力加速度g
        //y = v0 * sinθ * t - (1/2) * g * t * t
        //v0 = (y + (1/2) * g * t * t) / (sinθ * t)
        //θ = arcsin((y + (1/2) * g * t * t) / (v0 * t))

        //角度をラジアンに変換
        rad = _jumpAngle * Mathf.Deg2Rad;

        //初速度を計算
        _jumpSpeed = (_jumpHeight + 0.5f * _gravity * _jumpTime * _jumpTime) / Mathf.Sin(rad) * _jumpTime;

        //ジャンプのベクトル
        _jumpDir = new Vector2(Mathf.Cos(rad),Mathf.Sin(rad));
    }

    void FixedUpdate()
    {
        if (_onPlayer)
        {
            _player._isJump = true;
            _playerRigidBody.velocity = Vector2.zero;
            _playerRigidBody.AddForce(_jumpDir * _jumpSpeed, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == _playerTag)
        {
            _player = col.GetComponent<Player>();
            _playerRigidBody = col.GetComponent<Rigidbody2D>();
            _onPlayer = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        _onPlayer = false;
    }

}

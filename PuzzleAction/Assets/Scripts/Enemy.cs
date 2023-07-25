using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float _moveSpeed = 5f; //進むスピード

    [SerializeField]
    float _fallSpeed = 15f; //落ちるスピード

    bool _isGround = false; //接地判定
    bool _isRoad = false;   //道があるかどうか
    float _colHalfSizeX = 0f;   //コライダーのxサイズの半分
    float _colHalfSizeY = 0f;   //コライダーのyサイズの半分
    Vector2 _moveDir = Vector2.right;   //進む向き
    Vector2 _prevPos;   //前の位置
    string _wallTag = "Wall";   //壁のタグ
    Rigidbody2D _rb;
    CapsuleCollider2D _collider;
    GroundCheck _groundCheck;
    RoadCheck _roadCheck;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CapsuleCollider2D>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _roadCheck = GetComponentInChildren<RoadCheck>();

        _colHalfSizeX = _collider.size.x / 2;
        _colHalfSizeY = _collider.size.y / 2;

        _prevPos = transform.position;
    }

    void Update()
    {
        //前の位置をfalseに
        GameManager.Instance.CellFalse(_prevPos.x, _prevPos.y);
        GameManager.Instance.CellFalse(_prevPos.x + _colHalfSizeX, _prevPos.y + _colHalfSizeY);
        GameManager.Instance.CellFalse(_prevPos.x - _colHalfSizeX, _prevPos.y - _colHalfSizeY);

        Vector2 pos = transform.position;
        _prevPos = pos;

        //現在の位置をtrueに
        GameManager.Instance.CellTrue(pos.x, pos.y);
        GameManager.Instance.CellTrue(pos.x + _colHalfSizeX, pos.y + _collider.offset.y + _colHalfSizeY);
        GameManager.Instance.CellTrue(pos.x - _colHalfSizeX, pos.y + _collider.offset.y - _colHalfSizeY);

    }

    void FixedUpdate()
    {
        _isGround = _groundCheck.IsGround();
        _isRoad = _roadCheck.IsRoad();

        //道がなければ
        if (!_isRoad)
        {
            Flip();
        }

        //接地中なら
        if (_isGround)
        {
            _rb.velocity = new Vector2(_moveDir.x * _moveSpeed, _rb.velocity.y);
        }
        //接地中でないなら
        else
        {
            _rb.velocity = Vector2.down * _fallSpeed;
        }

    }

    /// <summary>
    /// 反転
    /// </summary>
    void Flip()
    {
        _moveDir = -_moveDir;
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //壁なら
        if(col.tag == _wallTag)
        {
            Flip();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    bool _isGround = false;
    bool _isGroundEnter = false;
    bool _isGroundStay = false;
    bool _isGroundExit = false;

    bool _isMoveGround = false;
    bool _isMoveGroundEnter = false;
    bool _isMoveGroundStay = false;
    bool _isMoveGroundExit = false;

    string _blockTag = "Block"; //ブロックのタグ
    string _moveBlockTag = "MoveBlock"; //動くブロックのタグ

    [System.NonSerialized]
    public Transform _moveGroundTransform; //動く足場のTransform

    /// <summary>
    /// 接地判定を返す
    /// </summary>
    public bool IsGround()
    {
        if(_isGroundEnter || _isGroundStay)
        {
            _isGround = true;
        }
        else if (_isGroundExit)
        {
            _isGround = false;
        }

        _isGroundEnter = false;
        _isGroundStay = false;
        _isGroundExit = false;

        return _isGround;
    }

    /// <summary>
    /// 動く足場の判定を返す
    /// </summary>
    public bool IsMoveGround()
    {
        if (_isMoveGroundEnter || _isMoveGroundStay)
        {
            _isMoveGround = true;
        }
        else if (_isMoveGroundExit)
        {
            _isMoveGround = false;
        }

        _isMoveGroundEnter = false;
        _isMoveGroundStay = false;
        _isMoveGroundExit = false;

        return _isMoveGround;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == _blockTag)
        {
            _isGroundEnter = true;
        }

        if(col.tag == _moveBlockTag)
        {
            _isGroundEnter = true;
            _isMoveGroundEnter = true;
            _moveGroundTransform = col.transform;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == _blockTag)
        {
            _isGroundStay = true;
        }

        if (col.tag == _moveBlockTag)
        {
            _isGroundStay = true;
            _isMoveGroundStay = true;
            _moveGroundTransform = col.transform;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == _blockTag)
        {
            _isGroundExit = true;
        }

        if (col.tag == _moveBlockTag)
        {
            _isGroundExit = true;
            _isMoveGroundExit = true;
            _moveGroundTransform = null;
        }
    }
}

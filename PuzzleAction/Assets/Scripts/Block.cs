using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [System.NonSerialized]
    public Vector2 _cellPos;    //セルポジション

    Vector2 _prevPos;

    float _colHalfSizeX = 0;    //コライダーのxサイズの半分
    float _colHalfSizeY = 0;    //コライダーのyサイズの半分
    BoxCollider2D _collider;    //BoxCollider2Dのコンポーネント

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
        _colHalfSizeX = _collider.size.x / 2 - 0.01f;
        _colHalfSizeY = _collider.size.y / 2 - 0.01f;
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

        _cellPos = GameManager.Instance.GetCellPos(pos.x, pos.y);

        //現在の位置をtrueに
        GameManager.Instance.CellTrue(pos.x, pos.y);
        GameManager.Instance.CellTrue(pos.x + _colHalfSizeX, pos.y + _colHalfSizeY);
        GameManager.Instance.CellTrue(pos.x - _colHalfSizeX, pos.y - _colHalfSizeY);
    }
}

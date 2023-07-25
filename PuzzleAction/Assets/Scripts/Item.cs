using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField]
    int _itemId = 0;    //アイテムid

    [SerializeField]
    Sprite[] _sprite;   //スプライトの配列

    string _playerTag = "Player";
    ItemSet _itemSet;
    SpriteRenderer _spriteRenderer;

    void Start()
    {
        _itemSet = GameObject.FindWithTag("ItemSet").GetComponent<ItemSet>();
        _spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = _sprite[_itemId];
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //プレイヤーなら
        if (col.tag == _playerTag)
        {
            //所持数プラス
            _itemSet._itemCount[_itemId] += 1;
            Destroy(gameObject);
        }
    }
}

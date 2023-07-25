using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSet : MonoBehaviour
{

    [SerializeField]
    GameObject[] _itemPrefab;   //アイテムの配列

    [SerializeField]
    Text _itemText; //アイテムのテキスト

    public List<int> _itemCount = new List<int>();    //アイテムの所持数
    List<string> _name = new List<string> { "タイマー", "右ジャンプ", "左ジャンプ", "ストップ", "岩石", "豆の木2", "豆の木3", "梯子" }; //名前のリスト
    int _startItemtCount = 3;    //スタート時点で持っている種類
    int _startItemNum = 5;  //スタート時点で持っている数
    int _selectNum = 0; //選んでいる番号

    Vector2 _mousePos;  //マウスのポジション
    Vector2 _mouseCellPos;  //マウスのセルポジション
    Vector2 _newPos;    //セルポジションをワールドポジションに
    string _playerTag = "Player";   //プレイヤーのタグ
    Player _player; //コンポーネント

    void Start()
    {
        _player = GameObject.FindWithTag(_playerTag).GetComponent<Player>();

        //アイテムの所持数
        for (int i = 0; i < _itemPrefab.Length; i++)
        {
            if (i < _startItemtCount)
            {
                _itemCount.Add(_startItemNum);
            }
            else
            {
                _itemCount.Add(0);
            }
        }

        _itemText.text = $"{_name[_selectNum]}:{_itemCount[_selectNum]}個";
    }

    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;

        if(scroll != 0)
        {
            //入力に応じて増減させる
            _selectNum += scroll > 0 ? 1 : -1;

            //範囲制限
            _selectNum = Mathf.Clamp(_selectNum,0,_itemPrefab.Length - 1);

        }

        _itemText.text = $"{_name[_selectNum]}:{_itemCount[_selectNum]}個";

        //右クリック
        if (Input.GetMouseButtonDown(1))
        {
            GetMousePose();
            //アイテムが０個じゃなければ
            if (_itemCount[_selectNum] > 0  && !GameManager.Instance._cells[(int)_mouseCellPos.x][(int)_mouseCellPos.y])
            {
                SetItem();
                //アイテム減少
            }
        }

    }

    /// <summary>
    /// マウスのポジションをワールド座標に変換、セルポジションを取得
    /// </summary>
    void GetMousePose()
    {
        //マウスポジション取得
        Vector3 mousePos = Input.mousePosition;
        //ワールド座標に変換
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0;
        _mousePos = worldPos;
        //セルポジション取得
        _mouseCellPos = GameManager.Instance.GetCellPos(_mousePos.x, _mousePos.y);
        //ワールドポジションに
        _newPos = new Vector2(_mouseCellPos.x * GameManager.Instance._cellSize + 1, _mouseCellPos.y * GameManager.Instance._cellSize + 1);
    }

    void SetItem()
    {
        //条件付け不足
        //要改善
        switch (_selectNum)
        {
            case 0:
                //タイマー
                    //選択しているアイテムを設置
                    Instantiate(_itemPrefab[_selectNum], _newPos, Quaternion.identity);
                    //アイテム減少
                    _itemCount[_selectNum]--;
                
                break; 
            case 1:
                //右ジャンプ
                    //選択しているアイテムを設置
                    Instantiate(_itemPrefab[_selectNum], _newPos, Quaternion.identity);
                    //アイテム減少
                    _itemCount[_selectNum]--;
                break;
            case 2:
                //左ジャンプ
                //クリック位置に何もないなら
                    //選択しているアイテムを設置
                    Instantiate(_itemPrefab[_selectNum], _newPos, Quaternion.identity);
                    //アイテム減少
                    _itemCount[_selectNum]--;
                break;
            case 3:
                //ストップ
                //クリック位置に何もないなら
                    //選択しているアイテムを設置
                    Instantiate(_itemPrefab[_selectNum], _newPos, Quaternion.identity);
                    //アイテム減少
                    _itemCount[_selectNum]--;
                break;
            case 4:
                //岩石
                //クリック位置に何もないなら
                    //選択しているアイテムを設置
                    Instantiate(_itemPrefab[_selectNum], _newPos, Quaternion.identity);
                    //アイテム減少
                    _itemCount[_selectNum]--;
                break;
            case 5:
                //豆2
                //クリック位置に何もないなら
                    //選択しているアイテムを設置
                    Instantiate(_itemPrefab[_selectNum], _newPos, Quaternion.identity);
                    //アイテム減少
                    _itemCount[_selectNum]--;
                break;
            case 6:
                //豆3
                //クリック位置に何もないなら
                    //選択しているアイテムを設置
                    Instantiate(_itemPrefab[_selectNum], _newPos, Quaternion.identity);
                    //アイテム減少
                    _itemCount[_selectNum]--;
                break;
            case 7:
                //梯子
                //クリック位置に何もない
                    //選択しているアイテムを設置
                    Instantiate(_itemPrefab[_selectNum], _newPos, Quaternion.identity);
                    //アイテム減少
                    _itemCount[_selectNum]--;
                break;
        }
    }
}

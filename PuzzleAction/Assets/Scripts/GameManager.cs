using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }   //シングルトン用

    public List<List<bool>> _cells = new List<List<bool>>();   //セルポジションに何かあるか
    public float _cellSize = 2f;   //セルのサイズ
    const int COLUMNCOUNT = 62; //列数
    const int ROWCOUNT = 16;    //行数
    int _hitPoint = 5;    //ヒットポイント
    string _playerTag = "Player";
    string _buttonTag = "Button";
    bool _findInit = false;
    Player _player;
    GiveUp _giveup;
    Text _timeText;
    Text _hpText;

    [SerializeField]
    float _timeLimit = 300f;    //制限時間

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
       for(int i = 0; i < COLUMNCOUNT; i++)
        {
            _cells.Add(new List<bool>());
            for(int k = 0; k < ROWCOUNT; k++)
            {
                _cells[i].Add(false);
            }
        }

    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Main") return;

        //メインシーンで1度だけ行う
        if (!_findInit)
        {
            _player = GameObject.FindWithTag(_playerTag).GetComponent<Player>();
            _giveup = GameObject.FindWithTag(_buttonTag).GetComponent<GiveUp>();
            _timeText = GameObject.Find("TimeLimit").GetComponent<Text>();
            _hpText = GameObject.Find("HP").GetComponent<Text>();
            _findInit = true;
        }

        //時間経過
        _timeLimit -= Time.deltaTime;

        //制限時間が来たら
        if(_timeLimit <= 0)
        {
            ChangeScene();
        }


        if (_player != null)
        {
            //ダメージを受けているなら
            if (_player._isDamage)
            {
                ChangeScene();
                _player._isDamage = false;
            }

            if (_player._isGoal)
            {
                ChangeScene();
            }
        }

        if(_giveup != null)
        {
            if (_giveup._clickButton)
            {
                ChangeScene();
            }
        }

        _timeText.text = $"残り時間:{Mathf.Floor(_timeLimit)}";
        _hpText.text = $"HP:{_hitPoint}";
    }

    /// <summary>
    /// hpを減らし、シーンチェンジ
    /// </summary>
    void ChangeScene()
    {
        _hitPoint--;

        if (_hitPoint > 0 && !_player._isGoal)
        {
            SceneManager.LoadScene("Main");
        }
        else if(_hitPoint > 0 && _player._isGoal)
        {
            SceneManager.LoadScene("GameClear");
        }
        else
        {
            SceneManager.LoadScene("GameOver");
            _hitPoint = 5;
        }
        _timeLimit = 300f;
        _findInit = false;
    }

    /// <summary>
    /// ポジションを１マスのサイズで割り、その番号をtrueに
    /// </summary>
    /// <param name="x">ポジションx</param>
    /// <param name="y">ポジションy</param>
    public void CellTrue(float x, float y)
    {
        int column = (int)Math.Floor(x / _cellSize);
        int row = (int)Math.Floor(y / _cellSize);
        //範囲外だったら以下の処理は行わない
        if (row >= ROWCOUNT || row < 0 || column >= COLUMNCOUNT || column < 0) return;
        _cells[column][row] = true;
    }

    /// <summary>
    /// ポジションを１マスのサイズで割り、その番号をfalseに
    /// </summary>
    /// <param name="x">ポジションx</param>
    /// <param name="y">ポジションy</param>
    public void CellFalse(float x, float y)
    {
        int column = (int)Math.Floor(x / _cellSize);
        int row = (int)Math.Floor(y / _cellSize);
        //範囲外だったら以下の処理は行わない
        if (row >= ROWCOUNT || row < 0 || column >= COLUMNCOUNT || column < 0) return;
        _cells[column][row] = false;
    }

    /// <summary>
    /// ワールドポジションを変換
    /// </summary>
    /// <param name="x">ポジションx</param>
    /// <param name="y">ポジションy</param>
    public Vector2 GetCellPos(float x, float y)
    {
        int column = (int)Math.Floor(x / _cellSize);
        int row = (int)Math.Floor(y / _cellSize);
        return new Vector2(column,row);
    }
}

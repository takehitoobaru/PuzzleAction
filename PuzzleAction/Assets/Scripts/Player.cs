using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public float _moveSpeed = 10f;  //スピード

    [SerializeField]
    float _fallSpeed = 15f; //落下速度

    [SerializeField]
    float _deceleration = 0.5f; //x方向の減速率


    [System.NonSerialized]
    public float _stopTime = 0;    //停止時間

    Vector2 _prevPos;   //前の位置
    public Vector2 _moveDir = Vector2.right;   //進む方向
    public bool _isGround = false; //接地判定
    bool _isMoveGround = false; //動く足場の判定
    public bool _isJump = true;   //ジャンプフラグ
    public bool _isStop = false;    //ストップフラグ
    public bool _isClimb = false;   //梯子登り
    public bool _isDescend = false; //梯子下り
    public bool _isTreeClimb = false;   //豆の木初登り
    public bool _isDamage = false;  //ダメージを受けたか
    public bool _isGoal = false;    //ゴールした
    string _wallTag = "Wall";   //壁のタグ
    string _ladderTopTag = "LadderTop"; //梯子上部のタグ
    string _ladderBottomTag = "LadderBottom";   //梯子下部のタグ
    string _treeTopTag = "TreeTop"; //豆の木上部のタグ
    string _treeBottomTag = "TreeBottom";   //豆の木下部のタグ
    string _treeTag = "Tree";   //豆の木タグ
    string _enemyTag = "Enemy"; //敵のタグ
    float _colHalfSizeX = 0f;   //コライダーのxサイズの半分
    float _colHalfSizeY = 0f;   //コライダーのyサイズの半分
    Rigidbody2D _rb;    //Rigidbody2Dのコンポーネント
    GroundCheck _groundCheck;   //GroundCheckのコンポーネント
    CapsuleCollider2D _collider;    //CapsuleCollider2Dのコンポーネント
    Transform _treeTransform;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponentInChildren<GroundCheck>();
        _collider = GetComponent<CapsuleCollider2D>();

        _colHalfSizeX = _collider.size.x / 2 - 0.01f;
        _colHalfSizeY = _collider.size.y / 2 - 0.01f;

        _prevPos = transform.position;

        StartCoroutine(Run());
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

        if(pos.y < 0)
        {
            _isDamage = true;
        }
    }

    void FixedUpdate()
    {
        _isGround = _groundCheck.IsGround();
        _isMoveGround = _groundCheck.IsMoveGround();

        //地面についたらジャンプ判定を切る
        if (_isGround || _isMoveGround)
        {
            _isJump = false;
        }

        //梯子を下りるフラグが切れないことがあったため
        if(_isGround && _isDescend)
        {
            _isDescend = false;
        }

        //梯子では重力を無視する
        if(_isClimb || _isDescend)
        {
            _rb.gravityScale = 0;
        }
        else
        {
            _rb.gravityScale = 1;
        }

        //移動床の子供にする
        if (_isMoveGround)
        {
            transform.SetParent(_groundCheck._moveGroundTransform);
        }
        else
        {
            transform.SetParent(null);
        }

        //豆の木に追従
        if (_isTreeClimb)
        {
            _isGround = false;
            transform.position = _treeTransform.position;
        }

        Fall();

        //ジャンプ中にx方向減速
        //if (_isJump)
        //{
        //    _rb.AddForce(new Vector2(-_rb.velocity.x * _deceleration, 0), ForceMode2D.Force);
        //}
    }

    IEnumerator Run()
    {
        while (true)
        {
            //２秒間ストップ
            if(_isStop)
            {
                _rb.velocity = new Vector2(0, _rb.velocity.y);
                yield return new WaitForSeconds(_stopTime);
                _isStop = false;
            }

            //接地中かつ梯子昇降中でないなら
            if ((_isGround || _isMoveGround) && !_isClimb && !_isDescend)
            {
                _rb.velocity = new Vector2(_moveDir.x * _moveSpeed, _rb.velocity.y);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 梯子を上る
    /// </summary>
    void Climb(Vector2 pos)
    {
        _isClimb = true;
        _isGround = false;
        transform.position = new Vector2(pos.x,transform.position.y);
        _rb.velocity = Vector2.up * _moveSpeed;
    }

    /// <summary>
    /// 昇り終わるときの処理
    /// </summary>
    void ClimbEnd(Vector2 cellPos)
    {
        //プレイヤーが右向きなら
        if (_moveDir.x > 0)
        {
            //右側にブロックがある
            if (GameManager.Instance._cells[(int)cellPos.x + 1][(int)cellPos.y])
            {
                //右のブロックの上に
                transform.position = new Vector2((cellPos.x + 1) * GameManager.Instance._cellSize, (cellPos.y + 2) * GameManager.Instance._cellSize);
            }
            //左側にブロックがある
            else if (GameManager.Instance._cells[(int)cellPos.x - 1][(int)cellPos.y])
            {
                //左のブロックの上に
                transform.position = new Vector2((cellPos.x - 1) * GameManager.Instance._cellSize, (cellPos.y + 2) * GameManager.Instance._cellSize);
                Flip();
            }
            //左右にブロックがない
            else
            {
                //右に
                transform.position = new Vector2((cellPos.x + 1) * GameManager.Instance._cellSize, cellPos.y * GameManager.Instance._cellSize);
            }
        }
        //プレイヤーが左向きなら
        else if (_moveDir.x < 0)
        {
            //左側にブロックがある
            if (GameManager.Instance._cells[(int)cellPos.x - 1][(int)cellPos.y])
            {
                //左のブロックの上に
                transform.position = new Vector2((cellPos.x - 1) * GameManager.Instance._cellSize, (cellPos.y + 2) * GameManager.Instance._cellSize);
            }
            //右側にブロックがある
            else if (GameManager.Instance._cells[(int)cellPos.x + 1][(int)cellPos.y])
            {
                //右のブロックの上に
                transform.position = new Vector2((cellPos.x + 1) * GameManager.Instance._cellSize, (cellPos.y + 2) * GameManager.Instance._cellSize);
                Flip();
            }
            //左右にブロックがない
            else
            {
                //左に
                transform.position = new Vector2((cellPos.x - 1) * GameManager.Instance._cellSize, cellPos.y * GameManager.Instance._cellSize);
            }
        }
        _isClimb = false;
        _isTreeClimb = false;
    }

    /// <summary>
    /// 梯子を降りる
    /// </summary>
    void Descend(Vector2 pos)
    {
        _isDescend = true;
        _isGround = false;
        transform.position = new Vector2(pos.x, transform.position.y);
        _rb.velocity = Vector2.down * _moveSpeed;
    }

    /// <summary>
    /// 降り終わるときの処理
    /// </summary>
    void DescendEnd(Vector2 cellPos)
    {
        //プレイヤーが右向きなら
        if (_moveDir.x > 0)
        {
            //下にブロックがある
            if(GameManager.Instance._cells[(int)cellPos.x][(int)cellPos.y - 1])
            {
            }
            //右側にブロックがある
            else if (GameManager.Instance._cells[(int)cellPos.x + 1][(int)cellPos.y -1])
            {
                //右のブロックの上に
                transform.position = new Vector2((cellPos.x + 1) * GameManager.Instance._cellSize, (cellPos.y + 1) * GameManager.Instance._cellSize);
            }
            //左側にブロックがある
            else if (GameManager.Instance._cells[(int)cellPos.x - 1][(int)cellPos.y - 1])
            {
                //左のブロックの上に
                transform.position = new Vector2((cellPos.x - 1) * GameManager.Instance._cellSize, (cellPos.y + 1) * GameManager.Instance._cellSize);
                Flip();
            }
            
        }
        //プレイヤーが左向きなら
        else if (_moveDir.x < 0)
        {
            //下にブロックがある
            if (GameManager.Instance._cells[(int)cellPos.x][(int)cellPos.y - 1])
            {
            }
            //左側にブロックがある
            else if (GameManager.Instance._cells[(int)cellPos.x - 1][(int)cellPos.y - 1])
            {
                //左のブロックの上に
                transform.position = new Vector2((cellPos.x - 1) * GameManager.Instance._cellSize, (cellPos.y) * GameManager.Instance._cellSize);
            }
            //右側にブロックがある
            else if (GameManager.Instance._cells[(int)cellPos.x + 1][(int)cellPos.y - 1])
            {
                //右のブロックの上に
                transform.position = new Vector2((cellPos.x + 1) * GameManager.Instance._cellSize, (cellPos.y) * GameManager.Instance._cellSize);
                Flip();
            }
        }
        _isDescend = false;
    }

    /// <summary>
    /// 接地中、ジャンプ中、梯子昇降中でないなら落ちる
    /// </summary>
    void Fall()
    {
        //接地中、ジャンプ中、梯子昇降中でないなら
        if (!_isGround && !_isJump && !_isClimb && !_isDescend)
        {
            _rb.velocity = Vector2.down * _fallSpeed;
        }
    }

    /// <summary>
    /// 向きを変える
    /// </summary>
    public void Flip()
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
        //梯子上部なら
        else if(col.tag == _ladderTopTag)
        {
            Vector2 ladderPos = col.transform.position;
            //昇っているなら
            if (_isClimb)
            {
                Vector2 ladderCellPos = GameManager.Instance.GetCellPos(ladderPos.x, ladderPos.y);
                ClimbEnd(ladderCellPos);
            }
            //昇ってないなら
            else if(!_isClimb)
            {
                Descend(ladderPos);
            }
        }
        //梯子下部なら
        else if(col.tag == _ladderBottomTag)
        {
            Vector2 ladderPos = col.transform.position;
            //降りているなら
            if (_isDescend)
            {
                _isDescend = false;
                Vector2 ladderCellPos = GameManager.Instance.GetCellPos(ladderPos.x, ladderPos.y);
                DescendEnd(ladderCellPos);
            }
            //降りてないかつ接地中
            else if (!_isDescend && _isGround)
            {
                Climb(ladderPos);
            }
        }
        //豆の木上部なら
        else if( col.tag == _treeTopTag)
        {
            Beanstalk beanstalk = col.GetComponent<Beanstalk>();
            _treeTransform = col.transform;
            //成長中でないかつ成長が終わってない
            if(!beanstalk._isGrow && !beanstalk._isGrowEnd)
            { 
                transform.position = new Vector2(transform.position.x + _moveDir.x, transform.position.y);
                beanstalk._isGrow = true;
                _isTreeClimb = true;
            }
            //成長中でなく、成長が終わっているかつ昇っている
            else if(!beanstalk._isGrow && beanstalk._isGrowEnd && _isClimb)
            {
                Vector2 treePos = col.transform.position;
                Vector2 treeCellPos = GameManager.Instance.GetCellPos(treePos.x, treePos.y);
                ClimbEnd(treeCellPos);
            }
            //成長中でなく、成長が終わっているかつ昇っていない
            else if (!beanstalk._isGrow && beanstalk._isGrowEnd && !_isClimb)
            {
                transform.position = new Vector2(transform.position.x + 1.5f * _moveDir.x, transform.position.y);
            }
        }
        //豆の木下部なら
        else if(col.tag == _treeBottomTag)
        {
            Beanstalk beanstalk = col.GetComponent<Beanstalk>();
            Vector2 treePos = col.transform.position;
            //成長中でないかつ成長が終わってない
            if (!beanstalk._isGrow && !beanstalk._isGrowEnd)
            {
                beanstalk._isGrow = true;
            }
            //成長中でなく、成長が終わっているかつ自動昇降中でない
            else if (!beanstalk._isGrow && beanstalk._isGrowEnd && !_isTreeClimb)
            {
                Climb(treePos);
            }
        }
        //豆の木なら
        else if(col.tag == _treeTag)
        {
            Beanstalk beanstalk = col.GetComponent<Beanstalk>();
            //成長中でないかつ成長が終わってない
            if (!beanstalk._isGrow && !beanstalk._isGrowEnd)
            {
                beanstalk._isGrow = true;
            }
        }
        //ゴールなら
        else if(col.tag == "Goal")
        {
            _isGoal = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        //豆の木上部なら
        if(col.tag == _treeTopTag)
        {
            Beanstalk beanstalk = col.GetComponent<Beanstalk>();
            //登っているかつ成長が終わった
            if (_isTreeClimb && beanstalk._isGrowEnd)
            {
                _isTreeClimb = false;
                Vector2 treePos = col.transform.position;
                Vector2 treeCellPos = GameManager.Instance.GetCellPos(treePos.x, treePos.y);
                ClimbEnd(treeCellPos);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == _ladderTopTag)
        {
            _isClimb = false;
        }
        else if( col.tag == _ladderBottomTag)
        {
            _isDescend = false;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        //敵なら
        if(col.gameObject.tag == _enemyTag)
        {
            _isDamage = true;
        }
    }

}

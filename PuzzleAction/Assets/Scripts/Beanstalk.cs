using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Beanstalk : MonoBehaviour
{
    [SerializeField]
    float _maxHeight = 8f;  //最大

    [SerializeField]
    float _growSpeed = 1f;  //成長速度

    public bool _isGrow = false; //成長するかどうか
    public bool _isGrowEnd = false; //成長が終わったかどうか

    void Start()
    {
        _maxHeight = transform.position.y + _maxHeight;
    }

    void Update()
    {
        if (_isGrow)
        {
            //最大値に達していないなら
            if (transform.position.y < _maxHeight)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + _growSpeed);
            }
            //最大値に達したら
            else if(transform.position.y >= _maxHeight)
            {
                transform.position = new Vector2(transform.position.x, _maxHeight);
                _isGrow = false;
                _isGrowEnd = true;
            }
        }
    }
}

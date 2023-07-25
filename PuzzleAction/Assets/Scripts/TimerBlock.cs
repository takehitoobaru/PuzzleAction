using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerBlock : MonoBehaviour
{
    float _lifeTime = 5f;   //生存時間
    float _blinkStartTime = 3f; //点滅開始時間
    float _timeCount = 0;  //点滅開始用カウント
    float _blinkInterval = 10f;  //点滅間隔
    float _blinkCount = 0;  //点滅用カウント 
    float _changeAlpha = -120f;  //アルファ値変化

    SpriteRenderer _spriteRenderer; //コンポーネント

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Destroy(gameObject, _lifeTime);
    }

    void Update()
    {
        _timeCount += Time.deltaTime;

        if(_timeCount >= _blinkStartTime)
        {
            _blinkCount++;
            if(_blinkCount >= _blinkInterval)
            {
                _spriteRenderer.color += new Color(0, 0, 0, _changeAlpha);
                _changeAlpha *= -1;
                _blinkCount = 0;
            }
        }

        if(_timeCount >= _lifeTime)
        {
            transform.position = new Vector2(0, -500);
        }
    }
}

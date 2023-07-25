using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCheck : MonoBehaviour
{
    bool _isRoad = false;
    bool _isRoadEnter = false;
    bool _isRoadStay = false;
    bool _isRoadExit = false;
    string _blockTag = "Block";

    public bool IsRoad()
    {
        if(_isRoadEnter || _isRoadStay)
        {
            _isRoad = true;
        }
        else if (_isRoadExit)
        {
            _isRoad = false;
        }

        _isRoadEnter = false;
        _isRoadStay = false;
        _isRoadExit = false;

        return _isRoad;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag == _blockTag)
        {
            _isRoadEnter = true;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == _blockTag)
        {
            _isRoadStay = true;
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == _blockTag)
        {
            _isRoadExit = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Controller : MonoBehaviour
{
    [SerializeField] private Visualizator _mainVis;
    [SerializeField] private MethodMinSqrt _mainMMS;
    [SerializeField] private LagranjMethod _mainLM;
    [SerializeField] private Transform[] _mainPoints;
    
    private float _oldYMousePos = 0;
    private int _curretXSection = 0;
    private bool _followMouse = false;
    
    // Update is called once per frame
    void Update()
    {
        if (!_followMouse && (Input.GetKeyDown(KeyCode.Mouse0) || Input.touchCount > 0))
        {
            StartCoroutine(FollowMouseM());
        }
    }

    IEnumerator FollowMouseM()
    {
        _followMouse = true;
        bool MouseFollowMode = false;
        Vector3 worldPos;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            MouseFollowMode = true;

            worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _oldYMousePos = worldPos.y;

        }
        else
        {
            worldPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            _oldYMousePos = worldPos.y;
        }

        float minLength = (_mainPoints[0].position-worldPos).sqrMagnitude;
        _curretXSection = 0;
        for (int i = 1; i < _mainPoints.Length; i++)
        {
            float sqrtMagn = (_mainPoints[i].position - worldPos).sqrMagnitude;
            if (sqrtMagn < minLength)
            {
                minLength = sqrtMagn;
                _curretXSection = i;
            }
            
        }

        _oldYMousePos = worldPos.y;
        
        while (Input.GetKey(KeyCode.Mouse0) || Input.touchCount > 0)
        {
            float deltaY = 0;
            
            if (MouseFollowMode)
            {
                float newYMousePos = Camera.main.ScreenToWorldPoint(((Vector3)Input.mousePosition)+Vector3.forward*10).y;
                deltaY = newYMousePos-_oldYMousePos;
                _oldYMousePos = newYMousePos;
            }
            else
            {
                float newYMousePos = Camera.main.ScreenToWorldPoint(((Vector3)Input.GetTouch(0).position)+Vector3.forward*10).y;
                deltaY = newYMousePos-_oldYMousePos;
                _oldYMousePos = newYMousePos;
            }
            _mainPoints[_curretXSection].Translate(Vector3.up*deltaY);
            
            yield return new WaitForEndOfFrame();
        }

        _followMouse = false;
        
        _mainLM.SetParam(_curretXSection,_mainPoints[_curretXSection].position.y);
        _mainLM.SetL();
        
        _mainMMS.SetParam(_mainPoints[_curretXSection].position.y,_curretXSection);
        _mainMMS.SetKoofL();
        
        _mainVis.SetLines();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizator : MonoBehaviour
{
    [SerializeField] private Transform[] _points;
    [SerializeField] private LineRenderer[] _mainLines;

    [SerializeField] private LagranjMethod _lgrMt;
    [SerializeField] private MethodMinSqrt _msqrtMt;

    [SerializeField] private int _countOfCurvePoints = 24;
    [SerializeField] private float _lenghtOfCurvePoints = 0.25f;
    [SerializeField] private float _gridScaleX = 3;

    private void Start()
    {
        SetLines();
    }

    [ContextMenu("Set Lines")]
    public void SetLines()
    {
        Vector3[] pointPos = new Vector3[6];
        pointPos[0] = Vector3.zero;
        for (int i = 0; i < 5; i++)
        {
            pointPos[i + 1] = _points[i].localPosition;
        }
        _mainLines[0].SetPositions(pointPos);
        
        pointPos = new Vector3[_countOfCurvePoints];
        _mainLines[1].positionCount = _countOfCurvePoints;
        _msqrtMt.SetKoofL();
        for (int i = 0; i < pointPos.Length; i++)
        {
            pointPos[i] = new Vector3(i*_lenghtOfCurvePoints*_gridScaleX,Mathf.Clamp(_msqrtMt.GetMethod(i*_lenghtOfCurvePoints),0,100) ,0);
        }
        _mainLines[1].SetPositions(pointPos);
        
        pointPos = new Vector3[_countOfCurvePoints];
        _mainLines[2].positionCount = _countOfCurvePoints;
        _lgrMt.SetL();
        for (int i = 0; i < pointPos.Length; i++)
        {
            pointPos[i] = new Vector3(i*_lenghtOfCurvePoints*_gridScaleX,Mathf.Clamp(_lgrMt.Lp(i*_lenghtOfCurvePoints),0,100),0);
        }
        _mainLines[2].SetPositions(pointPos);
    }
}

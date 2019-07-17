using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class LagranjMethod : MonoBehaviour
{
    //Points and function values
    [ SerializeField] private float[] _p;
    [ SerializeField] private float[] _fp;

    //Coefficients for Lagrange polynomial
    private float[] _l;

    public void SetParams(float[] X,float[] Y)
    {
        _p = X;
        _fp = X;
    }
    
    public void SetParam(int id,float paramToSet)
    {
        if (id > _fp.Length)
        {
            return;
        }
        _fp[id] = paramToSet;
    }

    //Delegates for needed parameters
    public delegate double Function(double x);

    delegate float Operation(int x);

    [ContextMenu("SetL")]
    public void SetL()
    {
        _l = new float[_p.Length];
        float number = _p.Length;

        for (int i = 0; i < number; i++)
            _l[i] = _fp[i] / (float)Product(Enumerable.Range(0, _p.Length).Where(j => j != i), j => _p[i] - _p[j]);
    }

    //Lagrange's formula
    public float Lp(float x)
    {
        return (float)Enumerable.Range(0, _p.Length).Sum(i => _l[i]
                                                       * Product(Enumerable.Range(0, _p.Length).Where(j => j != i),
                                                           j => x - _p[j]));
    }

    //Auxiliary functions
    static double Product(IEnumerable<int> values, Operation oper)
    {
        return values.Aggregate(1D, (current, v) => current * oper(v));
    }
}
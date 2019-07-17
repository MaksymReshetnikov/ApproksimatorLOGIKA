using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MethodMinSqrt : MonoBehaviour
{
    /// <summary>
    /// Исходные параметры X
    /// </summary>
    [ SerializeField] private float[] PointsX = new float[5];
    /// <summary>
    /// Исходные параметры Y
    /// </summary>
    [ SerializeField] private float[] PointsY = new float[5];
    
    /// <summary>
    /// Сумма степеней X
    /// </summary>
    private float[] powSumX;
    /// <summary>
    /// Сумма степеней X помноженного на Y
    /// </summary>
    private float[] powSumY;
    /// <summary>
    /// Коофициенты в уравнении конечной кривой
    /// </summary>
    private float[] koofL = new float[4];

//    public void GetMatrix4()
//    {
//        Matrix4x4 newMatrix = new Matrix4x4(new Vector4(0,1,0,1),new Vector4(0,1,0,1),new Vector4(0,1,0,1),new Vector4(0,1,0,1) );
//        float _D = newMatrix.
//    }

    [ContextMenu("SetKoofL")]
    public void SetKoofL()
    {
        powSumAllParam();
        
        Matrix4x4 newMatrix = new Matrix4x4(new Vector4(powSumX[0],powSumX[1],powSumX[2],powSumX[3]),
            new Vector4(powSumX[1],powSumX[2],powSumX[3],powSumX[4]),
            new Vector4(powSumX[2],powSumX[3],powSumX[4],powSumX[5]),
            new Vector4(powSumX[3],powSumX[4],powSumX[5],powSumX[6]));
        Vector4 newFreeK = new Vector4(powSumY[0],powSumY[1],powSumY[2],powSumY[3]);
        
        Vector4 newSolve = newMatrix.inverse * newFreeK;

        koofL[0] = newSolve.x;
        koofL[1] = newSolve.y;
        koofL[2] = newSolve.z;
        koofL[3] = newSolve.w;
    }

    public float GetMethod(float x)
    {
        return (koofL[0] + x * koofL[1] + x * x * koofL[2] + x * x * x * koofL[3]);
    }

    private Vector4 SolveInequality(Matrix4x4 MainK,Vector4 FreeK)
    {
        float dK = MainK.determinant;
        
        if(Mathf.Abs(dK) <= 0)
        {
            Debug.Log("основной детерминант равен нулю");
            return Vector4.zero;
        }
        
        float dkX = new Matrix4x4(FreeK,MainK.GetColumn(1),MainK.GetColumn(2),MainK.GetColumn(3)).determinant;
        float dkY = new Matrix4x4(MainK.GetColumn(0),FreeK,MainK.GetColumn(2),MainK.GetColumn(3)).determinant;
        float dkZ = new Matrix4x4(MainK.GetColumn(0),MainK.GetColumn(1),FreeK,MainK.GetColumn(3)).determinant;
        float dkW = new Matrix4x4(MainK.GetColumn(0),MainK.GetColumn(1),MainK.GetColumn(2),FreeK).determinant;
        
        return new Vector4(dkX/dK,dkY/dK,dkZ/dK,dkW/dK);
    }

    public void SetParam(float Y, int id, float paramX = -1)
    {
        if (paramX < 0)
        {
            paramX = id;
        }
        
        PointsX[id] = paramX;
        PointsY[id] = Y;
    }

    public void ClearParam()
    {
        PointsX = new float[5];
        PointsY = new float[5];
    }
    
    private void powSumAllParam()
    {
        powSumX = new float[7];
        powSumY = new float[4];
        
        for (int i = 0; i < powSumX.Length; i++)
        {
            powSumX[i] = powSum(i,PointsX);
        }
        
        for (int i = 0; i < powSumY.Length; i++)
        {
            powSumY[i] = powSum(i,PointsX,PointsY);
        }
    }

    private float powSum(int pow, float[] paramsToSum)
    {
        float result = 0;

        foreach (var p in paramsToSum)
        {
            result += Mathf.Pow(p,pow);
        }
        
        return result;
    }
    
    private float powSum(int pow, float[] paramsToSum, float[] paramsToSumMult)
    {
        float result = 0;

        for (int i = 0; i < paramsToSum.Length;i++)
        {
            result = result + Mathf.Pow(paramsToSum[i],pow)*paramsToSumMult[i];
        }
        
        return result;
    }
}

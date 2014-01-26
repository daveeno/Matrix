using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MatVec
{
  public class Vector
  {
    private double[] _V;
    public double[] Components
    {
      get { return _V; }
    }

    private int _rows;
    public int Rows
    {
      get { return _rows; }
    }

    public Vector(int n)
    {
      _rows = n;
      _V = new double[_rows];
    }

    public Vector(double[] comps)
    {
      _rows = comps.Count();
      _V = new double[_rows];
      for (int i = 0; i < _rows; i++)
        _V[i] = comps[i];
    }

    public Vector(string[] rows)
    {
      _rows = rows.Count();
      _V = new double[_rows];
      for (int i = 0; i < _rows; i++)
        _V[i] = double.Parse(rows[i]);
    }

    public Vector(string vecstr)
    {
      string[] rows = vecstr.Split('\n');
      _rows = rows.Count();
      _V = new double[_rows];

      for (int i = 0; i < _rows; i++)
      {
        _V[i] = double.Parse(rows[i]);
      }
    }

    public static Vector operator +(Vector v1, Vector v2)
    {
      if (v1.Rows == v2.Rows)
      {
        double[] d = new double[v1.Rows];
        for(int i = 0; i < v1.Rows; i++)
        {
          d[i] = v1.Components[i] + v2.Components[i];
        }
        return new Vector(d);
      }
      return null;
    }

    public static Vector operator -(Vector v1, Vector v2)
    {
      if (v1.Rows == v2.Rows)
      {
        double[] d = new double[v1.Rows];
        for (int i = 0; i < v1.Rows; i++)
        {
          d[i] = v1.Components[i] - v2.Components[i];
        }
        return new Vector(d);
      }
      return null;
    }

    public static Vector operator *(Matrix m1, Vector v1)
    {
      Vector mv = new Vector(m1.getRows());

      mv.Components[0] = v1.Components[0] * m1.getVal(0, 0) + v1.Components[1] * m1.getVal(0, 1) + v1.Components[2] * m1.getVal(0, 2);
      mv.Components[1] = v1.Components[0] * m1.getVal(1, 0) + v1.Components[1] * m1.getVal(1, 1) + v1.Components[2] * m1.getVal(1, 2);
      mv.Components[2] = v1.Components[0] * m1.getVal(2, 0) + v1.Components[1] * m1.getVal(2, 1) + v1.Components[2] * m1.getVal(2, 2);

      return mv;
    }

    public Vector SMult(double s)
    {
      double[] d = new double[_rows];
      for (int i = 0; i < _rows; i++)
      {
        d[i] = _V[i] * s;
      }
      return new Vector(d);
    }

    public double Dotp(Vector v)
    {
      double d = 0;
      if (_rows == v.Rows)
      {
        for (int i = 0; i < _rows; i++)
        {
          d += _V[i] * v.Components[i];
        }
      }
      return d;
    }

    public Vector Crossp(Vector v)
    {
      double[] d = new double[3];
      if (_rows == 3 && _rows == v.Rows)
      {
        d[0] = _V[1] * v.Components[2] + _V[2] * v.Components[1];
        d[1] = _V[0] * v.Components[2] + _V[2] * v.Components[0];
        d[2] = _V[0] * v.Components[1] + _V[1] * v.Components[0];
      }
      return new Vector(d);
    }

    public double Norm()
    {
      double sum = 0;
      for (int i = 0; i < _rows; i++)
      {
        sum += _V[i] * _V[i];
      }
      return Math.Sqrt(sum);
    }

    public Vector Normalized()
    {
      double[] v = new double[_rows];
      double norm = this.Norm();
      for(int i = 0; i < _rows; i++)
      {
        v[i] = _V[i] / norm;
      }
      return new Vector(v);
    }

    public String toStr()
    {
      return this.toStr(0);
    }

    public String toStr(int decplcs)
    {
      String strout = "";

      for (int i = 0; i < this._rows; i++)
      {
        if (decplcs > 0) strout = strout + Math.Round(_V[i], decplcs);
        else strout = strout + _V[i];
        strout = strout + "\r\n";
      }

      return strout;
    }

    private double fix(double n, double dp)
    {
      if (dp > 0)
      {
        double prec = Math.Pow(10, dp);
        double num = Math.Round(n * dp);
        return num / prec;
      }
      return n;
    }
  }
}

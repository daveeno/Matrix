using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MatVec
{
  public class Matrix
  {
    double[,] _M;
    int _rows, _cols;

    public Matrix(int r, int c)
    {
      _M = new double[r, c];
      _rows = r;
      _cols = c;
      for(int i = 0; i < r; i++) {
        for (int j = 0; j < c; j++) {
          _M[i, j] = 0;
        }
      }
    }

    public Matrix(int r) // Makes Identity Matrix
    {
      _M = new double[r, r];
      _rows = r;
      _cols = r;
      for (int i = 0; i < r; i++) {
        for (int j = 0; j < r; j++) {
          if (i == j)
            _M[i, j] = 1;
          else
            _M[i, j] = 0;
        }
      }
    }

    public Matrix(string matstr)
    {
      string[] rows = matstr.Split(';');
      string[] cols = rows[0].Split(',');
      _rows = rows.Count();
      _cols = cols.Count();
      _M = new double[_rows, _cols];

      for (int i = 0; i < _rows; i++ )
      {
        cols = rows[i].Split(',');
        for (int j = 0; j < _cols; j++)
        {
          _M[i, j] = double.Parse(cols[j]);
        }
      }
    }

    public Matrix(string[] rows)
    {
      string[] cols = rows[0].Split(' ');
      _rows = rows.Count();
      _cols = cols.Count();
      //MessageBox.Show(_rows + ", " + _cols);
      _M = new double[_rows, _cols];

      for (int i = 0; i < _rows; i++)
      {
        cols = rows[i].Split(' ');
        for (int j = 0; j < _cols; j++)
        {
          _M[i, j] = double.Parse(cols[j]);
        }
      }
    }

    public static Matrix operator +(Matrix m1, Matrix m2)
    {
      if (m1.getRows() != m2.getRows())
        return null;
      if (m1.getCols() != m2.getCols())
        return null;
      Matrix dm = new Matrix(m1.getRows(), m1.getCols());
      for (int i = 0; i < m1.getRows(); i++)
      {
        for (int j = 0; j < m1.getCols(); j++)
        {
          dm.setVal(i, j, m1.getVal(i, j) + m2.getVal(i, j));
        }
      }
      return dm;
    }

    public static Matrix operator -(Matrix m1, Matrix m2)
    {
      if (m1.getRows() != m2.getRows())
        return null;
      if (m1.getCols() != m2.getCols())
        return null;
      Matrix dm = new Matrix(m1.getRows(), m1.getCols());
      for (int i = 0; i < m1.getRows(); i++)
      {
        for (int j = 0; j < m1.getCols(); j++)
        {
          dm.setVal(i, j, m1.getVal(i, j) - m2.getVal(i, j));
        }
      }
      return dm;
    }

    public static Matrix operator *(Matrix m1, Matrix m2)
    {
      Matrix MN = new Matrix(m1.getRows(), m2.getCols());
      int i, j, k;

      for (i = 0; i < m1.getRows(); i++)
      {
        for (j = 0; j < m2.getCols(); j++)
        {
          for (k = 0; k < m2.getRows(); k++)
          {
            MN.setVal(i, j, MN.getVal(i, j) + m1.getVal(i,k) * m2.getVal(k, j));
          }
        }
      }

      return MN;
    }

    public static Matrix operator *(double d, Matrix m1)
    {
      Matrix MN = new Matrix(m1.getRows(), m1.getCols());
      int i, j;

      for (i = 0; i < m1.getRows(); i++)
      {
        for (j = 0; j < m1.getCols(); j++)
        {
          MN.setVal(i, j, m1.getVal(i, j) * d);
        }
      }

      return MN;
    }

    public void setVal(int r, int c, double val)
    {
      this._M[r, c] = val;
    }

    public double getVal(int r, int c)
    {
      return this._M[r, c];
    }

    public int getRows()
    {
      return _rows;
    }

    public int getCols()
    {
      return _cols;
    }

    public void swapRows(int r1, int r2)
    {
      for (int i = 0; i < this._cols; i++) {
        var t = this._M[r1, i];
        this._M[r1, i] = this._M[r2, i];
        this._M[r2, i] = t;
      }
    }

    public Matrix transpose()
    {
      Matrix N = new Matrix(this._cols, this._rows);

      for (int i = 0; i < this._rows; i++) {
        for (int j = 0; j < this._cols; j++) {
          N.setVal(j, i, this._M[i, j]);
        }
      }
      return N;
    }

    public bool isInvertable()
    {
      try
      {
        this.inverse();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public Matrix inverse()
    {
      if (_rows != _cols) return null;

      int i, j;
      Matrix a = new Matrix(this._rows, this._cols);

      Matrix N;
      N = this.augment(new Matrix(this._rows));
      N.rref();
      for (i = 0; i < this._rows; i++)
      {
        for (j = 0; j < this._cols; j++)
        {
          a.setVal(i, j, N.getVal(i, j + N.getRows()));
        }
      }
      return a;
    }

    public Matrix augment(Matrix N)
    {
      int i, j;
      Matrix a = new Matrix(this._rows, this._cols + N.getCols());
      for (i = 0; i < this._rows; i++) {
        for (j = 0; j < this._cols; j++) {
          a.setVal(i, j, _M[i,j]);
        }
        for (j = 0; j < N.getCols(); j++) {
          a.setVal(i, j + this._rows, N.getVal(i, j));
        }
      }
      return a;
    }

    public Matrix lastNCols(int col)
    {
      Matrix M = new Matrix(this._rows, col);
      int k = 0;
      for(int j = 0; j < this._rows; j++)
      {
        for(int i = this._cols - col; i < this._cols; i++)
        {
          M.setVal(j, k, this._M[j, i]);
          k++;
        }
        k = 0;
      }
      
      return M;
    }

    public void rref()
    {
      int i, j, r;

      int lead = 0;
      for (r = 0; r < this._rows; r++)
      {
        if (lead >= this._cols)
          break;
        {
          i = r;
          while (_M[i, lead] == 0)
          {
            i++;
            if (i == this._rows)
            {
              i = r;
              lead++;
              if (lead == this._cols)
                break;
            }
          }
          swapRows(r, i);
        }

        double lv = _M[r, lead];
        for (j = 0; j < this._cols; j++)
          _M[r, j] = _M[r, j] / lv;

        for (i = 0; i < this._rows; i++)
        {
          if (i != r)
          {
            lv = _M[i, lead];
            for (j = 0; j < this._cols; j++)
              _M[i, j] -= lv * _M[r, j];
          }
        }
        lead++;
      }
    }

    public Matrix solve(int solncols)
    {
      if (this._rows >= this._cols)
        return null;

      Matrix soln = new Matrix(this._rows, solncols);

      int i, j, k, p;
      int n = this._rows;

      for (i = 0; i <= n - 2; i++)
      {
        p = i;
        while (_M[p,i] == 0 && p < n - 1)
          p++;
        if (p == n - 1)
        {
          return null;
        }
        if (p != i)
        {
          swapRows(p, i);
        }
        double m_ji;
        for (j = i + 1; j < n; j++)
        {
          m_ji = _M[j,i] / _M[i,i];
          for (k = i; k <= n; k++)
          {
            _M[j,k] -= m_ji * _M[i,k];
          }
          _M[j,i] = 0;
        }
      }

      double sum = 0;
      double[] x;
      x = new double[n];

      x[n - 1] = _M[n - 1,n] / _M[n - 1,n - 1];
      for (i = n - 2; i >= 0; i--)
      {
        for (j = i + 1; j <= n - 1; j++)
        {
          sum += _M[i,j] * x[j];
        }
        x[i] = (_M[i,n] - sum) / _M[i,i];
        sum = 0;
      }

      // Output
      for (k = 0; k < n; k++)
      {
        soln.setVal(k,0, x[k]);
      }
      return soln;
    }

    public Matrix mmult(Matrix N)
    {
      Matrix MN = new Matrix(this._rows, N.getCols());
      int i, j, k;

      for (i = 0; i < this._rows; i++) {
        for (j = 0; j < N.getCols(); j++) {
          for (k = 0; k < N.getRows(); k++) {
            MN.setVal(i, j, MN.getVal(i, j) + this._M[i,k] * N.getVal(k, j));
          }
        }
      }

      return MN;
    }

    public String toStr()
    {
      return this.toStr(0);
    }

    public String toStr(int decplcs)
    {
      String strout = "";
      int i, j;

      for (i = 0; i < this._rows; i++)
      {
        //strout += "[ ";
        for (j = 0; j < this._cols; j++)
        {
          if (decplcs > 0) strout = strout + Math.Round(_M[i, j], decplcs) + " ";
          else strout = strout + _M[i, j] + " ";
        }
        strout = strout + "\r\n";
      }

      return strout;
    }

    public string toMatStr()
    {
      String strout = "";
      int i, j;

      for (i = 0; i < this._rows; i++)
      {
        //strout += "[ ";
        for (j = 0; j < this._cols; j++)
        {
          strout = strout + _M[i, j];
          if(j < this._cols-1) strout += ",";
        }
        if(i < this._rows - 1) strout = strout + ";";
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

    public void addRows(int n)
    {
      this._rows += n;
      
    }


    public double DET()
    {
      int i, j, k;
      double det = 0;
      int n = _rows;
      for (i = 0; i < n - 1; i++)
      {
        for (j = i + 1; j < n; j++)
        {
          det = _M[j, i] / _M[i, i];
          for (k = i; k < n; k++)
            _M[j, k] = _M[j, k] - det * _M[i, k];
        }
      }
      det = 1;
      for (i = 0; i < n; i++)
        det = det * _M[i, i];
      return det;
    }

  } // Matrix
}

    public struct RationalMatrix
    {
        private readonly RationalNumber[,] C;

        private RationalMatrix(RationalNumber[,] C)
        {
            this.C = C;
        }


        public RationalMatrix(int dim)
        {
            this.C = new RationalNumber[dim, dim];
            for (int row = 0; row < dim; row++)
            {
                for (int col = 0; col < dim; col++)
                {
                    C[row, col] = 0;
                }

            }
        }

        public int Dimension
        {
            get
            {
                return C.GetLength(0);
            }
        }

        public RationalNumber this[int row, int col]
        {
            get
            {
                return this.C[row, col];
            }
            set
            {
                this.C[row, col] = value;
            }
        }

        public RationalMatrix Clone()
        {
            int D = Dimension;
            RationalMatrix M = new RationalMatrix(D);
            for (int r = 0; r < D; r++)
            {
                for (int c = 0; c < D; c++)
                {
                    M[r, c] = this[r, c];
                }
            }
            return M;
        }

        public RationalNumber[] Column(int c, params RationalNumber[] newvalues)
        {
            int D = Dimension;
            for (int i = 0; i < newvalues.Length && i < Dimension; i++)
            {
                this[i, c] = newvalues[i];
            }
            RationalNumber[] v = new RationalNumber[D];
            for (int i = 0; i < D; i++)
            {
                v[i] = this[i, c];
            }
            return v;
        }

        public RationalNumber[] Row(int r, params RationalNumber[] newvalues)
        {
            int D = Dimension;

            for (int i = 0; i < newvalues.Length && i < Dimension; i++)
            {
                this[r, i] = newvalues[i];
            }

            RationalNumber[] v = new RationalNumber[D];
            for (int i = 0; i < D; i++)
            {
                v[i] = this[r, i];
            }
            return v;
        }

        public static RationalMatrix From(double[,] a)
        {
            RationalMatrix M = new RationalMatrix(a.GetLength(0));
            M = a;
            return M;
        }

        public static implicit operator RationalNumber[,] (RationalMatrix M)
        {
            return M.C;
        }
        public static implicit operator RationalMatrix(RationalNumber[,] m)
        {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);
            if (rows != cols)
            {
                throw new System.NotSupportedException();
            }
            return new RationalMatrix(m);
        }
        public static implicit operator RationalMatrix(double[,] m)
        {
            int rows = m.GetLength(0);
            int cols = m.GetLength(1);
            if (rows != cols)
            {
                throw new System.NotSupportedException();
            }
            RationalMatrix M = new RationalMatrix(rows);
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    M[r, c] = m[r, c];
                }
            }
            return M;
        }


        public override string ToString()
        {
            int D = Dimension;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.AppendLine("[");
            for (int row = 0; row < D; row++)
            {
                builder.Append(" [ ");
                for (int col = 0; col < D; col++)
                {
                    if (col > 0)
                    {
                        builder.Append(" , ");
                    }
                    builder.Append(this[row, col]);
                }
                if (row < D - 1)
                {
                    builder.AppendLine(" ],");
                }
                else
                {
                    builder.AppendLine(" ]");
                }
            }
            builder.AppendLine("]");
            return builder.ToString();
        }

        public RationalNumber Determinant()
        {
            int D = Dimension;
            RationalNumber result = 0;
            RationalMatrix A = Clone();
            for (int i = 0; i < D; i++)
            {
                RationalNumber pivot = A[i, i];
                for (int j = 0; j < D; j++)
                {
                    if (i != j)
                    {
                        for (int k = i + 1; k < D; k++)
                        {
                            RationalNumber divider = i == 0 ? 1 : A[i - 1, i - 1];
                            RationalNumber a = pivot * A[j, k];
                            RationalNumber b = A[j, i] * A[i, k];
                            RationalNumber ab = a - b;
                            ab = ab / divider;
                            A[j, k] = ab;
                        }
                    }
                }
            }

            return A[D - 1, D - 1];
        }

        private RationalMatrix MinorMatrix(int row, int col)
        {
            int D = Dimension;
            RationalMatrix M = new RationalMatrix(D - 1);
            int i = 0;
            int j = 0;
            for (int r = 0; r < D; r++)
            {
                for (int c = 0; c < D; c++)
                {
                    if (r != row && c != col)
                    {
                        M[i, j++] = this[r, c];
                        if (j == D - 1)
                        {
                            j = 0;
                            i++;
                        }
                    }
                }
            }
            return M;
        }

        public RationalMatrix CofactorMatrix()
        {
            int D = Dimension;
            RationalMatrix CF = new RationalMatrix(D);
            if (D == 1)
            {
                CF[0, 0] = 1;
                return CF;
            }
            int sign;
            for (int i = 0; i < D; i++)
            {
                for (int j = 0; j < D; j++)
                {
                    sign = ((i + j) % 2 == 0) ? 1 : -1; ;
                    CF[i, j] = MinorMatrix(i, j).Determinant() * sign;
                }
            }
            return CF;
        }

        public RationalMatrix AdjointMatrix()
        {
            return CofactorMatrix().TransposeMatrix();
        }

        public RationalMatrix InverseMatrix()
        {
            RationalNumber det = Determinant();
            if (det == 0)
            {
                return this.ZeroMatrix();
            }

            return AdjointMatrix() / det;
        }

        public RationalMatrix TransposeMatrix()
        {
            int D = Dimension;
            RationalMatrix T = new RationalMatrix(D);
            for (int r = 0; r < D; r++)
            {
                for (int c = 0; c < D; c++)
                {
                    T[c, r] = this[r, c];
                }
            }
            return T;
        }

        public static RationalMatrix operator +(RationalMatrix a, RationalMatrix b)
        {
            if (a.Dimension != b.Dimension)
            {
                throw new System.NotSupportedException();
            }
            RationalMatrix M = new RationalMatrix(a.Dimension);
            for (int r = 0; r < M.Dimension; r++)
            {
                for (int c = 0; c < M.Dimension; c++)
                {
                    M[r, c] = a[r, c] + b[r, c];
                }
            }
            return M;
        }

        public static RationalMatrix operator -(RationalMatrix a, RationalMatrix b)
        {
            if (a.Dimension != b.Dimension)
            {
                throw new System.NotSupportedException();
            }
            RationalMatrix M = new RationalMatrix(a.Dimension);
            for (int r = 0; r < M.Dimension; r++)
            {
                for (int c = 0; c < M.Dimension; c++)
                {
                    M[r, c] = a[r, c] - b[r, c];
                }
            }
            return M;
        }

        public static RationalMatrix operator *(RationalMatrix a, RationalMatrix b)
        {
            if (a.Dimension != b.Dimension)
            {
                throw new System.NotSupportedException();
            }
            RationalMatrix M = new RationalMatrix(a.Dimension);

            for (int i = 0; i < a.Dimension; i++)
            {
                for (int j = 0; j < a.Dimension; j++)
                {
                    for (int k = 0; k < a.Dimension; k++)
                    {
                        M[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return M;
        }
        public static RationalMatrix operator *(RationalNumber a, RationalMatrix b)
        {
            RationalMatrix M = new RationalMatrix(b.Dimension);
            for (int r = 0; r < M.Dimension; r++)
            {
                for (int c = 0; c < M.Dimension; c++)
                {
                    M[r, c] = a * b[r, c];
                }
            }
            return M;
        }

        public static RationalMatrix operator /(RationalMatrix a, RationalNumber b)
        {
            RationalMatrix M = new RationalMatrix(a.Dimension);
            for (int r = 0; r < M.Dimension; r++)
            {
                for (int c = 0; c < M.Dimension; c++)
                {
                    M[r, c] = a[r, c] / b;
                }
            }
            return M;
        }
        public RationalMatrix IdentityMatrix()
        {
            return IdentityMatrix(Dimension);
        }

        public static RationalMatrix IdentityMatrix(int D)
        {
            RationalMatrix I = new RationalMatrix(D);
            for (int i = 0; i < D; i++)
            {
                I[i, i] = 1;
            }
            return I;
        }

        public RationalMatrix ZeroMatrix()
        {
            return ZeroMatrix(Dimension);
        }

        public static RationalMatrix ZeroMatrix(int D)
        {
            RationalMatrix I = new RationalMatrix(D);
            return I;
        }


        public static RationalMatrix operator ^(RationalMatrix a, int exponent)
        {
            if (exponent < 0)
            {
                throw new System.NotSupportedException();
            }
            RationalMatrix R = a.IdentityMatrix();
            while (exponent > 0)
            {
                R *= a;
                exponent--;
            }
            return R;
        }
    }

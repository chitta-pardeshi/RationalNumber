# RationalNumber

Still work in progress. 
Planning to complete following structures before 2020 year end.

1. RationalNumber - ratio of two big integers
2. RationalMatrix - n-dimensional square matrix of RationalNumber
3. RationalVector - n-dimensional vector of RationNumber
4. RationalPoly   - multi-variate polynomial with rational coeffecients
5. RationalColor  - CIE-XYZ color using rational numbers
6. RationalImage  - image plotted with rational color.

all good bad ugly comments are welcome and please send to chitta.pardeshi@gmail.com.

example

Rational Number Represented As Ratio of BigInteger

            // magic square matrix 3x3
            RationalMatrix A = new double[,] {
                { 4,   9,   2 },
                { 3,   5,   7 },
                { 8,   1,   6 }
            };

            System.Console.WriteLine("Determinant: {0}", A.Determinant().ToString());

            /*
             * Determinant: 360
            */

            RationalMatrix B = A.InverseMatrix();
            System.Console.WriteLine(B.ToString());

            /*
                [
                 [ 23/360 , -13/90 , 53/360 ],
                 [ 19/180 , 1/45 , -11/180 ],
                 [ -37/360 , 17/90 , -7/360 ]
                ]
            */

            RationalMatrix C = A * B;
            System.Console.WriteLine(C.ToString());

            /*
                [
                 [ 1 , 0 , 0 ],
                 [ 0 , 1 , 0 ],
                 [ 0 , 0 , 1 ]
                ]
            */

            System.Console.ReadLine();


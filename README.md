# RationalNumber
Rational Number Represented As Ratio of BigInteger

            RationalMatrix A = new double[,] {
                { 4, 7 },
                { 2, 6 }
            };
            RationalMatrix B = A.InverseMatrix();
            System.Console.WriteLine(B.ToString());

            /* outputs
                [
                 [ 3/5 , -7/10 ],
                 [ -1/5 , 2/5 ]
                ]
            */

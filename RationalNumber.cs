    public struct RationalNumber
    {
        public static RationalNumber One = new RationalNumber(1, 1);
        public static RationalNumber Zero = new RationalNumber(0, 1);

        private readonly System.Numerics.BigInteger num;
        private readonly System.Numerics.BigInteger den;

        private static int Compare(double left, double right)
        {
            return System.Math.Sign(left - right);
        }

        public static int Compare(RationalNumber left, RationalNumber right)
        {
            return System.Numerics.BigInteger.Compare(left.num * right.den, left.den * right.num);
        }

        private RationalNumber(System.Numerics.BigInteger numerator, System.Numerics.BigInteger denominator)
        {
            System.Numerics.BigInteger g = System.Numerics.BigInteger.Abs(System.Numerics.BigInteger.GreatestCommonDivisor(numerator, denominator));
            if (numerator != 0)
            {
                if (denominator < 0)
                {
                    num = -numerator / g;
                    den = -denominator / g;
                }
                else
                {
                    num = numerator / g;
                    den = denominator / g;
                }
            }
            else
            {
                num = numerator;
                den = denominator;
            }
        }

        public static RationalNumber From(string numerator, string denominator)
        {
            return new RationalNumber(System.Numerics.BigInteger.Parse(numerator), System.Numerics.BigInteger.Parse(denominator));
        }

        public static RationalNumber From(System.Numerics.BigInteger numerator, System.Numerics.BigInteger denominator)
        {
            return new RationalNumber(numerator, denominator);
        }

        public System.Numerics.BigInteger Numerator
        {
            get
            {
                return num;
            }
        }

        public System.Numerics.BigInteger Denominator
        {
            get
            {
                return den;
            }
        }

        public int Sign()
        {
            if (num.Sign == 0) return 0;
            return num.Sign / den.Sign;
        }

        public RationalNumber Abs()
        {
            return new RationalNumber(System.Numerics.BigInteger.Abs(num), System.Numerics.BigInteger.Abs(den));
        }

        public bool IsNaN()
        {
            return num != 0 && den == 0;
        }


        public static RationalNumber operator +(RationalNumber a)
        {
            return a;
        }

        public static RationalNumber operator -(RationalNumber a)
        {
            return new RationalNumber(-a.num, a.den);
        }

        public static RationalNumber operator +(RationalNumber a, RationalNumber b)
        {
            return new RationalNumber(a.num * b.den + b.num * a.den, a.den * b.den);
        }

        public static RationalNumber operator -(RationalNumber a, RationalNumber b)
        {
            return a + (-b);
        }

        public static RationalNumber operator *(RationalNumber a, RationalNumber b)
        {
            return new RationalNumber(a.num * b.num, a.den * b.den);
        }

        public static RationalNumber operator /(RationalNumber a, RationalNumber b)
        {
            if (a.num == 0) return new RationalNumber(0, 1);
            if (b.num == 0)
            {
                throw new System.DivideByZeroException();
            }
            return new RationalNumber(a.num * b.den, a.den * b.num);
        }

        public static bool operator <=(RationalNumber a, RationalNumber b)
        {
            return RationalNumber.Compare(a, b) <= 0;
        }

        public static bool operator <(RationalNumber a, RationalNumber b)
        {
            return RationalNumber.Compare(a, b) < 0;
        }

        public static bool operator >(RationalNumber a, RationalNumber b)
        {
            return RationalNumber.Compare(a, b) > 0;
        }

        public static bool operator >=(RationalNumber a, RationalNumber b)
        {
            return RationalNumber.Compare(a, b) >= 0;
        }

        public static bool operator ==(RationalNumber a, RationalNumber b)
        {
            return RationalNumber.Compare(a, b) == 0;
        }

        public static bool operator !=(RationalNumber a, RationalNumber b)
        {
            return RationalNumber.Compare(a, b) != 0;
        }

        public static RationalNumber operator |(RationalNumber a, RationalNumber b)
        {
            return new RationalNumber(a.num + b.num, a.den + b.den);
        }

        public RationalNumber Reciprocal ()
        {
            return new RationalNumber(den, num);
        }

        public static RationalNumber operator ^(RationalNumber a, int exponent)
        {
            if (exponent > 0)
            {
                return new RationalNumber(System.Numerics.BigInteger.Pow(a.num, exponent), System.Numerics.BigInteger.Pow(a.den, exponent));
            }
            if (exponent == 0) return One;
            return a.Reciprocal() ^ (-exponent);
        }

        public static implicit operator RationalNumber(int a)
        {
            return new RationalNumber(a, 1);
        }

        public static implicit operator RationalNumber(long a)
        {
            return new RationalNumber(a, 1);
        }

        private static RationalNumber From (string[] str)
        {
            if (str.Length != 2)
            {
                throw new System.NotSupportedException();
            }
            return From(System.Numerics.BigInteger.Parse(str[0].Trim()), System.Numerics.BigInteger.Parse(str[1].Trim()));
        }

        public static implicit operator RationalNumber(string str)
        {
            int index = str.IndexOf('/');
            if (index > 0)
            {
                return From(str.Split('/'));
            }
            else if (str.IndexOf('.') >= 0)
            {
                return double.Parse(str);
            }
            else
            {
                return new RationalNumber(System.Numerics.BigInteger.Parse(str), 1);
            }
        }

        public static implicit operator RationalNumber(double v)
        {
            int sgn = v < 0 ? -1 : (v > 0 ? 1 : 0);
            if (sgn == 0) return new RationalNumber(0, 1);
            if (sgn < 0) v = -v;

            double dy1 = 0;
            double dx1 = 1;
            double dy2 = 1;
            double dx2 = 0;

            RationalNumber lo = new RationalNumber(0, 1);
            RationalNumber hi = new RationalNumber(1, 0);

            int r = 0;
            while ((r = Compare(v, (dy1 + dy2) / (dx1 + dx2))) != 0)
            {
                if (r > 0)
                {
                    dy1 += dy2;
                    dx1 += dx2;
                    lo |= hi;
                }
                else
                {
                    dy2 += dy1;
                    dx2 += dx1;
                    hi |= lo;
                }
            }
            RationalNumber f = lo | hi;
            if (sgn < 0)
            {
                return -f;
            }
            else
            {
                return f;
            }
        }

        public static explicit operator double(RationalNumber a)
        {
            if (a.IsNaN()) return double.NaN;
            int sgn = a.Sign();
            if (sgn == 0) return 0;
            a = a.Abs();

            double dy1 = 0;
            double dx1 = 1;
            double dy2 = 1;
            double dx2 = 0;

            RationalNumber lo = new RationalNumber(0, 1);
            RationalNumber hi = new RationalNumber(1, 0);

            int r = 0;
            while ((r = Compare(a, lo | hi)) != 0)
            {
                if (r > 0)
                {
                    dy1 += dy2;
                    dx1 += dx2;
                    lo |= hi;
                }
                else
                {
                    dy2 += dy1;
                    dx2 += dx1;
                    hi |= lo;
                }
            }
            if (sgn < 0)
            {
                return -(dy1 + dy2) / (dx1 + dx2);
            }
            else
            {
                return (dy1 + dy2) / (dx1 + dx2);
            }
        }

        public override string ToString()
        {
            if (IsNaN())
            {
                return "NaN";
            }
            else if (num == 0)
            {
                return "0";
            }
            else if (den == 1)
            {
                return string.Format("{0}", num);
            }
            else
            {
                return string.Format("{0}/{1}", num, den);
            }
        }

        public override bool Equals(object obj)
        {
            if (obj is RationalNumber)
            {
                return this == (RationalNumber)obj;
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

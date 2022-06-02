namespace DecimalDemoProject
{
    using System;

    //Expect value matches decimal (max precision = 28)
    public readonly struct PrecisionDecimal
    {
        private const int MAX_PRECISION = 38;


        public readonly decimal Value;
        private readonly int Precision;
        private readonly int Scale;

        public PrecisionDecimal(decimal value, int? precision = null, int? scale = null)
        {
            if(precision > MAX_PRECISION || precision <= 0)
                throw new ArgumentException($"Wrong Precision! Precision cannot be more than {MAX_PRECISION} and less then or equal to 0.", nameof(precision));      
            if (scale < 0)
                throw new ArgumentException($"Wrong Scale! Scale cannot be less then 0.", nameof(scale));
            if (scale > precision)
                throw new ArgumentException("Scale cannot be more than Precision.", nameof(scale));

            int valuePrecision = value.GetPrecision();
            int valueScale = value.GetScale();
            precision = precision ?? valuePrecision;
            scale = scale ?? valueScale;

            if (scale < valueScale)
            {
                value = Math.Round(value, scale.Value, MidpointRounding.ToEven);
                valuePrecision = value.GetPrecision();
                valueScale = value.GetScale();
            }

            if (valuePrecision - valueScale > precision - scale)
                throw new ArgumentException("Integral value cannot be more than indicated integral.", "ValueIntegral");

            if (scale < valueScale)
                value = Math.Round(value, scale.Value, MidpointRounding.ToEven);

            this.Value = value;
            this.Precision = precision.Value;
            this.Scale = scale.Value;
        }

        public static PrecisionDecimal operator +(PrecisionDecimal a) => a;

        public static PrecisionDecimal operator -(PrecisionDecimal a) => new PrecisionDecimal(-a.Value, a.Precision, a.Scale);

        public static PrecisionDecimal operator +(PrecisionDecimal a, PrecisionDecimal b)
        {
            var newValue = a.Value + b.Value;
            var newPrecision = Math.Max(a.Scale, b.Scale) + Math.Max(a.Precision - a.Scale, b.Precision - b.Scale) + 1;
            var newScale = Math.Max(a.Scale, b.Scale);

            if (newPrecision > MAX_PRECISION)
            {
                var newValueIntegral = newValue.GetPrecision() - newValue.GetScale();
                if (newValueIntegral + newScale <= MAX_PRECISION)
                {
                    newPrecision = MAX_PRECISION;
                }
                //Same as in sql, but it never happens as the expected value matches C# decimal value.
                else
                {
                    throw new ArgumentException($"Precision cannot be more than {MAX_PRECISION}.", nameof(Precision));
                }
            }

            return new PrecisionDecimal(newValue, newPrecision, newScale);
        }

        public static PrecisionDecimal operator -(PrecisionDecimal a, PrecisionDecimal b)
            => a + (-b);

        public static PrecisionDecimal operator *(PrecisionDecimal a, PrecisionDecimal b)
        {
            int integralLimit = 32;
            int scaleLimit = MAX_PRECISION - integralLimit;

            var newValue = a.Value * b.Value;
            var newPrecision = a.Precision + b.Precision + 1;
            var newScale = a.Scale + b.Scale;

            var newValueIntegral = newValue.GetPrecision() - newValue.GetScale();
            if (newValueIntegral < integralLimit)
            {
                int newIntegral = Math.Min(integralLimit, newPrecision - newScale);
                newScale = Math.Min(newScale, MAX_PRECISION - newIntegral);
                newPrecision = newIntegral + newScale;
            }
            //Same as in sql, but it never happens as the expected value matches C# decimal value.
            else
            {
                if (newScale < scaleLimit)
                {
                    throw new OverflowException();
                }
                else
                {
                    newPrecision = MAX_PRECISION;
                    newScale = scaleLimit;
                }
            }
            newValue = Math.Round(newValue, newScale, MidpointRounding.ToEven);
            return new PrecisionDecimal(newValue, newPrecision, newScale);
        }

        public static PrecisionDecimal operator /(PrecisionDecimal a, PrecisionDecimal b)
        {
            int integralLimit = 32;
            int scaleLimit = MAX_PRECISION - integralLimit;

            var newValue = a.Value / b.Value;
            var newPrecision = a.Precision - a.Scale + b.Scale + Math.Max(6, a.Scale + b.Precision + 1);
            var newScale = Math.Max(6, a.Scale + b.Precision + 1);

            var newValueIntegral = newValue.GetPrecision() - newValue.GetScale();
            if (newValueIntegral < integralLimit)
            {
                int newIntegral = Math.Min(integralLimit, newPrecision - newScale);
                newScale = Math.Min(newScale, MAX_PRECISION - newIntegral);
                newPrecision = newIntegral + newScale;
            }
            //Same as in sql, but it never happens as the expected value matches C# decimal value.
            else
            {
                if (newScale < scaleLimit)
                {
                    throw new OverflowException();
                }
                else
                {
                    newPrecision = MAX_PRECISION;
                    newScale = scaleLimit;
                }
            }
            newValue = newValue.TruncateToNPlaces(newScale);
            return new PrecisionDecimal(newValue, newPrecision, newScale);
        }

        public override bool Equals(Object obj)
        {
            return obj is PrecisionDecimal precisionDecimal && Value == precisionDecimal.Value;
        }

        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                // Suitable nullity checks etc, of course :)
                hash = hash * 23 + Value.GetHashCode();
                hash = hash * 23 + Scale.GetHashCode();
                hash = hash * 23 + Precision.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(PrecisionDecimal a, PrecisionDecimal b)
        {
            return a.Value == b.Value;
        }

        public static bool operator !=(PrecisionDecimal a, PrecisionDecimal b)
        {
            return !(a == b);
        }
    }
}

using System;
using UnityEngine;

namespace Source.Utility
{
    public class EquationMaker
    {
        private readonly Vector2 _min, _max;
        private double _slope, _yIntersect;
        
        public EquationMaker(string type, Vector2 min, Vector2 max)
        {
            CheckType(type);
            
            _min = min;
            _max = max;

            switch (type)
            {
                case "Linear":
                    FindLinearEquation();
                    break;
                case "Power":
                    
                    break;
                case "Logarithmic":
                    
                    break;
                default: throw new Exception("Somehow, the type [" + type + "] got through error checking...");
            }
        }

        public double CalcValue(double x)
        {
            return _slope * x + _yIntersect;
        }

        public Vector2 GetMax()
        {
            return _max;
        }

        public Vector2 GetMin()
        {
            return _min;
        }

        private void FindLinearEquation()
        {
            // Use Point Formula - y2-y1 / x2-x1 - to find slope
            _slope = (_max.y - _min.y) / (_max.x - _min.x);
            
            // Plug in to find y-intersect
            _yIntersect = _min.y - _min.x * _slope;
        }

        private void CheckType(string type)
        {
            if (type != "Linear" && type != "Power" && type != "Logarithmic") throw new Exception("Incorrect type defined for Equation Maker. Type defined -> " + type);
        }
    }
}

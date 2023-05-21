using System;
using UnityEngine;

namespace Utils
{

    [Serializable] public struct Optional<T>
    {
        [SerializeField] private bool _enabled;
        [SerializeField] private T _value;

        public bool enabled => _enabled;
        public T value => _value;

        public Optional(T initialValue) {
            this._enabled = true;
            this._value = initialValue;
        }
    }

    struct Math
    {
        static public void RotationToDirection(Vector2 direction, out Quaternion rotation) {
            rotation = Quaternion.LookRotation(Vector3.forward, Quaternion.identity * direction);
        }
        
        static public void DirectionToRotation(Quaternion rotation, out Vector2 direction) {
            direction = rotation * Vector3.forward;
        }
    }

    namespace Bezier
    {
        public struct BezierCurve
        {
            private Vector2 _anchorL;
            public Vector2 anchorL => _anchorL;

            private Vector2 _controlL;
            public Vector2 controlL => _controlL;

            private Vector2 _controlR;
            public Vector2 controlR => _controlR;

            private Vector2 _anchorR;
            public Vector2 anchorR => _anchorR;

            public BezierCurve(Vector2 anchorL, Vector2 controlL, Vector2 controlR, Vector2 anchorR) {
                this._anchorL = anchorL;
                this._controlL = controlL;
                this._controlR = controlR;
                this._anchorR = anchorL;
            }

            public Vector2 Position(float t) {
                t = Mathf.Clamp01(t);

                Vector2 a = Vector2.Lerp(anchorL, controlL, t);
                Vector2 b = Vector2.Lerp(controlL, controlR, t);
                Vector2 c = Vector2.Lerp(controlR, anchorR, t);

                Vector2 d = Vector2.Lerp(a, b, t);
                Vector2 e = Vector2.Lerp(b, c, t);

                Vector2 p = Vector2.Lerp(d, e, t);

                return p;
            }
        }
    }

    namespace ProceduralAnimation
    {        
        [System.Serializable] public struct PersonalityData
        {
            [SerializeField] [Range(0.1f, 20)] private float _f;
            [SerializeField] [Range(0, 1)] private float _z;
            [SerializeField] [Range(-1, 5)] private float _r;

            public float f {
                get => _f;
                set => _f = value <= 0 ? 0.1f : value;
            }

            public float z {
                get => _z;
                set => _z = Mathf.Clamp01(value);
            }

            public float r => _r;

            public PersonalityData(float f, float z, float r) {
                this._f = f;
                this._z = z;
                this._r = r;
            }
        }
        public struct Personality
        {
            private Vector2 _xp, _xd;
            private Vector2 _y, _yd;
            private float _k1, _k2, _k3;
        
            public Personality(PersonalityData data) {
                this._k1 = data.z / (Mathf.PI * data.f);
                this._k2 = 1 / ((2 * Mathf.PI * data.f) * (2 * Mathf.PI * data.f));
                this._k3 = data.r * data.z / (2 * Mathf.PI * data.f);

                this._xp = Vector2.zero;
                this._xd = Vector2.zero;
                this._y = Vector2.zero;
                this._yd = Vector2.zero;
            }

            public Vector2 UpdateInterpolation(float T, Vector2 x) {
                if (this._xd == null) {
                    this._xd = (x - this._xp) / T;
                    this._xp = x;
                }

                this._y = this._y + T * this._yd;
                this._yd = this._yd + T * (x + this._k3 * this._xd - this._y - this._k1 * this._yd) / this._k2;
                
                return this._y;
            }
        }
    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;

namespace BiniLab
{
    public class Utils
    {

        public static Vector3 CalcPosition(Vector3 CurPos, float angle, float distance)
        {
            Vector3 calcPos = Vector3.zero;
            calcPos.x = 1 * Mathf.Cos(angle * Mathf.PI / 180) * distance;
            calcPos.y = 1 * Mathf.Sin(angle * Mathf.PI / 180) * distance;
            return calcPos;
        }

        public static bool CheckProbability(float rate)
        {
            return UnityEngine.Random.Range(0f, 1f) < rate;
        }

        public static void Shuffle(IList list)
        {
            int count = list.Count;
            int last = count - 1;
            for (int i = 0; i < last; ++i)
            {
                int r = UnityEngine.Random.Range(i, count);
                var tmp = list[i];
                list[i] = list[r];
                list[r] = tmp;
            }
        }

        public static T RandomPick<T>(List<T> list)
        {
            int pick = UnityEngine.Random.Range(0, list.Count);
            return list[pick];
        }

        public static float RoundFloat(float v, int c = 2)
        {
            return (float)Mathf.RoundToInt(v * (Mathf.Pow(10, c))) / Mathf.Pow(10, c);
        }

        public static Vector3 GetVelocity(float angle, float power)
        {
            Vector3 velocity = new Vector3();
            velocity.x = Mathf.Cos(angle * Mathf.PI / 180.0f) * power;
            velocity.y = Mathf.Sin(angle * Mathf.PI / 180.0f) * power;
            return velocity;
        }

        public static string GetEnumDescription(Enum value)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])value.GetType().GetField(value.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static Color ColorFromHTMLString(string htmlColor)
        {
            Color color = Color.white;
            ColorUtility.TryParseHtmlString(htmlColor, out color);
            return color;
        }


        private static readonly int charA = Convert.ToInt32('a');

        public static Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1)
        {
            return ((1 - t) * p0) + ((t) * p1);
        }

        public static Vector3 BezierCurve(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            Vector3 pa = BezierCurve(t, p0, p1);
            Vector3 pb = BezierCurve(t, p1, p2);
            return BezierCurve(t, pa, pb);
        }

        private static readonly Dictionary<int, string> units = new Dictionary<int, string>
    {
        {0, ""},
        {1, "K"},
        {2, "M"},
        {3, "B"},
        {4, "T"}
    };

        public static string FormatNumber(double value)
        {
            if (value < 1d)
            {
                return "0";
            }

            var n = (int)Math.Log(value, 1000);
            var m = value / Math.Pow(1000, n);
            var unit = "";

            if (n < units.Count)
            {
                unit = units[n];
            }
            else
            {
                var unitInt = n - units.Count;
                var secondUnit = unitInt % 26;
                var firstUnit = unitInt / 26;
                unit = Convert.ToChar(firstUnit + charA).ToString() + Convert.ToChar(secondUnit + charA).ToString();
            }

            // Math.Floor(m * 100) / 100) fixes rounding errors
            if (n == 0)
                return (Math.Floor(m * 100) / 100).ToString("0");
            else
                return (Math.Floor(m * 100) / 100).ToString("0.##") + unit;
        }
    }

    public static class JsonHelper
    {
        public static T FromJson<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }

        public static string ToJson<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
        public static string ToJson<T>(T obj, bool prettyPrint)
        {
            return JsonUtility.ToJson(obj, prettyPrint);
        }

        public static T[] ListFromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.items;
        }

        public static string ListToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.items = array;
            return JsonUtility.ToJson(wrapper);
        }
        public static string ListToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static string FixJson(string value)
        {
            value = "{\"items\":" + value + "}";
            return value;
        }

        [System.Serializable]
        private class Wrapper<T>
        {
            public T[] items;
        }
    }
}
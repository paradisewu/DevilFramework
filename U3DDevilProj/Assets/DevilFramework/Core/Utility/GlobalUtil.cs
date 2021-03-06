﻿using System.Collections.Generic;
using UnityEngine;

namespace Devil.Utility
{
    public static class GlobalUtil
    {
        public static float Pow(float b, int times)
        {
            if (b == 0)
                return 0;
            float t = 1;
            for (int i = 0; i < times; i++)
            {
                t *= b;
            }
            return t;
        }

        public static long Pow(long b, int times)
        {
            if (b == 0)
                return 0;
            long v = 1;
            for (int i = 0; i < times; i++)
            {
                v *= b;
            }
            return v;
        }

        public static Vector3 Mul(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }

        public static bool Insert<T>(ref T[] lst, int index, T value)
        {
            int len = lst == null ? 0 : lst.Length;
            if (index > len || index < 0)
                return false;
            var arr = new T[len + 1];
            if (index > 0)
                System.Array.Copy(lst, 0, arr, 0, index);
            if (index < len)
                System.Array.Copy(lst, index, arr, index + 1, len - index);
            arr[index] = value;
            lst = arr;
            return true;
        }

        public static bool RemoveAt<T>(ref T[] lst, int index)
        {
            int len = lst == null ? 0 : lst.Length;
            if (index < 0 || index >= len)
                return false;
            var arr = new T[len - 1];
            if (index > 0)
                System.Array.Copy(lst, 0, arr, 0, index);
            if (index < len - 1)
                System.Array.Copy(lst, index + 1, arr, index, len - 1 - index);
            lst = arr;
            return true;
        }

        public static int FindIndex<T>(IList<T> array, FilterDelegate<T> filter)
        {
            for (int i = 0; i < array.Count; i++)
            {
                if (filter(array[i]))
                    return i;
            }
            return -1;
        }

        public static T Find<T>(IList<T> array, FilterDelegate<T> filter)
        {
            for (int i = 0; i < array.Count; i++)
            {
                T t = array[i];
                if (filter(t))
                    return t;
            }
            return default(T);
        }

        public static T Find<T>(ICollection<T> collection, FilterDelegate<T> filter)
        {
            IEnumerator<T> iter = collection.GetEnumerator();
            while (iter.MoveNext())
            {
                if (filter(iter.Current))
                    return iter.Current;
            }
            return default(T);
        }

        public static bool InsertBySortId<T>(IList<T> list, T value, GetterDelegate<T, int> compareResult)
        {
            var index = BinsearchFromRightIndex(list, compareResult);
            if(index == -1)
            {
                list.Add(value);
                return true;
            }
            else if(compareResult(list[index]) == 0)
            {
                return false;
            }
            else
            {
                list.Insert(index, value);
                return true;
            }
        }

        public static bool InsertBySortId<T>(IList<T> list, T value) where T : IIdentified
        {
            var index = BinsearchFromRightIndex(list, value.Identify);
            if (index == -1)
            {
                list.Add(value);
                return true;
            }
            else if (list[index].Identify == value.Identify)
            {
                return false;
            }
            else
            {
                list.Insert(index, value);
                return true;
            }
        }
        
        public static T Binsearch<T>(IList<T> list, int id) where T : IIdentified
        {
            int index = BinsearchIndex(list, id);
            return index == -1 ? default(T) : list[index];
        }

        public static T Binsearch<T>(IList<T> list, GetterDelegate<T,int> compareResult)
        {
            int index = BinsearchIndex(list, compareResult);
            return index == -1 ? default(T) : list[index];
        }

        public static int BinsearchIndex<T>(IList<T> list, int id) where T : IIdentified
        {
            return BinsearchIndex(list, (x) => x.Identify < id ? -1 : (x.Identify > id ? 1 : 0), 0, list.Count);
        }
        
        public static int BinsearchIndex<T>(IList<T> list, GetterDelegate<T, int> compareResult)
        {
            return BinsearchIndex(list, compareResult, 0, list.Count);
        }

        public static int BinsearchIndex<T>(IList<T> list, GetterDelegate<T, int> compareResult, int start, int end)
        {
            int l = start;
            int r = end - 1;
            int c;
            int cs;
            while (l <= r)
            {
                c = (l + r) >> 1;
                cs = compareResult(list[c]);
                if (cs == 0)
                    return c;
                else if (cs > 0)
                    r = c - 1;
                else
                    l = c + 1;
            }
            return -1;
        }

        public static T BinsearchFromRight<T>(IList<T> list, int id) where T : IIdentified
        {
            int index = BinsearchFromRightIndex(list, id);
            return index == -1 ? default(T) : list[index];
        }

        public static int BinsearchFromRightIndex<T>(IList<T> list, int id) where T : IIdentified
        {
            return BinsearchFromRightIndex(list, (x) => x.Identify < id ? -1 : (x.Identify > id ? 1 : 0), 0, list.Count);
        }

        public static int BinsearchFromRightIndex<T>(IList<T> list, GetterDelegate<T, int> compareResult)
        {
            return BinsearchFromRightIndex(list, compareResult, 0, list.Count);
        }

        /// <summary>
        /// 从右边查找最接近目标 id 的对象 (查找结果 ≧ 目标)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="compareResult"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int BinsearchFromRightIndex<T>(IList<T> list, GetterDelegate<T, int> compareResult, int start, int end)
        {
            int l = start;
            int r = end - 1;
            int c = l;
            int cs;
            int ret = -1;
            while (l <= r)
            {
                c = (l + r) >> 1;
                cs = compareResult(list[c]);
                if (cs == 0)
                {
                    return c;
                }
                else if (cs > 0)
                {
                    r = c - 1;
                    ret = c;
                }
                else
                {
                    l = c + 1;
                }
            }
            return ret;
        }

        public static T BinsearchFromLeft<T>(IList<T> list, int id) where T : IIdentified
        {
            int index = BinsearchFromLeftIndex(list, id);
            return index == -1 ? default(T) : list[index];
        }

        public static int BinsearchFromLeftIndex<T>(IList<T> list, int id) where T : IIdentified
        {
            return BinsearchFromLeftIndex(list, (x) => x.Identify < id ? -1 : (x.Identify > id ? 1 : 0), 0, list.Count);
        }

        public static int BinsearchFromLeftIndex<T>(IList<T> list, GetterDelegate<T, int> compareResult)
        {
            return BinsearchFromRightIndex(list, compareResult, 0, list.Count);
        }

        /// <summary>
        /// 从左边查找最接近目标 id 的对象 (查找结果 ≦ 目标)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="compareResult"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int BinsearchFromLeftIndex<T>(IList<T> list, GetterDelegate<T, int> compareResult, int start, int end)
        {
            int l = start;
            int r = end - 1;
            int c = l;
            int cs;
            int ret = -1;
            while (l <= r)
            {
                c = (l + r) >> 1;
                cs = compareResult(list[c]);
                if (cs == 0)
                {
                    return c;
                }
                else if (cs > 0)
                {
                    r = c - 1;
                }
                else
                {
                    l = c + 1;
                    ret = c;
                }
            }
            return ret;
        }

        public static void RandomSort<T>(IList<T> array)
        {
            T tmp;
            for (int i = array.Count - 1; i > 0; i--)
            {
                var r = Mathf.RoundToInt(Random.value * i);
                tmp = array[r];
                array[r] = array[i];
                array[i] = tmp;
            }
        }

        public static void Sort<T>(IList<T> array, System.Comparison<T> compare)
        {
            for (int i = 0; i < array.Count; i++)
            {
                for (int j = i + 1; j < array.Count; j++)
                {
                    T a = array[i];
                    T b = array[j];
                    if (compare(a, b) > 0)
                    {
                        array[i] = b;
                        array[j] = a;
                    }
                }
            }
        }

        public static void Sort<T>(LinkedList<T> array, System.Comparison<T> compare)
        {
            var first = array.First;
            if (first == null)
                return;
            var current = first.Next;
            while (current != null)
            {
                var next = current.Next;
                var prev = current.Previous;
                while (prev != null)
                {
                    if (compare(prev.Value, current.Value) <= 0)
                        break;
                    prev = prev.Previous;
                }
                if (prev == null)
                {
                    array.Remove(current);
                    array.AddFirst(current);
                    first = current;
                }
                else if (prev.Next != current)
                {
                    array.Remove(current);
                    array.AddAfter(prev, current);
                }
                current = next;
            }
        }

        /// <summary>
        /// 计算以normal为发现确定的平面上，从from旋转到to的旋转方向
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static float RotateDirection(Vector3 from, Vector3 to, Vector3 normal)
        {
            Vector3 projFrom = Vector3.ProjectOnPlane(from, normal);
            Vector3 projTo = Vector3.ProjectOnPlane(to, normal);
            Vector3 cross = Vector3.Cross(projFrom, projTo);
            float dir = Vector3.Dot(cross, normal);
            return Mathf.Sign(dir);
        }

        public static float RotateDirectionOnPlane(Vector3 from, Vector3 to, Vector3 normal)
        {
            var cross = Vector3.Cross(from, to);
            return Mathf.Sin(Vector3.Dot(cross, normal));
        }

        /// <summary>
        /// 判断点p在平面前面否
        /// </summary>
        /// <param name="p"></param>
        /// <param name="pointInPlane"></param>
        /// <param name="planeNormal"></param>
        /// <returns></returns>
        public static bool IsFront(Vector3 p, Vector3 pointInPlane, Vector3 planeNormal)
        {
            var dir = p - pointInPlane;
            return Vector3.Dot(dir, planeNormal) >= 0;
        }

        public static bool IsBack(Vector3 p, Vector3 pointInPlane, Vector3 planeNormal)
        {
            var dir = p - pointInPlane;
            return Vector3.Dot(dir, planeNormal) < 0;
        }

        // not normalized
        public static Vector3 TopNormalDir(FrustumPlanes frustum)
        {
            return new Vector3(0,
                frustum.zFar * frustum.right - frustum.zFar * frustum.left, 
                frustum.left * frustum.top - frustum.right * frustum.top);
        }

        public static Vector3 DownNormalDir(FrustumPlanes frustum)
        {
            return new Vector3(0,
                frustum.zFar * frustum.left - frustum.zFar * frustum.right,
                frustum.right * frustum.bottom - frustum.left * frustum.bottom);
        }
         
        public static Vector3 RightNormalDir(FrustumPlanes frustum)
        {
            return new Vector3(frustum.zFar * frustum.top - frustum.zFar * frustum.bottom,
                0,
                frustum.right * frustum.bottom - frustum.right * frustum.top);
        }

        public static Vector3 LeftNormalDir(FrustumPlanes frustum)
        {
            return new Vector3(frustum.zFar * frustum.bottom - frustum.zFar * frustum.top,
                0,
                frustum.left * frustum.top - frustum.left * frustum.bottom);
        }

        public static bool IsInFrustum(Vector3 p, FrustumPlanes frustum)
        {
            if (p.z <= frustum.zNear || p.z >= frustum.zFar)
                return false;
            var nor = TopNormalDir(frustum);
            if (Vector3.Dot(p, nor) >= 0)
                return false;
            nor = DownNormalDir(frustum);
            if (Vector3.Dot(p, nor) >= 0)
                return false;
            nor = RightNormalDir(frustum);
            if (Vector3.Dot(p, nor) >= 0)
                return false;
            nor = LeftNormalDir(frustum);
            if (Vector3.Dot(p, nor) >= 0)
                return false;
            return true;
        }

        public static bool IsInFrustum(Vector3 p, float zFar, float zNear, float x, float y)
        {
            if (p.z <= zNear || p.z >= zFar)
                return false;
            var nor = new Vector3(0,
                zFar * x * 2,
                -2 * x * y);
            if (Vector3.Dot(p, nor) >= 0)
                return false;
            nor = new Vector3(0,
                -2 * zFar * x,
                -2 * x * y);
            if (Vector3.Dot(p, nor) >= 0)
                return false;
            nor = new Vector3(zFar * y * 2,
                0,
               -2 * x * y);
            if (Vector3.Dot(p, nor) >= 0)
                return false;
            nor = new Vector3(-2 * zFar * y,
                0,
                -2 * x * y);
            if (Vector3.Dot(p, nor) >= 0)
                return false;
            return true;
        }

        /// <summary>
        /// 计算以normal为法线确定的平面上，从from旋转到to的旋转角度
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static float RotateAngleFromTo(Vector3 from, Vector3 to, Vector3 normal)
        {
            Vector3 projFrom = Vector3.ProjectOnPlane(from, normal);
            Vector3 projTo = Vector3.ProjectOnPlane(to, normal);
            Vector3 cross = Vector3.Cross(projFrom, projTo);
            float dir = Vector3.Dot(cross, normal);
            return Mathf.Sign(dir) * Vector3.Angle(projFrom, projTo);
        }

        public static float RotateAngleFromToOnPlane(Vector3 from, Vector3 to, Vector3 normal)
        {
            var cross = Vector3.Cross(from, to);
            var dir = Vector3.Dot(cross, normal);
            return Mathf.Sign(dir) * Vector3.Angle(from, to);
        }

        /// <summary>
        /// 计算射线与平面的交点
        /// </summary>
        /// <param name="original"></param>
        /// <param name="direction"></param>
        /// <param name="anyPointInPlane"></param>
        /// <param name="planeNormal"></param>
        /// <returns></returns>
        public static Vector3 CalculateIntersectionPoint(Vector3 original, Vector3 direction, Vector3 anyPointInPlane, Vector3 planeNormal)
        {
            float dot2 = Vector3.Dot(direction, planeNormal);
            if (dot2 == 0)
                return anyPointInPlane;
            Vector3 normal;
            if (dot2 < 0)
            {
                normal = -planeNormal.normalized;
                dot2 = -dot2;
            }
            else
            {
                normal = planeNormal.normalized;
            }
            Vector3 p = anyPointInPlane - original;
            float dot1 = Vector3.Dot(normal, p);
            if (dot1 == 0)
                return original;
            Vector3 oo1 = dot1 * normal;
            Vector3 oo2 = dot2 * normal;
            return original + oo1 + (dot1 / dot2) * (direction - oo2);
        }

        public static bool GetIntersectionPointInXOZ(Vector3 original, Vector3 direction, out Vector3 point)
        {
            if (direction.y == 0)
            {
                point = original;
                return original.y == 0;
            }
            if (direction.y * original.y > 0)
            {
                point = original;
                return false;
            }
            float dot = Vector3.Dot(direction, Vector3.up);
            Vector3 p = original;
            p.y = 0;
            point = p - (direction - dot * Vector3.up) * original.y / dot;
            return true;
        }

        public static Vector3 AddMagnitude(Vector3 v, float magnitude)
        {
            float m = v.magnitude;
            float ma = m + magnitude;
            if (ma <= 0 || m == 0)
                return Vector3.zero;
            else
                return v * ma / m;
        }

        public static Vector3 CloseTo(Vector3 from, Vector3 to, float strength, float devition = 0.001f)
        {
            Vector3 v = Vector3.Lerp(from, to, strength);
            if (Vector3.Distance(v, to) <= devition)
            {
                return to;
            }
            else
            {
                return v;
            }
        }

        public static int GetBitIndex(uint value)
        {
            if (value == 0)
                return -1;
            int l = 0;
            int r = 32;
            int n = 16;
            uint v = value >> n;
            while (v != 1)
            {
                if (v == 0)
                {
                    r = n - 1;
                    n = (l + r) >> 1;
                }
                else
                {
                    l = n + 1;
                    n = (l + r) >> 1;
                }
                v = value >> n;
            }
            return n;
        }

        public static bool IsValid(AnimationCurve curve)
        {
            return curve != null && curve.length > 1;
        }

        public static float GetMinTime(AnimationCurve curve)
        {
            return curve.keys[0].time;
        }

        public static float GetMaxTime(AnimationCurve curve)
        {
            return curve.keys[curve.length - 1].time;
        }

        public static float ClampTime(AnimationCurve curve, float time)
        {
            return Mathf.Clamp(time, GetMinTime(curve), GetMaxTime(curve));
        }

        public static float GetEndValue(AnimationCurve curve)
        {
            return curve.keys[curve.length - 1].value;
        }

        public static float GetStartValue(AnimationCurve curve)
        {
            return curve.keys[0].value;
        }

        public static float GetNormalizedValue(AnimationCurve curve, float t)
        {
            float tmin = GetMinTime(curve);
            float tmax = GetMaxTime(curve);
            float lerp = Mathf.Lerp(tmin, tmax, t);
            float v = curve.Evaluate(lerp);
            tmin = GetStartValue(curve);
            tmax = GetEndValue(curve);
            float len = Mathf.Abs(tmax - tmin);
            if (tmin > tmax)
                tmin = tmax;
            return len == 0 ? v : (v - tmin) / len;
        }

        public static void QuitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

#if UNITY_EDITOR
using System.Reflection;
#endif

public enum NetworkLoading
{
    Loading,
    Ready
}

public static class StaticVariable
{
    public static bool isLoaded = false;
    public static readonly string DATA_PLAYER = "player.data";
    public static readonly string DATA_SOUND = "sound.data";

    public static void ClearLog()
    {
#if UNITY_EDITOR
        var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
        var type = assembly.GetType("UnityEditor.LogEntries");
        var method = type.GetMethod("Clear");
        method.Invoke(new object(), null);
#endif
    }

    public static float GetKeyboardHeight()
    {
#if UNITY_EDITOR
        return 0;
#elif UNITY_ANDROID
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var view = unityClass.GetStatic<AndroidJavaObject>("currentActivity")
                .Get<AndroidJavaObject>("mUnityPlayer")
                .Call<AndroidJavaObject>("getView");

            var dialog = unityClass.GetStatic<AndroidJavaObject>("currentActivity")
                .Get<AndroidJavaObject>("mUnityPlayer")
                .Get<AndroidJavaObject>("b");

            var decorView = dialog.Call<AndroidJavaObject>("getWindow")
                .Call<AndroidJavaObject>("getDecorView");

            var height = decorView.Call<int>("getHeight");

            using (var rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                return (float)(Screen.height - rect.Call<int>("height") + height) /*/ Screen.height*/;
            }
        }
#elif UNITY_IOS
        return (float) TouchScreenKeyboard.area.height /*/ Screen.height*/;
#endif
    }

    public static void SetLayer(this Transform target, int layer)
    {
        target.gameObject.SetLayer(layer);
    }

    public static void SetLayer(this GameObject target, int layer)
    {
        target.layer = layer;
        target.ForChild((_child) =>
        {
            _child.layer = layer;
        });
    }

    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static void Shuffle<T>(this T[] ts)
    {
        var count = ts.Length;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static T[] Add<T>(this T[] target, T item)
    {
        if (target == null)
        {
            return null;
        }

        T[] result = new T[target.Length + 1];
        target.CopyTo(result, 0);
        result[target.Length] = item;
        return result;
    }

    public static T[] Remove<T>(this T[] target, T item)
    {
        if (target == null)
        {
            return null;
        }

        List<T> lst = new List<T>();
        for(int i = 0; i < target.Length; i++)
        {
            if (Compare(target[i], item))
                continue;
            lst.Add(target[i]);
        }
        return lst.ToArray();
    }

    public static T[] OnlyAdd<T>(this T[] target, T item)
    {
        if (target == null)
        {
            return null;
        }

        for (int i = 0; i < target.Length; i++)
            if (Compare(target[i], item))
                return target;

        T[] result = new T[target.Length + 1];
        target.CopyTo(result, 0);
        result[target.Length] = item;
        return result;
    }

    public static bool Compare<T>(T x, T y)
    {
        return EqualityComparer<T>.Default.Equals(x, y);
    }

    public static void HideAllChild(this Transform target)
    {
        for (int i = 0; i < target.childCount; i++)
        {
            if (target.GetChild(i).IsActive())
                target.GetChild(i).SetActive(false);
        }
    }

    public static bool IsChildActive(this Transform target)
    {
        for(int i = 0; i < target.childCount; i++)
        {
            if (target.GetChild(i).IsActive())
                return true;
        }
        return false;
    }

    public static bool IsActive(this Transform target)
    {
        if (target == null)
            return false;

        bool result = false;

        result = target.gameObject.activeInHierarchy;

        return result;
    }

    public static bool IsActive(this GameObject target)
    {
        if (target == null)
            return false;

        bool result = false;

        result = target.gameObject.activeInHierarchy;

        return result;
    }

    public static T GetOnce<T>(this T[] target)
    {
        var i = UnityEngine.Random.Range(0, target.Length);
        return target[i];
    }

    internal static Transform FindChildByRecursion(this Transform aParent, string aName)
    {
        if (aParent == null) return null;    
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindChildByRecursion(aName);
            if (result != null)
                return result;
        }
        return null;
    }

    internal static T FindChildByRecursion<T>(this Transform aParent)
    {
        if (aParent == null) return default;
        var result = aParent.GetComponent<T>();
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindChildByRecursion<T>();
            if (result != null)
                return result;
        }
        return default;
    }

    internal static Transform FindChildByParent(this Transform aParent, string aName)
    {
        if (aParent == null) return null;
        for(int i = 0; i < aParent.childCount; i++)
        {
            var child = aParent.GetChild(i);
            if (child.name == aName)
                return child;
        }
        return null;
    }

    internal static Transform FindChildByParent(this GameObject aParent, string aName)
    {
        if (aParent == null) return null;
        for (int i = 0; i < aParent.transform.childCount; i++)
        {
            var child = aParent.transform.GetChild(i);
            if (child.name == aName)
                return child;
        }
        return null;
    }

    public static string AddSpace(this string target)
    {
        string input = target;
        string output = System.Text.RegularExpressions.Regex.Replace(input, "(\\B[A-Z])", " $1");
        return output;
    }

    internal static bool IsChild(this Transform aParent, string aName)
    {
        if (aParent == null) return false;
        for (int i = 0; i < aParent.childCount; i++)
        {
            var child = aParent.GetChild(i);
            if (child.name == aName)
                return true;
        }
        return false;
    }

    public static bool IsContain<T>(this T[] target, T input)
    {
        if (target == null)
            return false;

        for(int i = 0; i < target.Length; i++)
        {
            if (target[i].Equals(input))
                return true;
        }
        return false;
    }

    internal static void SetActive(this Transform _object, bool _active)
    {
        if (_object == null)
            return;
        GameObject _rp = _object.gameObject;
        _rp.SetActive(_active);
    }

    public static Vector2 travelAlongCircle(this Vector2 pos, Vector2 center, float distance)
    {
        Vector3 axis = Vector3.back;
        Vector2 dir = pos - center;
        float circumference = 2.0f * Mathf.PI * dir.magnitude;
        float angle = distance / circumference * 360.0f;
        dir = Quaternion.AngleAxis(angle, axis) * dir;
        return dir + center;
    }

    public static Vector3 PositionInCircumference(Vector3 center, float radius, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(radians);
        float z = Mathf.Sin(radians);
        Vector3 position = new Vector3(x, center.y, z);
        position *= radius;
        position += center;

        return position;
    }

    public static Vector3 GetCirclePosition(double radius, Vector3 center)
    {
        int point = UnityEngine.Random.RandomRange(5, 10);
        double slice = 2 * Math.PI / point;
        double angle = slice * UnityEngine.Random.RandomRange(0, point);
        int newX = (int)(center.x + radius * Math.Cos(angle));
        int newY = (int)(center.y + radius * Math.Sin(angle));

        return new Vector3(newX, newY);
    }

    public static Vector3 GetRandomDirection()
    {
        Vector3 rotatedVector = Quaternion.AngleAxis(UnityEngine.Random.RandomRange(0f, 360f), Vector3.up) * Vector3.forward;
        return rotatedVector;
    }

    private static string[] Months = new string[] { "Jan", "Feb", "Mar",
                                                  "Apr", "May", "Jun",
                                                  "Jul", "Aug", "Sep",
                                                  "Oct", "Nov", "Dec"};
    public static string ConvertMonthIntToString(int Month)
    {
        return Months[Month - 1];
    }

    public static DayOfWeek ConvertDateTimeToDayOfWeek(DateTime dateTime)
    {
        return dateTime.DayOfWeek;
    }

    public static DayOfWeek ConvertDateTimeToDayOfWeek(int year, int month, int day)
    {
        DateTime dt = new DateTime(year, month, day);
        return dt.DayOfWeek;
    }

    public static string ConvertMonthIntToString(string Month)
    {
        int _Month = int.Parse(Month);
        return ConvertMonthIntToString(_Month);
    }

    public static void OpenUrl(string url)
    {
#if UNITY_EDITOR
        Application.OpenURL(url);
#elif UNITY_WEBGL
            string _script = string.Format("window.open('{0}', '_blank')", url);
            Application.ExternalEval(_script);
#else
            Application.OpenURL(url);
#endif
    }
}

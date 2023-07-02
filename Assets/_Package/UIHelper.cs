using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class UIHelper : MonoSingleton<UIHelper>
{
    public static string MenuSelected;

    public static Type GetActive()
    {
        for (int i = 0; i < Instance.transform.childCount; i++)
        {
            var _child = Instance.transform.GetChild(i);
            var _childScript = _child.GetComponent<UICanvas>();
            if (_childScript != null)
            {
                if (!_child.IsActive())
                    continue;

                return _childScript.GetType();
            }
        }
        return default;
    }

    public static void HideAll()
    {
        MenuSelected = "";
        for (int i = 0; i < Instance.transform.childCount; i++)
        {
            var _child = Instance.transform.GetChild(i);
            var _childScript = _child.GetComponent<UICanvas>();
            if (_childScript != null)
            {
                if (!_child.IsActive())
                    continue;

                var _canvas = _child.GetComponent<UICanvas>();
                if (_canvas != null)
                    _canvas.Hide();
            }
        }
    }

    private GenericDictionary dictionary = new GenericDictionary();

    public static T FindScript<T>()
    {
        //StaticVariable.ClearLog();
        MenuSelected = typeof(T).ToString();
        if (Instance.dictionary.Exists<T>(MenuSelected))
        {
          return Instance.dictionary.GetValue<T>(MenuSelected);
        }
        else
        {
            var uiObjects = Resources.LoadAll<GameObject>("UI/Screen");
            for (int i = 0; i < uiObjects.Length; i++)
            {
                var uiObjectPrefab = uiObjects[i].GetComponent<T>();
                if (uiObjectPrefab == null)
                    continue;

                var uiObject = Instantiate(uiObjects[i], Instance.transform);
                uiObject.name = uiObject.name.Replace("(Clone)", "");
                uiObject.transform.SetAsFirstSibling();
                var uiObjectScript = uiObject.GetComponent<T>();
                Instance.dictionary.Add(MenuSelected, uiObjectScript);
            }
            return Instance.dictionary.GetValue<T>(MenuSelected);
        }
    }

    public static T FindOnceScript<T>()
    {
        MenuSelected = typeof(T).ToString();
        T response = default(T);
        for (int i = 0; i < Instance.transform.childCount; i++)
        {
            var _child = Instance.transform.GetChild(i);
            var _childScript = _child.GetComponent<T>();
            if (_childScript != null)
                response = _childScript;
            else
            {
                if (!_child.IsActive())
                    continue;

                var _canvas = _child.GetComponent<UICanvas>();
                if (_canvas != null)
                    _canvas.Hide();
            }
        }

        return response;
    }
}

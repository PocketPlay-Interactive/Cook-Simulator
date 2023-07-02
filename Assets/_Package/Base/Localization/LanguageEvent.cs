using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageEvent
{
    public delegate void onChangeLanguage();
    public static onChangeLanguage OnChangeLanguage;
    public static void OnChangeLanguageMethod()
    {
        if (OnChangeLanguage != null)
            OnChangeLanguage?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SyncSaveFiles : MonoBehaviour
{
    void Awake()
    {
		Debug.Log(ES3Settings.defaultSettings.path);
		ES3AutoSaveMgr.Current.settings.path = ES3Settings.defaultSettings.path;
    }
}

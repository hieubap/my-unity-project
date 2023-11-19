using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine;

public class NativeAPI {
#if UNITY_IOS && !UNITY_EDITOR
    [DllImport("__Internal")]
    public static extern void sendMessageToMobileApp(string message);
#endif
}

public class EventToJS{
    public static string Exit = "Exit";
}

public class NativeBridgeCall : Singleton<NativeBridgeCall>
{
    public Text titleText;

    public void SetTitle(string titleChange){
        if(titleText)
        titleText.text = titleChange;
    }

    public void OnBack(){
        SendToJS(EventToJS.Exit);
    }

    public void ReceivedFromJS(string type, object payload){

    }

    public void SendToJS(string payload)
    {
        Debug.Log("Send to JS");
        if (Application.platform == RuntimePlatform.Android)
        {
            using (AndroidJavaClass jc = new AndroidJavaClass("com.azesmwayreactnativeunity.ReactNativeUnityViewManager"))
            {
                jc.CallStatic("sendMessageToMobileApp", payload);
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
#if UNITY_IOS && !UNITY_EDITOR
            NativeAPI.sendMessageToMobileApp(payload);
#endif
        }
    }
}

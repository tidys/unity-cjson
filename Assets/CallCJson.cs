using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
public class CallCJson : MonoBehaviour
{
 [StructLayout(LayoutKind.Sequential)]
 public struct CJson
    {
        public IntPtr next;
        public IntPtr prev;
        public IntPtr child;
        public int type;
        public IntPtr valueString;
        public int valueint;
        public double valuedouble;
        public IntPtr str;
    }
    [DllImport ("cjson")]
    private static extern IntPtr cJSON_Version();
    [DllImport("cjson")]
    private static extern IntPtr cJSON_Parse(IntPtr value); // cjson中指针类型的统一都声明为IntPtr，因为指针就是一个int值
    [DllImport("cjson")]
    private static extern IntPtr cJSON_GetObjectItemCaseSensitive(IntPtr obj, IntPtr str);
    [DllImport("cjson")]
    private static extern bool cJSON_IsString(IntPtr obj);
    void Start()
    {
        // 最简单的例子
        IntPtr ret = cJSON_Version();
        string strRet = Marshal.PtrToStringAnsi(ret);
        UnityEngine.Debug.Log("using cjson version: " + strRet);


        string jsonString = "{\"test\":\"abc\"}";
        // 将string转换为IntPtr后作为参数入参
        IntPtr jsonObject = cJSON_Parse(Marshal.StringToHGlobalAnsi(jsonString));

        // 获取json.test对象
        IntPtr testObject = cJSON_GetObjectItemCaseSensitive(jsonObject, Marshal.StringToHGlobalAnsi("test"));
        bool isString = cJSON_IsString(testObject);
        if (isString)
        {
            // 将test对象进行解析为结构体，是因为c++层cJSON_GetObjectItemCaseSensitive的返回值就是结构体指针
            CJson jsonObj = (CJson)Marshal.PtrToStructure(testObject, typeof(CJson));
            // 是字符串类型，就将IntPtr转换为c#的string
            string stringValue = Marshal.PtrToStringAnsi(jsonObj.valueString);
            UnityEngine.Debug.Log("test: " + stringValue);
        }

    }
 

}

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
    private static extern IntPtr cJSON_Parse(IntPtr value); // cjson��ָ�����͵�ͳһ������ΪIntPtr����Ϊָ�����һ��intֵ
    [DllImport("cjson")]
    private static extern IntPtr cJSON_GetObjectItemCaseSensitive(IntPtr obj, IntPtr str);
    [DllImport("cjson")]
    private static extern bool cJSON_IsString(IntPtr obj);
    void Start()
    {
        // ��򵥵�����
        IntPtr ret = cJSON_Version();
        string strRet = Marshal.PtrToStringAnsi(ret);
        UnityEngine.Debug.Log("using cjson version: " + strRet);


        string jsonString = "{\"test\":\"abc\"}";
        // ��stringת��ΪIntPtr����Ϊ�������
        IntPtr jsonObject = cJSON_Parse(Marshal.StringToHGlobalAnsi(jsonString));

        // ��ȡjson.test����
        IntPtr testObject = cJSON_GetObjectItemCaseSensitive(jsonObject, Marshal.StringToHGlobalAnsi("test"));
        bool isString = cJSON_IsString(testObject);
        if (isString)
        {
            // ��test������н���Ϊ�ṹ�壬����Ϊc++��cJSON_GetObjectItemCaseSensitive�ķ���ֵ���ǽṹ��ָ��
            CJson jsonObj = (CJson)Marshal.PtrToStructure(testObject, typeof(CJson));
            // ���ַ������ͣ��ͽ�IntPtrת��Ϊc#��string
            string stringValue = Marshal.PtrToStringAnsi(jsonObj.valueString);
            UnityEngine.Debug.Log("test: " + stringValue);
        }

    }
 

}

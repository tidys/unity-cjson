using UnityEngine;
using System.Runtime.InteropServices;
using System;
public class CallCJson : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    public struct _CJson
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
    [DllImport("cjson")]
    private static extern bool cJSON_IsNumber(IntPtr obj);
    [DllImport("cjson")]
    private static extern bool cJSON_IsObject(IntPtr obj);
    [DllImport("cjson")]
    private static extern bool cJSON_IsArray(IntPtr obj);
    [DllImport("cjson")]
    private static extern int cJSON_GetArraySize(IntPtr obj);
    [DllImport("cjson")]
    private static extern IntPtr cJSON_GetArrayItem(IntPtr obj, int index);
    
    [DllImport("cjson")]
    private static extern double cJSON_GetNumberValue(IntPtr obj);
    [DllImport("cjson")]
    private static extern bool cJSON_IsInvalid(IntPtr obj);
    [DllImport("cjson")]
    private static extern IntPtr cJSON_GetStringValue(IntPtr obj);
    void Start()
    {
        // 最简单的例子
        IntPtr ret = cJSON_Version();
        string strRet = Marshal.PtrToStringAnsi(ret);
        UnityEngine.Debug.Log("using cjson version: " + strRet);

        string jsonString = "{\"test-string\":\"abc\",\"test-number\":99,\"obj\":{\"v1\":15}, \"arr\":[1,2,3] }";
        // 将string转换为IntPtr后作为参数入参
        IntPtr jsonObject = cJSON_Parse(Marshal.StringToHGlobalAnsi(jsonString));
        if (!cJSON_IsInvalid(jsonObject))
        {
            //return;
        }

        // 解析string
        // 获取json.test对象
        IntPtr testStringObject = cJSON_GetObjectItemCaseSensitive(jsonObject, Marshal.StringToHGlobalAnsi("test-string"));
        bool isString = cJSON_IsString(testStringObject);
        if (isString)
        {
            // 将test对象进行解析为结构体，是因为c++层cJSON_GetObjectItemCaseSensitive的返回值就是结构体指针
            _CJson jsonObj = (_CJson)Marshal.PtrToStructure(testStringObject, typeof(_CJson));
            // way1: 是字符串类型，就将IntPtr转换为c#的string
            string stringValue = Marshal.PtrToStringAnsi(jsonObj.valueString);
            UnityEngine.Debug.Log("test-string: " + stringValue);

            // way2:
            stringValue = Marshal.PtrToStringAnsi(cJSON_GetStringValue(testStringObject));
            UnityEngine.Debug.Log("test-string: " + stringValue);
        }

        // 解析number
        IntPtr testNumberObject = cJSON_GetObjectItemCaseSensitive(jsonObject, Marshal.StringToHGlobalAnsi("test-number"));
        bool isNumber = cJSON_IsNumber(testNumberObject);
        if (isNumber)
        {
            _CJson jsonObj = (_CJson)Marshal.PtrToStructure(testStringObject, typeof(_CJson));
            UnityEngine.Debug.Log("test-number:"+jsonObj.valuedouble);

            double value= cJSON_GetNumberValue(testNumberObject);
            UnityEngine.Debug.Log("test-number:" + value);

        }
        // 解析object
        IntPtr testObject = cJSON_GetObjectItemCaseSensitive(jsonObject, Marshal.StringToHGlobalAnsi("obj"));
        if (cJSON_IsObject(testObject))
        {
          IntPtr v1Ptr=  cJSON_GetObjectItemCaseSensitive(testObject, Marshal.StringToHGlobalAnsi("v1"));
            if (cJSON_IsNumber(v1Ptr))
            {
                double value = cJSON_GetNumberValue(v1Ptr);
                UnityEngine.Debug.Log("obj.v1:" + value);
            }
        }
        // 解析array
        IntPtr testArray = cJSON_GetObjectItemCaseSensitive(jsonObject, Marshal.StringToHGlobalAnsi("arr"));
        if (cJSON_IsArray(testArray))
        {
            int size = cJSON_GetArraySize(testArray);
            for (int i = 0; i < size; i++)
            {
                IntPtr ptr= cJSON_GetArrayItem(testArray, i);
                if (cJSON_IsNumber(ptr))
                {
                    UnityEngine.Debug.Log("arr[" + i + "]=" + cJSON_GetNumberValue(ptr));
                }
            }
        }

    }
    void test()
    {
        string jsonString = "{\"test-string\":\"abc\",\"test-number\":99,\"obj\":{\"v1\":15}, \"arr\":[1,2,3] }";
        // 封装后的使用
        CJson root = CJson.parse(jsonString);
        string str = root["test-string"].toString();
        double num = root["test-number"].toNumber();
        double v1 = root["obj"]["v1"].toNumber();

        CJson arr = root["arr"];
        double arr1 = arr[0].toNumber();
        double arr2 = arr[1].toNumber();
        double arr3 = arr[2].toNumber();
    }


}

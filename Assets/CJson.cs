using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
public class CJson
{
    [DllImport("cjson")]
    private static extern IntPtr cJSON_Version();
    [DllImport("cjson")]
    private static extern IntPtr cJSON_Parse(IntPtr value);  
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

    private IntPtr ptr;
    private CJson(string jsonString)
    {
        this.ptr = cJSON_Parse(Marshal.StringToHGlobalAnsi(jsonString));
    }
    private CJson(IntPtr ptr)
    {
        this.ptr = ptr;
    }
    public static CJson parse(string jsonString)
    {
        return new CJson(jsonString);
    }
   
    public string toString()
    {
        if (cJSON_IsString(this.ptr))
        {
            return Marshal.PtrToStringAnsi(cJSON_GetStringValue(this.ptr));
        }
        return "";
    }
    public double toNumber()
    {
        if (cJSON_IsNumber(this.ptr))
        {
            return cJSON_GetNumberValue(this.ptr);
        }
        return 0;
    }
    public CJson this[string str]
    {
        get
        {
            IntPtr itemPtr = cJSON_GetObjectItemCaseSensitive(this.ptr, Marshal.StringToHGlobalAnsi(str));
            return new CJson(itemPtr);
        }
    }
    public CJson this[int index]
    {
        get
        {
            if (cJSON_IsArray(this.ptr))
            {
                int size = cJSON_GetArraySize(this.ptr);
                if (index < size)
                {
                   return new CJson( cJSON_GetArrayItem(this.ptr, index));
                }
            }
            return null;
        }
    }
}

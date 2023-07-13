using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class ObfuscateAlgoritm
{
    public int VisibleToObfuscatedInt(int visibleInt)
    {
        int obfuscatedInt = visibleInt * 2;
        return obfuscatedInt;
    }

    public int ObfuscatedToVisibleInt(int obfuscatedInt)
    {
        int visibleInt = obfuscatedInt / 2;
        return visibleInt;
    }

    public float VisibleToObfuscatedFloat(float visibleFloat)
    {
        float obfuscatedFloat = visibleFloat * 2;
        return obfuscatedFloat;
    }

    public float ObfuscatedToVisibleFloat(float obfuscatedFloat)
    {
        float visibleFloat = obfuscatedFloat / 2;
        return visibleFloat;
    }
}

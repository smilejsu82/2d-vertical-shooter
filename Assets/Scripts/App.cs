
using System;
using UnityEngine;

public class App 
{
    //생성자 
    public App()
    {
        Debug.Log("App 클래스 생성자 호출됨");
        
        Test((name) => { });
    }

    public void Test(Action<string> callback)
    {
        callback("hong");
    }
}

using UnityEngine;
using UnityEngine.UI;

public class Grammar : MonoBehaviour
{
    public Button testButton;
    
    void Start()
    {
        testButton.onClick.AddListener(OnClickButton);
    }

    private void OnClickButton()
    {
        Debug.Log("Hello World");
    }
}

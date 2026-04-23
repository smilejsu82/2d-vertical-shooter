using UnityEngine;
using UnityEngine.UI;

public class Test3Main : MonoBehaviour
{
    public Button btn;
   
    public Enemy3 enemyAGo;
    
    void Start()
    {

        enemyAGo.onDie = (pos) =>
        {
            ItemManager3.Instance.CreateItem(pos);
        };


        btn.onClick.AddListener(() =>
        {
            Debug.Log("clicked!!");
            enemyAGo.TakeDamage(5);
        });    
    }
}

using Core;
using Items.Resource;
using UnityEditor;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    [SerializeField] private float _timeToGiveResource;
    private float _time;

    private void GiveResource(ResourcesScript.Types types)
    {
        GameManager.Instance.ResourcesManagerScript.AffectResource(types, 1);
    }
            
    private void RunLootTabe()
    {
        float randomNumber = Random.Range(0f, 100f);
            
        if (randomNumber > 40)
            GiveResource(ResourcesScript.Types.Stone);
            
        if (randomNumber < 40 && randomNumber > 20)
            GiveResource(ResourcesScript.Types.Iron_Ore);
            
        if (randomNumber < 20 && randomNumber > 3)
            GiveResource(ResourcesScript.Types.Gold_Ore);
            
        if (randomNumber < 3 && randomNumber > 0)
            GiveResource(ResourcesScript.Types.Diamond_Ore);
    }
        
    void Start()
    {
        float _time = _timeToGiveResource;
    }
        
    void Update()
    {
        if (_time > 0)
            _time -= Time.deltaTime;
        else
        {
            RunLootTabe();
            _time = _timeToGiveResource;
        }
    }
}
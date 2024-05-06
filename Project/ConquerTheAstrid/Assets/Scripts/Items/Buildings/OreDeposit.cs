using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using Items.Resource;
using UnityEngine;
using Random = UnityEngine.Random;

public class OreDeposit : MonoBehaviour, IInteractable
{
    [SerializeField] private ResourceSO _resourceSO;
    [SerializeField] private GameObject _mesh;
    [SerializeField] private float _resourceTimeToMine;

    private Vector3 _startPos;
    
    private void Start()
    {
        _startPos = _mesh.transform.position;
    }
    
    private void MineResouce()
    {
        int amountToGive = Random.Range(1, 8);
        
        GameManager.Instance.ResourcesManagerScript.AffectResource(_resourceSO.Type, amountToGive);
        Destroy(gameObject);
    }
    
    public void Focus()
    {
        
    }

    public void Unfocus()
    {
            
    }

    public void Select()
    {
        StartCoroutine(MineResourceRoutine());
    }

    public void Deselect()
    {
        
    }

    private IEnumerator MineResourceRoutine()
    {
        float time = 0.0f;

        while (time < _resourceTimeToMine)
        {
            time += Time.deltaTime;
            float sliderValue = time / _resourceTimeToMine;
            

            yield return null;
        }
        
        MineResouce();
    }

    private IEnumerator ShakeDepositRoutine()
    {
        float time = 0.0f;
 
        while (time < _resourceTimeToMine)
        {
            time += Time.deltaTime;
 
            Vector3 randomPos = _startPos + (Random.insideUnitSphere * 0.1f);
 
            transform.position = randomPos;
 
            if (0.2f > 0f)
            {
                yield return new WaitForSeconds(0.2f);
            }
            else
            {
                yield return null;
            }
        }
 
        transform.position = _startPos;
    }
}

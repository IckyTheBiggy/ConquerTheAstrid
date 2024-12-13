using System.Collections;
using System.Collections.Generic;
using Core;
using Items.Resource;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI : MonoBehaviour
{
    [SerializeField] private TMP_Text _woodAmountText;
    [SerializeField] private TMP_Text _stoneAmountText;
    [SerializeField] private TMP_Text _ironAmountText;
    [SerializeField] private TMP_Text _goldAmountText;
    [SerializeField] private TMP_Text _diamondAmountText;
    [SerializeField] private TMP_Text _moneyText;
    
    void Start()
    {
        
    }
    
    void Update()
    {
        //!!!TESTING CODE 
        DisplayUI();
    }

    private void DisplayUI()
    {
        _woodAmountText.text = GameManager.Instance.ResourcesManagerScript.FindResouce(ResourcesScript.Types.Wood).Amount.ToString();
        _stoneAmountText.text = GameManager.Instance.ResourcesManagerScript.FindResouce(ResourcesScript.Types.Stone).Amount.ToString();
        _ironAmountText.text = GameManager.Instance.ResourcesManagerScript.FindResouce(ResourcesScript.Types.Iron).Amount.ToString();
        _goldAmountText.text = GameManager.Instance.ResourcesManagerScript.FindResouce(ResourcesScript.Types.Gold).Amount.ToString();
        _diamondAmountText.text = GameManager.Instance.ResourcesManagerScript.FindResouce(ResourcesScript.Types.Diamond).Amount.ToString();
        _moneyText.text = GameManager.Instance.ResourcesManagerScript.FindResouce(ResourcesScript.Types.Money).Amount.ToString();
    }
}

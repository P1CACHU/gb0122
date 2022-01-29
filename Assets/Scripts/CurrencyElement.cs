using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyElement : MonoBehaviour
{
    [SerializeField] private Text _text;

    public void SetCurrency(KeyValuePair<string, int> currency)
    {
        _text.text = $"{GetCurrencyNameByKeyText(currency.Key)}: {currency.Value}";
    }

    private string GetCurrencyNameByKeyText(string key)
    {
        return key switch
        {
            "HC" => "Gold",
            "SC" => "Chip",
            _ => String.Empty
        };
    }
}
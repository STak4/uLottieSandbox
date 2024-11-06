using System.Collections;
using System.Collections.Generic;
using Gilzoide.LottiePlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LottieDropdownView : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private List<CustomLottieAnimationAsset> lottieAssets;

    public CustomLottieAnimationAsset Current => lottieAssets[dropDown.value];

    // Start is called before the first frame update
    void Start()
    {
        UpdateView();
    }

    public void UpdateView()
    {
        var lotties = new List<string>();
        foreach (var asset in lottieAssets)
        {
            lotties.Add(asset.jsonFile.name);
        }
        SetOptions(lotties);
    }

    public void SetOptions(List<string> options)
    {
        dropDown.ClearOptions();
        dropDown.AddOptions(options);
    }
}

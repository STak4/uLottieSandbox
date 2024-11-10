using System.Collections;
using System.Collections.Generic;
using System.IO;
using Gilzoide.LottiePlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LottieDropdownView : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropDown;
    [SerializeField] private LottieJsonList lottieJsons;

    public string Current =>Path.Combine(Application.streamingAssetsPath, lottieJsons.JsonFiles[dropDown.value]);

    // Start is called before the first frame update
    void Start()
    {
        UpdateView();
    }

    public void UpdateView()
    {
        var lotties = new List<string>();
        foreach (var path in lottieJsons.JsonFiles)
        {
            lotties.Add(Path.GetFileNameWithoutExtension(path));
        }
        SetOptions(lotties);
    }

    public void SetOptions(List<string> options)
    {
        dropDown.ClearOptions();
        dropDown.AddOptions(options);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.IO;
using Gilzoide.LottiePlayer;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LottieDropdownView : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown fileDropdown;
    [SerializeField] private TMP_Dropdown sizeDropdown;
    [SerializeField] private LottieJsonList lottieJsons;

    public string CurrentFile =>Path.Combine(Application.streamingAssetsPath, lottieJsons.JsonFiles[fileDropdown.value]);

    public int CurrentSize
    {
        get
        {
            if(int.TryParse(sizeDropdown.options[sizeDropdown.value].text, out int size))
            {
                return size;
            }
            else
            {
                return -1;
            }
        }
    }

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
        fileDropdown.ClearOptions();
        fileDropdown.AddOptions(options);
    }
}

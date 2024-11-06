using System.Collections;
using System.Collections.Generic;
using Gilzoide.LottiePlayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LottieDemo : MonoBehaviour
{
    [Header("Prefab settings")]
    [SerializeField] private CustomImageLottiePlayer playerPrefab;
    [SerializeField] private Transform playerParent;

    [Header("Views")]
    [SerializeField] private Button addButton;
    [SerializeField] private Button removeButton;

    [SerializeField] private LottieDropdownView dropDown;
    [SerializeField] private TMP_Text countText;
    
    // Start is called before the first frame update
    void Start()
    {
        addButton.onClick.AddListener(Add);
        removeButton.onClick.AddListener(Remove);
        OnUpdateList();
    }

    public void Add()
    {
        var newPlayer = Instantiate(playerPrefab, playerParent);
        var asset = dropDown.Current;
        newPlayer.SetAsset(asset);
        StartCoroutine(PlayRoutine(newPlayer));
        OnUpdateList();
    }

    public void Remove()
    {
        if (playerParent.childCount > 0)
        {
            var last = playerParent.transform.GetChild(playerParent.transform.childCount - 1);
            DestroyImmediate(last.gameObject);
        }
        OnUpdateList();
    }

    private void OnUpdateList()
    {
        UpdateCounts();
    }

    private void UpdateCounts()
    {
        var counts = playerParent.GetComponentsInChildren<ImageLottiePlayer>().Length;
        countText.text = $"Counts:{counts.ToString()}";
    }

    IEnumerator PlayRoutine(CustomImageLottiePlayer player)
    {
        yield return null;
        player.Play();
    }
}

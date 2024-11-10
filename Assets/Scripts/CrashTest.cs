using System.Collections;
using System.Collections.Generic;
using Gilzoide.LottiePlayer;
using TMPro;
using UnityEngine;

public class CrashTest : MonoBehaviour
{
    [Header("Prefab settings")]
    [SerializeField] private CustomImageLottiePlayer playerPrefab;
    [SerializeField] private Transform playerParent;
    
    [SerializeField] private TMP_Text countText;
    
    // Start is called before the first frame update
    void Start()
    {
        OnUpdateList();
    }

    public void Add()
    {
        var newPlayer = Instantiate(playerPrefab, playerParent);
        //newPlayer.SetAsset(asset);
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

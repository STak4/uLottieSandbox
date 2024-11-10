using System.Collections;
using System.Collections.Generic;
using System.IO;
using Gilzoide.LottiePlayer;
using UnityEngine;
using UnityEngine.Networking;

namespace Gilzoide.LottiePlayer
{
    [CreateAssetMenu(menuName = "Lottie/AnimationAsset")]
    public class CustomLottieAnimationAsset : LottieAnimationAsset
    {
        [SerializeField] public TextAsset jsonFile;

        public IEnumerator LoadFile()
        {
            if (!string.IsNullOrEmpty(ResourcePath))
            {
                string filePath = Path.Combine(Application.streamingAssetsPath, ResourcePath);
                Debug.Log($"[LottieAnimationAsset] Load json. path:{filePath}");

                using (UnityWebRequest www = UnityWebRequest.Get(filePath))
                {
                    yield return www.SendWebRequest();

                    if (www.result != UnityWebRequest.Result.Success)
                    {
                        Debug.Log(www.error);
                    }
                    else
                    {
                        string jsonString = www.downloadHandler.text;
                        TextAsset jsonText = new TextAsset(jsonString);
                        // ここでjsonTextを使って処理する
                        Debug.Log(jsonText.text);
                        jsonFile = jsonText;
                    }
                }
            }
            
            if (jsonFile != null)
            {
                Debug.Log($"[LottieAnimation] Asset updated. json:{jsonFile.name}");
                Json = jsonFile.text;
            }
        }
    }
}

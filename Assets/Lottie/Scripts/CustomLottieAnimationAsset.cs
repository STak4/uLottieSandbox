using System.Collections;
using System.Collections.Generic;
using Gilzoide.LottiePlayer;
using UnityEngine;

namespace Gilzoide.LottiePlayer
{
    [CreateAssetMenu(menuName = "Lottie/AnimationAsset")]
    public class CustomLottieAnimationAsset : LottieAnimationAsset
    {
        [SerializeField] public TextAsset jsonFile;

        public void LoadFile()
        {
            if (jsonFile == null) return;

            Debug.Log($"[LottieAnimation] Asset updated. json:{jsonFile.name}");
            Json = jsonFile.text;
        }
    }
}

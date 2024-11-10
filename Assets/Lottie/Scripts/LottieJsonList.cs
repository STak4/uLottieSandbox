using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Gilzoide.LottiePlayer
{
    [CreateAssetMenu(menuName = "Lottie/JsonList")]
    public class LottieJsonList : ScriptableObject
    {
        public List<string> JsonFiles = new List<string>();

        public string GetFileName(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace Gilzoide.LottiePlayer
{
    [CustomEditor(typeof(CustomLottieAnimationAsset), true)]
    public class CustomLottieAnimationAssetEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            CustomLottieAnimationAsset so = (CustomLottieAnimationAsset)target;

            if(GUI.changed)
            {
                if (!string.IsNullOrEmpty(so.ResourcePath) || so.jsonFile != null)
                {
                    EditorCoroutineUtility.StartCoroutineOwnerless(so.LoadFile());
                }
            }
        }
    }

}

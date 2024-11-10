using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gilzoide.LottiePlayer
{
    public class CustomImageLottiePlayer : ImageLottiePlayer
    {
        public void SetAsset(LottieAnimationAsset asset)
        {
            _animationAsset = asset;
            //RecreateAnimationIfNeeded();
        }
    }

}

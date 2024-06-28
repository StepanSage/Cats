using System;
using YG;

namespace Code.SDK_Yandex
{
    public class RewardingVideo: IWatchVideo, IRewarding
    {
        public event Action RewardCallBack;

        private YandexGame _sdk;

        public RewardingVideo(YandexGame SDK) 
        {
            _sdk = SDK;
        }
        
        public void WatchVideo()
        {
            _sdk._RewardedShow(1);
            RewardCallBack?.Invoke();
        }
    }

}

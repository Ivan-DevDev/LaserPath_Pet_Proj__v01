using UnityEngine;

namespace Assets.LazerPath2D.Scripts.Configs.Common.Sound
{
    [CreateAssetMenu(menuName = "Configs/Sound", fileName = "SoundClips")]
    public class SoundPrefabsConfigs : ScriptableObject
    {
        public AudioClip[] MusicPlayClips;

        //public AudioClip[] SelectNodeClips;
        //public AudioClip[] OffSelectNodeClips;

        public AudioClip[] RotateEmiterNodeClips;
        public AudioClip[] RotateMirrorNodeClips;
        public AudioClip[] NodeReceiverCompletedClips;

        public AudioClip[] GameOverCompletedClips;

        public AudioClip[] UIAnimScalingClips;
        public AudioClip[] UIAnimVibratoClips;
        public AudioClip[] UIAnimAttentionClips;
        public AudioClip[] UIClikedButtonClips;
    }
}

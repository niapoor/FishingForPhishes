using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagineRITGame
{
    public delegate void PlaySoundEffectDelegate(SoundEffects soundEffect);

    public enum Music
    {
        Game = 0
    }

    public enum SoundEffects
    {
        FishEscape = 0,
        CastRod = 1,
        Award = 2,
        ButtonPress = 3
    }
    public class SoundManager
    {

        private List<Song> music;
        private List<SoundEffect> soundEffects;

        public SoundManager(List<Song> music, List<SoundEffect> soundEffects)
        {
            this.music = music;
            this.soundEffects = soundEffects;
        }

        /// <summary>
        /// Plays a song and stops the current song
        /// </summary>
        /// <param name="song">song to play</param>
        /// <param name="repeat">if the song is repeatable</param>
        public void PlaySong(Song song, bool repeat)
        {
            MediaPlayer.Stop();
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = repeat;
        }

        public void PlaySoundEffect(SoundEffects soundEffect)
        {
            soundEffects[(int)soundEffect].Play();
        }
    }
}

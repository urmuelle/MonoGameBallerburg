// <copyright file="AudioManager.cs" company="Urs Müller">
//     Urs Müller. All rights reserved.
// </copyright>
// <author>Urs Müller</author>

namespace MonoGameBallerburg.Audio
{
  using System;
  using Gameplay;
  using Manager;
  using Microsoft.Xna.Framework;
  using Microsoft.Xna.Framework.Audio;
  using Microsoft.Xna.Framework.Media;
  using MonoGame.Framework;

  /// <summary>
  /// Class used to handle music and effect sounds in the game
  /// </summary>
  public class AudioManager : IDisposable
  {
    private readonly ApplicationSettings applicationSettings;
    /*
      private AudioEngine audioEngine;
      private WaveBank waveBank;
      private SoundBank soundBank;
      private WaveBank backgroundMusicWaveBank;
      private Cue sound12;
    */
    private ContentManager contentManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioManager"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>        
    public AudioManager(ApplicationSettings settings, ContentManager content)
    {
      this.applicationSettings = settings;
      this.contentManager = content;
      /*
        this.audioEngine = new AudioEngine(@"Content\Audio\Ballerburg.xgs");
        this.waveBank = new WaveBank(this.audioEngine, @"Content\Audio\BallerburgWaveBank.xwb");
        this.backgroundMusicWaveBank = new WaveBank(this.audioEngine, @"Content\Audio\BackgroundWaveBank.xwb");
        this.soundBank = new SoundBank(this.audioEngine, @"Content\Audio\BallerburgSoundBank.xsb");
       * */
      this.SetFxVolume(this.applicationSettings.FxVolume);
      this.SetMusicVolume(this.applicationSettings.MusicVolume);
    }

    /// <summary>
    /// Sets the music volume.
    /// </summary>
    /// <param name="volume">The volume.</param>
    public void SetMusicVolume(float volume)
    {
      /*
        var defaultCategory = this.audioEngine.GetCategory("Menu");
       * */
      this.applicationSettings.MusicVolume = MathHelper.Clamp(volume + 0.01f, 0.0f, 2.0f);
      
      MediaPlayer.Volume = MathHelper.Clamp(volume + 0.01f, 0.0f, 1.0f);
      /*
        defaultCategory.SetVolume(this.applicationSettings.MusicVolume);
       * */
    }

    /// <summary>
    /// Sets the fx volume.
    /// </summary>
    /// <param name="volume">The volume.</param>
    public void SetFxVolume(float volume)
    {
      /*
        var defaultCategory = this.audioEngine.GetCategory("Default");
       * */
      applicationSettings.FxVolume = MathHelper.Clamp(volume + 0.01f, 0.0f, 2.0f);
      /*
        defaultCategory.SetVolume(this.applicationSettings.FxVolume);
       * */
    }

    /// <summary>
    /// Updates this instance.
    /// </summary>
    public void Update()
    {
      /*
        this.audioEngine.Update();
       * */
    }

    /// <summary>
    /// Plays the klick sound.
    /// </summary>
    public void PlayKlickSound()
    {
      /*
        this.soundBank.PlayCue("Sound1");
       * */
    }

    /// <summary>
    /// Plays the menu background music.
    /// </summary>
    public void PlayMenuBackgroundMusic()
    {
      MediaPlayer.Play(contentManager.BackgroundMusicTracks[this.applicationSettings.ActiveBackgroundMusicTrack.ToString()]);
      MediaPlayer.IsRepeating = true;
    }

    /// <summary>
    /// Stops the menu background sound.
    /// </summary>
    public void StopMenuBackgroundMusic()
    {
      MediaPlayer.Stop();
    }

    /// <summary>
    /// Plays the game background music.
    /// </summary>
    public void PlayGameBackgroundMusic()
    {
      /*
        this.audioEngine.GetCategory("Menu").Stop(AudioStopOptions.Immediate);
       * */
    }

    /// <summary>
    /// Plays the fire sound.
    /// </summary>
    public void PlayFireSound()
    {
      /*
        this.soundBank.PlayCue("Sound10");
       * */
    }

    /// <summary>
    /// Plays the hit sound.
    /// </summary>
    public void PlayHitSound()
    {
      /*
        this.soundBank.PlayCue("Sound9");
       * */
    }

    /// <summary>
    /// Plays the cannon rotate sound.
    /// </summary>
    public void PlayCannonRotateSound()
    {
      /*
        this.sound12 = this.soundBank.GetCue("Sound12");          
        this.sound12.Play();
       * */
    }

    /// <summary>
    /// Stops the cannon rotate sound.
    /// </summary>
    public void StopCannonRotateSound()
    {
      /*
        if (this.sound12 != null)
        {
            this.sound12.Stop(AudioStopOptions.Immediate);
        }
       * */
    }

    /// <summary>
    /// Plays the tube move sound.
    /// </summary>
    public void PlayTubeMoveSound()
    {
      /*
        this.soundBank.PlayCue("Sound9");
       * */
    }

    /// <summary>
    /// Plays the explosion sound.
    /// </summary>
    public void PlayExplosionSound()
    {
      /*
        this.soundBank.PlayCue("Sound7");
       * */
    }

    #region IDisposable

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    #endregion

    /// <summary>
    /// Releases unmanaged and - optionally - managed resources
    /// </summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        /*
          this.audioEngine.Dispose();
          this.audioEngine = null;
          this.waveBank.Dispose();
          this.waveBank = null;
          this.backgroundMusicWaveBank.Dispose();
          this.backgroundMusicWaveBank = null;
          this.soundBank.Dispose();
          this.soundBank = null;
         * */
      }
    }
  }
}

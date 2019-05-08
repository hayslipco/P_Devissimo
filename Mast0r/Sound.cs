using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace SpicyInvader
{
    /// <summary>
    /// Classe des sons
    /// </summary>
    public static class Sound
    {
        private static MediaPlayer gameTheme = new MediaPlayer();
        private static MediaPlayer menuTheme = new MediaPlayer();
        private static MediaPlayer shield = new MediaPlayer();
        private static MediaPlayer shotgun = new MediaPlayer();
        private static MediaPlayer sniper = new MediaPlayer();
        private static MediaPlayer laser = new MediaPlayer();
        private static MediaPlayer shotgunR = new MediaPlayer();
        private static MediaPlayer sniperR = new MediaPlayer();
        private static MediaPlayer laserR = new MediaPlayer();
        private static MediaPlayer click = new MediaPlayer();
        private static MediaPlayer hit = new MediaPlayer();
        private static MediaPlayer record = new MediaPlayer();
        private static MediaPlayer suspense = new MediaPlayer();
        private static MediaPlayer loss = new MediaPlayer();
        private static MediaPlayer oof = new MediaPlayer();

        private static bool open = true;
        private static bool onOffSound = true;

        public static void OpenSounds()
        {
            menuTheme.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\mii_theme.wav"));
            gameTheme.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\wii_sport.wav"));            
            click.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\minecraft_click.wav"));            
            oof.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\off_sound.wav"));            
        }

        public static void OpenInGameSound()
        {
            if (OpenFiles())
            {
                shield.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\shield_sound.wav"));
                shotgun.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\shotgun_sound.wav"));
                sniper.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\invervention_sound.wav"));
                laser.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\laser_shot.wav"));
                shotgunR.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\shotgun_recharge.wav"));               
                sniperR.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\sniper_recharge.wav"));
                laserR.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\laser_recharge.wav"));
                hit.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\death.wav"));
                record.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\newRecord_sound.wav"));
                suspense.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\suspense_music.wav"));
                loss.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\cheh_sound.wav"));
                Thread.Sleep(150);
            }
        }
            

        private static bool OpenFiles()
        {
            if (open)
            {
                open = false;
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public static void Play()
        {
            SoundPlayer gameTheme = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + @"sounds\wii_sport.wav");
            gameTheme.PlayLooping();           
        }

        public static void MenuTheme(bool play)
        {
            if (play && onOffSound)
            {
                menuTheme.Play();
                
            }
            else
            {
                menuTheme.Stop();
            }
        }

        public static void GameTheme(bool play, bool pause, bool danger)
        {
            if (play)
            {
                gameTheme.SpeedRatio = 1;
                gameTheme.Play();
            }
            else if (pause)
            {
                gameTheme.Pause();
            }
            else if (danger)
            {                
                gameTheme.SpeedRatio = 1.5;
            }
            else
            {
                gameTheme.Stop();
            }
        }

        public static void OofSound()
        {
            oof.Position = TimeSpan.Zero;
            oof.Play();
        }

        public static void ClickSound()
        {
            click.Position = TimeSpan.FromMilliseconds(500);
            click.Play();
        }

        public static void HitSound()
        {
            hit.Position = TimeSpan.Zero;
            hit.Play();
        }

        public static void ShotgunSound()
        {
            shotgun.Position = TimeSpan.Zero;
            shotgun.Play();
        }

        public static void ShotgunRSound()
        {
            shotgunR.Position = TimeSpan.Zero;
            shotgunR.Play();
        }

        public static void SniperSound()
        {
            sniper.Position = TimeSpan.Zero;
            sniper.Play();
        }

        public static void SniperRSound()
        {
            sniperR.Position = TimeSpan.Zero;
            sniperR.Play();
        }

        public static void LaserSound()
        {
            laser.Position = TimeSpan.Zero;
            laser.Play();
        }

        public static void LaserRSound()
        {
            laserR.Position = TimeSpan.Zero;
            laserR.Play();
        }

        public static void ShieldSound()
        {
            shield.Position = TimeSpan.Zero;
            shield.Play();
        }

        public static void EndGameSuspense(bool play)
        {
            if (play)
            {
                suspense.Play();
            }
            else
            {
                suspense.Stop();
            }
            
        }

        public static void NoRecord(bool play)
        {
            if (play)
            {
                loss.Play();
            }
            else
            {
                loss.Stop();
            }

        }


        public static void NewRecord(bool play)
        {
            if (play)
            {
                record.Play();
            }
            else
            {
                record.Stop();
            }
        }

        public static void SoundClose()
        {
          
            menuTheme.Stop();
            suspense.Volume = 0;
            gameTheme.Volume = 0;
            record.Volume = 0;
            loss.Volume = 0;
            oof.Volume = 0;
            click.Volume = 0;
            hit.Volume = 0;
            sniper.Volume = 0;
            sniperR.Volume = 0;
            shotgun.Volume = 0;
            shotgunR.Volume = 0;
            laser.Volume = 0;
            laserR.Volume = 0;
            shield.Volume = 0;

            onOffSound = false;
        }

        public static void SoundStart()
        {           
            menuTheme.Play();
            oof.Volume = 0.5;
            click.Volume = 0.5;
            hit.Volume = 0.5;
            sniper.Volume = 0.5;
            sniperR.Volume = 0.5;
            shotgun.Volume = 0.5;
            shotgunR.Volume = 0.5;
            laser.Volume = 0.5;
            laserR.Volume = 0.5;
            shield.Volume = 0.5;
            suspense.Volume = 0.5;
            gameTheme.Volume = 0.5;
            record.Volume = 0.5;
            loss.Volume = 0.5;

            onOffSound = true;
        }


    }
}

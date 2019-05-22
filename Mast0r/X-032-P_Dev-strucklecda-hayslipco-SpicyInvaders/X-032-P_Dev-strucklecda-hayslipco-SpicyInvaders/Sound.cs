/*
 * ETML
 * Auteurs: Davor S.et Corwin H.
 * Date de création: 15.05.19
 * Description: Classe des sons joués
 */
using System;
using System.Media;
using System.Threading;
using System.Windows.Media;

namespace X_032_P_Dev_strucklecda_hayslipco_SpicyInvaders
{
    /// <summary>
    /// Classe des sons
    /// </summary>
    public static class Sound
    {
        // création des instances de sons
        private static MediaPlayer gameTheme = new MediaPlayer();       
        private static MediaPlayer shield = new MediaPlayer();
        private static MediaPlayer shotgun = new MediaPlayer();
        private static MediaPlayer sniper = new MediaPlayer();
        private static MediaPlayer laser = new MediaPlayer();
        private static MediaPlayer shotgunR = new MediaPlayer();
        private static MediaPlayer sniperR = new MediaPlayer();
        private static MediaPlayer laserR = new MediaPlayer();
        private static MediaPlayer click = new MediaPlayer();
        private static MediaPlayer hit = new MediaPlayer();       
        private static MediaPlayer suspense = new MediaPlayer();       
        private static MediaPlayer oof = new MediaPlayer();
        private static SoundPlayer menuTheme = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + @"sounds\mii_theme.wav");
        private static SoundPlayer record = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + @"sounds\newRecord_sound.wav");
        private static SoundPlayer loss = new SoundPlayer(AppDomain.CurrentDomain.BaseDirectory + @"sounds\cheh_sound.wav");


        private static bool open = true;
        public static bool onOffSound = true;

        /// <summary>
        /// Ouverture des fichiers sons au lancement du jeu
        /// </summary>
        public static void OpenSounds()
        {            
            gameTheme.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\wii_sport.wav"));            
            click.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\minecraft_click.wav"));
            oof.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\off_sound.wav"));            
            click.Volume = 0.7;
            oof.Volume = 0.7;
        }

        /// <summary>
        /// Ouverture des fichiers sons au lancement de la partie
        /// </summary>
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
                suspense.Open(new Uri(AppDomain.CurrentDomain.BaseDirectory + @"sounds\suspense_music.wav"));               
                Thread.Sleep(150);                
            }
        }
        /// <summary>
        /// Get de onOffSound
        /// </summary>
        public static bool OnOffSound
        {
            get
            {
                return onOffSound;
            }
        }

        

        /// <summary>
        /// Méthode permettant d'ouvrir une seul fois les fichiers sons in-game
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Joue/stop le theme du menu
        /// </summary>
        /// <param name="play"></param>
        public static void MenuTheme(bool play)
        {
            if (play && onOffSound)
            {                
                menuTheme.PlayLooping();
                
            }
            else
            {
                menuTheme.Stop();               
            }
        }

        /// <summary>
        /// Joue/stop/accélère le theme de la partie
        /// </summary>
        /// <param name="play"></param>
        /// <param name="pause"></param>
        /// <param name="danger"></param>
        public static void GameTheme(bool play, bool pause, bool danger)
        {
            if (play && onOffSound)
            {
                gameTheme.SpeedRatio = 1;
                gameTheme.Play();
            }
            else if (pause && onOffSound)
            {
                gameTheme.Pause();
            }
            // Accélère le theme de la partie s'il ne reste qu'une seul vie au joueur
            else if (danger && onOffSound)
            {                
                gameTheme.SpeedRatio = 1.5;
            }
            else
            {
                gameTheme.Stop();
            }
        }

        /// <summary>
        /// Joue le son du déplacement du curseur dans le menu
        /// </summary>
        public static void OofSound()
        {
            if (onOffSound)
            {
                oof.Position = TimeSpan.Zero;           
                oof.Play();
            }                
        }

        /// <summary>
        /// Joue le son de click dans le menu
        /// </summary>
        public static void ClickSound()
        {
            if (onOffSound)
            {
                click.Position = TimeSpan.FromMilliseconds(500);            
                click.Play();
            }
        }

        /// <summary>
        /// Son joué lorsqu'on est touché par un projectile
        /// </summary>
        public static void HitSound()
        {
            if (onOffSound)
            {
                hit.Position = TimeSpan.Zero;
                hit.Play();
            }
        }

        /// <summary>
        /// Son du tir fusil à pompe
        /// </summary>
        public static void ShotgunSound()
        {
            if (onOffSound)
            {
                shotgun.Position = TimeSpan.Zero;
                shotgun.Play();
            }               
        }

        /// <summary>
        /// Son du rechargement fusil à pompe
        /// </summary>
        public static void ShotgunRSound()
        {
            if (onOffSound)
            {
                shotgunR.Position = TimeSpan.Zero;
                shotgunR.Play();
            }
        }

        /// <summary>
        /// Son du tir sniper
        /// </summary>
        public static void SniperSound()
        {
            if (onOffSound)
            {
                sniper.Position = TimeSpan.Zero;
                sniper.Play();
            }
        }

        /// <summary>
        /// Son du rechargement sniper
        /// </summary>
        public static void SniperRSound()
        {
            if (onOffSound)
            {
                sniperR.Position = TimeSpan.Zero;
                sniperR.Play();
            }          
        }

        /// <summary>
        /// Son du tir laser
        /// </summary>
        public static void LaserSound()
        {
            if (onOffSound)
            {
                laser.Position = TimeSpan.Zero;
                laser.Play();
            }
        }

        /// <summary>
        /// Son du rechragement sniper
        /// </summary>
        public static void LaserRSound()
        {
            if (onOffSound)
            {
                laserR.Position = TimeSpan.Zero;
                laserR.Play();
            }            
        }

        /// <summary>
        /// Son du bouclier
        /// </summary>
        public static void ShieldSound()
        {
            if (onOffSound)
            {
                shield.Position = TimeSpan.Zero;
                shield.Play();
            }              
        }

        /// <summary>
        /// Musique de suspense
        /// </summary>
        /// <param name="play"></param>
        public static void EndGameSuspense(bool play)
        {
            if (play && onOffSound)
            {
                suspense.Play();
            }
            else
            {
                suspense.Stop();
            }           
        }

        /// <summary>
        /// Musique quand aucun record n'est battu
        /// </summary>
        /// <param name="play"></param>
        public static void NoRecord(bool play)
        {
            if (play && onOffSound)
            {
                loss.PlayLooping();
            }
            else
            {
                loss.Stop();
            }
        }

        /// <summary>
        /// Musique quand un record a été battu
        /// </summary>
        /// <param name="play"></param>
        public static void NewRecord(bool play)
        {
            if (play && onOffSound)
            {
                record.PlayLooping();
            }
            else
            {
                record.Stop();
            }
        }       
    }
}

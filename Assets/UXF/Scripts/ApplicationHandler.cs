using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UXF
{
    public class ApplicationHandler : MonoBehaviour
    {

        
        // setup high resolution timer & setup refpoint
        private static readonly System.DateTime _hr_refpoint = System.DateTime.Now;
        private static readonly System.Diagnostics.Stopwatch _hr_timer = System.Diagnostics.Stopwatch.StartNew();
        public static string CurrentHighResolutionTime
        {
            get
            {
                // use DateTime facility
                return new System.DateTime(

                    // from ref point
                    UXF.ApplicationHandler._hr_refpoint.Ticks

                //).AddMilliseconds(

                //    // then add elapsed ticks to ns to ms
                //    UXF.ApplicationHandler._hr_timer.ElapsedTicks
                //        * ((1000L * 1000L * 1000L) / System.Diagnostics.Stopwatch.Frequency)
                //        / 1000000.0
                ).AddTicks(

                    // then add elapsed ticks to ns to ms
                    UXF.ApplicationHandler._hr_timer.ElapsedTicks

                ).ToString(
                    
                    // finally, format as a string 
                    "HH:mm:ss.ffffff"

                );
                
            } /* get */

        } /* CurrentHighResolutionTime */

        /// <summary>
        /// Quits the application. This is a handy helper method for use with the onSessionEnd 
        /// </summary>
        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif	
        }

        /// <summary>
        /// Reloads the currently active scene. This is a handy helper method for use with the onSessionEnd
        /// </summary>
        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
    }
}

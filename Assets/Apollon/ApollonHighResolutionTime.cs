// using directive
using System.Linq;

// avoid namespace pollution
namespace Labsim.apollon
{

    public static class ApollonHighResolutionTime
    {

        public sealed class HighResolutionTimepoint
        {

            // setup high resolution chrono & setup refpoint
            private readonly System.DateTime m_current_refpoint 
                = new System.DateTime(

                    // from absolute ref point
                    ApollonHighResolutionTime._hr_refpoint.Ticks

                ).AddTicks(

                    // then add elapsed ticks to ns to ms
                    ApollonHighResolutionTime._hr_chrono.ElapsedTicks

                ); /* ApollonHighResolutionTime.Now; */
            private readonly System.Diagnostics.Stopwatch m_current_chrono = System.Diagnostics.Stopwatch.StartNew();

            public System.DateTime Reference => this.m_current_refpoint;
            public double ElapsedMilliseconds
            {
                get
                {

                    // use TimeSpan facility
                    return new System.TimeSpan(

                        // from current ref point to 
                        this.m_current_refpoint.Ticks + this.m_current_chrono.ElapsedTicks

                    ).TotalMilliseconds;
                    
                } /* get */

            } /* ElapsedMilliseconds */

            public override string ToString()
            {
                
                // format as a string 
                return this.Reference.ToString("HH:mm:ss.ffffff");

            } /* ToString() */

        } /* class HighResolutionTimepoint */

        // setup high resolution chrono & setup abolute refpoint
        private static readonly System.DateTime _hr_refpoint = System.DateTime.Now;
        private static readonly System.Diagnostics.Stopwatch _hr_chrono = System.Diagnostics.Stopwatch.StartNew();

        public static HighResolutionTimepoint Now => new HighResolutionTimepoint();

        public static async System.Threading.Tasks.Task DoSleep(double duration_in_ms)
        {
        
            // wait a certain amout of time
            var timepoint = ApollonHighResolutionTime.Now;
            while (timepoint.ElapsedMilliseconds < duration_in_ms)
            {
                await System.Threading.Tasks.Task.Delay(10);
            }

        } /* DoSleep() */
        
    } /* static class ApollonHighResolutionTime */

} /* namespace Labsim.apollon */
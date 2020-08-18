
// avoid namespace pollution
namespace Labsim.apollon.common
{

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public class ApollonFieldRangeAttribute
        : System.Attribute
    {

        private float m_min = 0.0f;
        private float m_max = 0.0f;

        public virtual float Min
        {
            get 
            {
                return this.m_min;
            }
            set
            {
                this.m_min = value;
            }
        }

        public virtual float Max
        {
            get 
            {
                return this.m_max;
            }
            set
            {
                this.m_max = value;
            }
        }

    } /* class ApollonFieldRangeAttribute */

} /* namespace Labsim.apollon.common */ 

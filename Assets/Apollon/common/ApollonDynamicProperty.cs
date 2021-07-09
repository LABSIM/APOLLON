
// avoid namespace pollution
namespace Labsim.apollon.common
{

    public class ApollonDynamicProperty
    {
        private readonly System.Collections.Generic.List<object> m_metadata = new System.Collections.Generic.List<object>();
        public virtual System.Collections.Generic.List<object> Metadata
        {
            get
            {
                return this.m_metadata;
            }
        }

        private object m_data = null;
        public virtual object Data
        {
            get
            {
                return this.m_data;
            }
            set
            {
                this.m_data = value;
            }
        }

        private ApollonFieldRangeAttribute m_metadataRange = null;
        public ApollonFieldRangeAttribute Range
        {
            get
            {
                return this.m_metadataRange;
            }
            private set
            {
                this.m_metadataRange = value;
            }
        }

        public ApollonDynamicProperty(object data, System.Collections.Generic.List<object> metadata = null)
        {
            this.m_data = data;
            if (metadata != null)
            {
                this.m_metadata = metadata;

                // iterate
                foreach (object meta_property in this.Metadata)
                {
                    // check range
                    ApollonFieldRangeAttribute range = meta_property as ApollonFieldRangeAttribute;
                    if (range != null)
                    {
                        this.Range = range;
                    }

                } /* foreach() */

            } /* if() */

        } /* ApollonDynamicProperty() */

    } /* class ApollonDynamicProperty */

} /* } Labsim.apollon.common */
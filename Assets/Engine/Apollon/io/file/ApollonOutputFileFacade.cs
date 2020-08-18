
// avoid namespace pollution
namespace Labsim.apollon.io.file
{

    public class ApollonOutputFileFacade
    {
        // Ctor
        public ApollonOutputFileFacade(datamodel.ApollonOutputFileImpl impl)
        {

            // the only one assignement
            this.Implementation = impl;

        } /* Ctor */

        #region private implementation

        private datamodel.ApollonOutputFileImpl m_impl = null;
        public datamodel.ApollonOutputFileImpl Implementation
        {
            get
            {
                return this.m_impl;
            }
            private set
            {
                this.m_impl = value;
            }
        }

        #endregion

        #region dynamic properties

        private readonly System.Collections.Generic.Dictionary<string, common.ApollonDynamicProperty> m_properties 
            = new System.Collections.Generic.Dictionary<string, common.ApollonDynamicProperty>();

        public common.ApollonFieldRangeAttribute GetRangeMetadata(string key)
        {
            if (this.m_properties.ContainsKey(key))
            {
                return this.m_properties[key].Range;
            }
            else
            {
                return null;
            }
        }

        public object GetData(string key)
        {
            if (this.m_properties.ContainsKey(key))
            {
                return this.m_properties[key].Data;
            }
            else
            {
                return null;
            }
        }

        public void SetData(string key, object value)
        {
            if (this.m_properties.ContainsKey(key))
            {
                this.m_properties[key].Data = value;
            }
        }

        public System.Collections.Generic.IEnumerable<string> GetKeys()
        {
            return this.m_properties.Keys;
        }

        #endregion

        #region public method

        private void _dbg_print_prop(string key)
        {
            UnityEngine.Debug.Log("<color=blue>Info: </color> ApollonOutputFileFacade.Checkout() : " + key + " [" + this.GetData(key) + "]");
            if (this.GetRangeMetadata(key) != null)
            {
                UnityEngine.Debug.Log(
                    "<color=blue>Info: </color> ApollonOutputFileFacade.dbg_print_prop() : " + key + " Range ["
                    + this.GetRangeMetadata(key).Min
                    + " : "
                    + this.GetRangeMetadata(key).Max
                    + "]"
                );
            }
        }

        public void Checkout()
        {

            // checkout data from type reflection
            foreach (System.Reflection.FieldInfo field in this.Implementation.GetType().GetFields())
            {

                string fieldID = "m_";
                char[] propID = field.Name.Substring(field.Name.IndexOf(fieldID) + fieldID.Length).ToCharArray();
                propID[0] = char.ToUpper(propID[0]);

                this.m_properties.Add(
                    new string(propID),
                    new common.ApollonDynamicProperty(
                        field.GetValue(this.Implementation),
                        new System.Collections.Generic.List<object>(field.GetCustomAttributes(false))
                    )
                );

                // debug log
                this._dbg_print_prop(new string(propID));

            } /* foreach() */

        } /* Checkout() */

        public void Commit()
        {

            // commit data from type reflection
            foreach (System.Reflection.FieldInfo field in this.Implementation.GetType().GetFields())
            {

                string fieldID = "m_";
                char[] propID = field.Name.Substring(field.Name.IndexOf(fieldID) + fieldID.Length).ToCharArray();
                propID[0] = char.ToUpper(propID[0]);

                object boxedObject = this.Implementation;
                field.SetValue(boxedObject, this.m_properties[new string(propID)].Data);
                this.Implementation = (datamodel.ApollonOutputFileImpl)boxedObject;

            } /* foreach() */

        } /* Commit() */

        #endregion

    } /* public class ApollonOutputFileFacade */

} /* namespace Labsim.apollon.io.file */

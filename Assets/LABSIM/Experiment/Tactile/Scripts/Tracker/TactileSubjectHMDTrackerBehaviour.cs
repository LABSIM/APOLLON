using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileSubjectHMDTrackerBehaviour 
        : UXF.Tracker
    {

        // members
        private bool m_bHasInitialized = false;

        // bridge
        public TactileSubjectHMDTracker Tracker { get; set; }

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectHMDTrackerBehaviour.Initialize() : begin");
            
            // todo

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectHMDTrackerBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectHMDTrackerBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectHMDTrackerBehaviour.Close() : begin");
   
            // todo
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectHMDTrackerBehaviour.Close() : end");

        } /* Close() */
        

        #region MonoBehaviour Impl         

        [UnityEngine.SerializeField, UnityEngine.Range(10.0f, 200.0f)]
        private float m_dataAcquisitionFrequencyHz = 100.0f;
        public double DataAcquisitionFrequencyHz => this.m_dataAcquisitionFrequencyHz;

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectHMDTrackerBehaviour.OnEnable() : initialize required");

                // call
                this.Initialize();

            } /* if() */

        } /* OnEnable() */

        private void OnDisable() 
        {

            // skip if it hasn't been initialized 
            if (this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectHMDTrackerBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

        #region UXF implementation

        // Sets measurementDescriptor and customHeader to appropriate values
        protected override void SetupDescriptorAndHeader()
        {

            this.measurementDescriptor = "movement";
            this.customHeader = new string[]
            {
                "host_timestamp",
                "world_pos_x",
                "world_pos_y",
                "world_pos_z",
                "world_rot_x",
                "world_rot_y",
                "world_rot_z",
                "local_pos_x",
                "local_pos_y",
                "local_pos_z",
                "local_rot_x",
                "local_rot_y",
                "local_rot_z"
            };

        } /* SetupDescriptorAndHeader() */

        // Returns current position and rotation values
        protected override UXF.UXFDataRow GetCurrentValues()
        {

            // return world [position, rotation] (x, y, z) & local [position, rotation] (x, y, z) as an array
            var values = new UXF.UXFDataRow()
            {
                ("host_timestamp", apollon.ApollonHighResolutionTime.Now.ToString()),
                ("world_pos_x", gameObject.transform.position.x),
                ("world_pos_y", gameObject.transform.position.y),
                ("world_pos_z", gameObject.transform.position.z),
                ("world_rot_x", gameObject.transform.eulerAngles.x),
                ("world_rot_y", gameObject.transform.eulerAngles.y),
                ("world_rot_z", gameObject.transform.eulerAngles.z),
                ("local_pos_x", gameObject.transform.localPosition.x),
                ("local_pos_y", gameObject.transform.localPosition.y),
                ("local_pos_z", gameObject.transform.localPosition.z),
                ("local_rot_x", gameObject.transform.localEulerAngles.x),
                ("local_rot_y", gameObject.transform.localEulerAngles.y),
                ("local_rot_z", gameObject.transform.localEulerAngles.z)
            };

            return values;

        } /* GetCurrentValues() */

        #endregion

    } /* class TactileSubjectHMDTrackerBehaviour */

} /* } Labsim.experiment.tactile */
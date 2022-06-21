using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileSubjectRHIndexMiddleJointTrackerBehaviour 
        : UXF.Tracker
    {

        // members
        private bool m_bHasInitialized = false;

        // bridge
        public TactileSubjectRHIndexMiddleJointTracker Tracker { get; set; }

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectRHIndexMiddleJointTrackerBehaviour.Initialize() : begin");
            
            // todo

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectRHIndexMiddleJointTrackerBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectRHIndexMiddleJointTrackerBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectRHIndexMiddleJointTrackerBehaviour.Close() : begin");
   
            // todo
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectRHIndexMiddleJointTrackerBehaviour.Close() : end");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectRHIndexMiddleJointTrackerBehaviour.OnEnable() : initialize required");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileSubjectRHIndexMiddleJointTrackerBehaviour.OnEnable() : close required");

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

            // get position and rotation
            UnityEngine.Vector3 world_p = gameObject.transform.position;
            UnityEngine.Vector3 world_r = gameObject.transform.eulerAngles;
            UnityEngine.Vector3 local_p = gameObject.transform.localPosition;
            UnityEngine.Vector3 local_r = gameObject.transform.localEulerAngles;

            // return world [position, rotation] (x, y, z) & local [position, rotation] (x, y, z) as an array
            var values = new UXF.UXFDataRow()
            {
                ("world_pos_x", world_p.x),
                ("world_pos_y", world_p.y),
                ("world_pos_z", world_p.z),
                ("world_rot_x", world_r.x),
                ("world_rot_y", world_r.y),
                ("world_rot_z", world_r.z),
                ("local_pos_x", local_p.x),
                ("local_pos_y", local_p.y),
                ("local_pos_z", local_p.z),
                ("local_rot_x", local_r.x),
                ("local_rot_y", local_r.y),
                ("local_rot_z", local_r.z)
            };

            return values;

        } /* GetCurrentValues() */

        #endregion

    } /* class TactileSubjectRHIndexMiddleJointTrackerBehaviour */

} /* } Labsim.experiment.tactile */
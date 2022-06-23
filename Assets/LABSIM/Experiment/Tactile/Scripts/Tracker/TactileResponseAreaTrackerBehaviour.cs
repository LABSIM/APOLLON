using System.Linq;

// avoid namespace pollution
namespace Labsim.experiment.tactile
{

    public sealed class TactileResponseAreaTrackerBehaviour 
        : UXF.Tracker
    {

        // members
        private bool m_bHasInitialized = false;

        // bridge
        public TactileResponseAreaTracker Tracker { get; set; }

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaTrackerBehaviour.Initialize() : begin");
            
            // todo

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaTrackerBehaviour.Initialize() : init ok, mark as initialized");
            
            // switch state
            this.m_bHasInitialized = true;

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaTrackerBehaviour.Initialize() : end");

        } /* Initialize() */

        private void Close()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaTrackerBehaviour.Close() : begin");
   
            // todo
            
            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaTrackerBehaviour.Close() : end");

        } /* Close() */
        

        #region MonoBehaviour Impl         

        [UnityEngine.SerializeField, UnityEngine.Range(10.0f, 200.0f)]
        private float m_dataAcquisitionFrequencyHz = 100.0f;
        public double DataAcquisitionFrequencyHz => this.m_dataAcquisitionFrequencyHz;

        public TactileResponseAreaBehaviour TrackedBehaviour => TactileManager.Instance.getBridge(TactileManager.IDType.TactileResponseArea).Behaviour as TactileResponseAreaBehaviour;

        private void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaTrackerBehaviour.OnEnable() : initialize required");

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
                UnityEngine.Debug.Log("<color=Blue>Info: </color> TactileResponseAreaTrackerBehaviour.OnEnable() : close required");

                // call
                this.Close();

            } /* if() */

        } /* OnDisable() */

        #endregion

        #region UXF implementation

        // Sets measurementDescriptor and customHeader to appropriate values
        protected override void SetupDescriptorAndHeader()
        {

            this.measurementDescriptor = "interaction";
            this.customHeader = new string[]
            {
                "host_timestamp",
                "point_0_unity_timestamp",
                "point_0_host_timestamp",
                "point_0_pos_x",
                "point_0_pos_y",
                "point_0_unity_timestamp",
                "point_0_host_timestamp",
                "point_1_pos_x",
                "point_1_pos_y",
                "point_1_unity_timestamp",
                "point_1_host_timestamp",
                "point_2_pos_x",
                "point_2_pos_y",
                "point_3_unity_timestamp",
                "point_3_host_timestamp",
                "point_3_pos_x",
                "point_3_pos_y",
                "point_4_unity_timestamp",
                "point_4_host_timestamp",
                "point_4_pos_x",
                "point_4_pos_y"
            };

        } /* SetupDescriptorAndHeader() */

        // Returns current position and rotation values
        protected override UXF.UXFDataRow GetCurrentValues()
        {

            // get a reference to the tracked behaviour
            var behaviour = this.TrackedBehaviour;

            // intantiate row & set timestamp
            var values = new UXF.UXFDataRow()
            {
                ("host_timestamp", apollon.ApollonHighResolutionTime.Now.ToString())
            };

            // add touchpoints values
            foreach(var touchpoint in behaviour.TouchpointList.Select((value, idx) => new { item = value, index = idx }))
            {
                
                values.Add(("point_" + touchpoint.index.ToString() + "_unity_timestamp", touchpoint.item.UnityTimestamp));
                values.Add(("point_" + touchpoint.index.ToString() + "_host_timestamp", touchpoint.item.HostTimestamp));
                values.Add(("point_" + touchpoint.index.ToString() + "_pos_x", touchpoint.item.X));
                values.Add(("point_" + touchpoint.index.ToString() + "_pos_y", touchpoint.item.Y));

            } /* foreach() */

            // fill remaining with default values
            for(int idx = /* 0 based max count */ 4; idx > behaviour.TouchpointList.Count - 1; --idx)
            {

                values.Add(("point_" + idx.ToString() + "_unity_timestamp", "NULL"));
                values.Add(("point_" + idx.ToString() + "_host_timestamp", "NULL"));
                values.Add(("point_" + idx.ToString() + "_pos_x", "NULL"));
                values.Add(("point_" + idx.ToString() + "_pos_y", "NULL"));
            
            } /* for() */

            return values;

        } /* GetCurrentValues() */

        #endregion

    } /* class TactileResponseAreaTrackerBehaviour */

} /* } Labsim.experiment.tactile */
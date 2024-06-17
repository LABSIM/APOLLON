//
// APOLLON : immersive & interactive experimental protocol engine
// Copyright (C) 2021-2023  Nawfel KINANI 
// nawfel (dot) kinani at onera (dot) fr
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program; see the files COPYING and COPYING.LESSER.
// If not, see <http://www.gnu.org/licenses/>.
//

// avoid namespace pollution
namespace Labsim.apollon.gameplay.device
{

    public class ApollonGeneric3DoFHapticArmBehaviour
        : ApollonConcreteGameplayBehaviour<ApollonGeneric3DoFHapticArmBridge>
    {

        #region properties/members

        [UnityEngine.SerializeField]
        private impedence.IApollonImpedenceModel m_impedenceReference = null;
        public ref impedence.IApollonImpedenceModel ImpedenceReference => ref this.m_impedenceReference;

        [UnityEngine.SerializeField]
        public UnityEngine.Vector3 m_initialPosition = UnityEngine.Vector3.zero;
        public ref UnityEngine.Vector3 InitialPosition => ref this.m_initialPosition;

        [UnityEngine.SerializeField]
        public UnityEngine.Vector3 m_initialRotation = UnityEngine.Vector3.zero;
        public ref UnityEngine.Vector3 InitialRotation => ref this.m_initialRotation;

        private bool m_bHasInitialized = false;

        #endregion

        #region controllers implemantion

        internal class InitController
            : UnityEngine.MonoBehaviour
        {
            private ApollonGeneric3DoFHapticArmBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;

            } /* Awake() */

            private void OnEnable() 
            {
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.InitController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonGeneric3DoFHapticArmBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonGeneric3DoFHapticArmBehaviour.InitController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // initialize
                // TODO ?

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.InitController.OnEnable() : rigidbody configured with current user settings, going idle state"
                );

                // change state
                this._parent.ConcreteBridge.ConcreteDispatcher.RaiseIdle();

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.InitController.OnEnable() : end"
                );
                
            } /* OnEnable() */
            
        } /* internal class InitController */

        internal class IdleController
            : UnityEngine.MonoBehaviour
        {

            private ApollonGeneric3DoFHapticArmBehaviour _parent = null;

            private void Awake()
            {

                // disable by default & set name
                this.enabled = false;
                //this.name = "ApollonIdleController";

            } /* Awake() */
            
            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.IdleController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonGeneric3DoFHapticArmBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonGeneric3DoFHapticArmBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // Idle
                // TODO
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.IdleController.OnEnable() : end"
                );

            } /* OnEnable()*/
            
            private void OnDisable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.IdleController.OnDisable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonGeneric3DoFHapticArmBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonGeneric3DoFHapticArmBehaviour.IdleController.OnEnable() : failed to get parent reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.IdleController.OnDisable() : end"
                );

            } /* OnDisable() */
            
        } /* class IdleController */

        internal class ControlController
            : UnityEngine.MonoBehaviour
        {

            private ApollonGeneric3DoFHapticArmBehaviour _parent = null;

            private void Awake()
            {

                // disable by default
                this.enabled = false;
                //this.name = "ApollonAccelerateController";

            } /* Awake() */

            private void OnEnable()
            {
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.AccelerateController.OnEnable() : begin"
                );

                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonGeneric3DoFHapticArmBehaviour>()) == null)
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonGeneric3DoFHapticArmBehaviour.AccelerateController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);
                    this.enabled = false;

                    // return
                    return;

                } /* if() */
                
                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.AccelerateController.OnEnable() : end"
                );

            } /* OnEnable()*/

        } /* class ControlController */

        internal class ResetController
            : UnityEngine.MonoBehaviour
        {

            private ApollonGeneric3DoFHapticArmBehaviour _parent = null;
            private UnityEngine.Quaternion _lerp_rotation_from;
            private UnityEngine.Vector3 _lerp_position_from;
            private UnityEngine.Vector3 _angular_filter_state;
            private UnityEngine.Vector3 _linear_filter_state;
            private float _time_count = 0.0f;
            private float _total_time = 0.0f;
            private bool _bEnd = false;


            private void Awake()
            {

                // disable this script by default
                this.enabled = false;
                //this.name = "ApollonResetController";

            } /* Awake() */

            private void OnEnable()
            {

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.ResetController.OnEnable() : begin"
                );
                
                // preliminary
                if ((this._parent = this.GetComponentInParent<ApollonGeneric3DoFHapticArmBehaviour>()) == null
                )
                {

                    // log
                    UnityEngine.Debug.LogError(
                        "<color=Red>Error: </color> ApollonGeneric3DoFHapticArmBehaviour.ResetController.OnEnable() : failed to get parent/rigidbody reference ! Self disabling..."
                    );

                    // disable
                    this.gameObject.SetActive(false);

                    // return
                    return;

                } /* if() */

                // log
                UnityEngine.Debug.Log(
                    "<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.ResetController.OnEnable() : end"
                );

            } /* OnEnable() */

        } /* class ResetController */

        #endregion

        // Init
        private void Initialize()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.Initialize() : begin");
            
            // instantiate state controller components
            var init       = this.gameObject.AddComponent<InitController>();
            var idle       = this.gameObject.AddComponent<IdleController>();
            var control    = this.gameObject.AddComponent<ControlController>();
            var reset      = this.gameObject.AddComponent<ResetController>();

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.Initialize() : end");

        } /* Initialize() */

        #region MonoBehaviour implementation

        void Awake()
        {

            // behaviour inactive by default & gameobject inactive
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */

        void OnEnable()
        {

            // initialize iff. required
            if (!this.m_bHasInitialized)
            {

                // log
                UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonGeneric3DoFHapticArmBehaviour.OnEnable() : initialize recquired");

                // call
                this.Initialize();

            } /* if() */

            // skip if no experimental setup is found necessary
            if (experiment.ApollonExperimentManager.Instance.Session == null) return;
                        
        } /* OnEnable()*/

        void OnDisable()
        {
            
            // skip it hasn't been initialized 
            if (!this.m_bHasInitialized)
            {
                return;
            }

        } /* OnDisable() */

        #endregion

    } /* public class ApollonGeneric3DoFHapticArmBehaviour */

} /* } Labsim.apollon.gameplay.device.command */

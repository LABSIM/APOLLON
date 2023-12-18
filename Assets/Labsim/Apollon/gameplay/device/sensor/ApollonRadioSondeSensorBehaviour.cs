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
namespace Labsim.apollon.gameplay.device.sensor
{

    public class ApollonRadioSondeSensorBehaviour 
        : ApollonConcreteGameplayBehaviour<ApollonRadioSondeSensorBridge>
    {
        
        [UnityEngine.SerializeField]
        public UnityEngine.GameObject HitPointTracker = null;
        public RadiosondeFrontend RadiosondeFrontend = null;

        #region properties/members

        [UnityEngine.SerializeField]
        protected float SensorHitPointDistance { private set; get; }
        
        [UnityEngine.SerializeField]
        protected UnityEngine.Vector3 SensorHitPointPosition { private set; get; }       
        
        [UnityEngine.SerializeField]
        protected UnityEngine.Quaternion SensorHitPointOrientation { private set; get; }
    
        [UnityEngine.SerializeField]
        private UnityEngine.LayerMask terrainMask;

        #endregion

        private void Initialize()
        {

        } /* Initialize() */

        #region Unity Mono Behaviour implementation

        private void Start()
        {

        }

        private void Awake()
        {

            // inactive by default
            this.enabled = false;
            this.gameObject.SetActive(false);

        } /* Awake() */


        public void OnEnable()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonRadioSondeSensorBehaviour.OnEnable() : call");
            this.enabled = true;
            this.gameObject.SetActive(true);

        } /* OnEnable() */

        private void OnDisable()
        {

            // log
            UnityEngine.Debug.Log("<color=Blue>Info: </color> ApollonRadioSondeSensorBehaviour.OnDisable() : call");

           
        } /* OnDisable() */

        private void FixedUpdate()
        {

            UnityEngine.RaycastHit hit;
            if ( 
                UnityEngine.Physics.Raycast(
                    origin:
                        this.transform.position, 
                    direction:
                        this.transform.TransformDirection(UnityEngine.Vector3.down), 
                    maxDistance:
                        UnityEngine.Mathf.Infinity, 
                    layerMask:
                        this.terrainMask,
                    hitInfo:
                        out hit
                )
            ) 
            {

                // update values 
                this.SensorHitPointDistance     = hit.distance;
                this.SensorHitPointPosition     = hit.point;
                this.SensorHitPointOrientation  = UnityEngine.Quaternion.Euler(hit.normal);

                // update tracker transform
                if(this.HitPointTracker)
                {

                    this.HitPointTracker.transform.SetPositionAndRotation(
                        this.SensorHitPointPosition,
                        this.SensorHitPointOrientation
                    );

                } /* if() */

                if(this.RadiosondeFrontend)
                {
                    this.RadiosondeFrontend.SetValue(
                        //this.transform.position.y - hit.point.y
                        hit.distance
                    );
                }

                // dispatch event
                this.ConcreteBridge.ConcreteDispatcher.RaiseHitChangedEvent(
                    this.SensorHitPointDistance,
                    this.SensorHitPointPosition,
                    this.SensorHitPointOrientation
                );
            
            }
            else
            {

                // log
                UnityEngine.Debug.LogWarning("<color=Orange>Warning: </color> ApollonRadioSondeSensorBehaviour.OnFixedUpdate() : failed to get a raycast hit...");

            } /* if() */

        } /* FixedUpdate() */

        #endregion

    } /* public class ApollonActiveSeatEntityBehaviour */

} /* } Labsim.apollon.gameplay.device.sensor */

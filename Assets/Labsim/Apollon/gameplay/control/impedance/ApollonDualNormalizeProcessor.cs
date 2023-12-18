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
namespace Labsim.apollon.gameplay.control.impedance
{

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public class ApollonDualNormalizeProcessor
        : UnityEngine.InputSystem.InputProcessor<float>
    {

        public float positive_min = 0.5f;
        public float positive_max = 1.0f;
        public float negative_min = -0.5f;
        public float negative_max = -1.0f;

#if UNITY_EDITOR
        static ApollonDualNormalizeProcessor()
        {
            Initialize();
        }
#endif

        [UnityEngine.RuntimeInitializeOnLoadMethod]
        static void Initialize()
        {
            UnityEngine.InputSystem.InputSystem.RegisterProcessor<ApollonDualNormalizeProcessor>();
        }

        public override float Process(float value, UnityEngine.InputSystem.InputControl control)
        {
            // normalize around -> continuous [-1.0, 1.0]
            return 
                (System.Math.Sign(value) > 0) 
                ? ((value - positive_min) / (positive_max - positive_min))
                : (-1.0f * ((value - negative_min) / (negative_max - negative_min)));

        } /* Process() */

    } /* class ApollonDualNormalizeProcessor */

} /* } Labsim.apollon.gameplay.control.impedance */

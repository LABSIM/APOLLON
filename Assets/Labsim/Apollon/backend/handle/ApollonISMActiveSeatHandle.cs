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
namespace Labsim.apollon.backend.handle
{


    public class ApollonISMActiveSeatHandle
        : ApollonAbstractTCPStreamHandle
    {

        protected override ApollonBackendManager.HandleIDType WrapID()
        {
            return ApollonBackendManager.HandleIDType.ApollonISMActiveSeatHandle;;
        }

        public enum messageID : short
        {
            NoMoreData = -1,
            BeginSession,
            EndSession,
            BeginTrial,
            EndTrial,
            Start,
            Stop,
            Reset
        }

        public void BeginSession()
        {

            this.TCPStream.WriteByte(System.Convert.ToByte(messageID.BeginSession));
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISMActiveSeatHandle.BeginSession() : sended [BeginSession]."
            );

        } /* BeginSession() */

        public void EndSession()
        {

            this.TCPStream.WriteByte(System.Convert.ToByte(messageID.EndSession));
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISMActiveSeatHandle.EndSession() : sended [EndSession]."
            );

        } /* EndSession() */

        public void BeginTrial()
        {

            this.TCPStream.WriteByte(System.Convert.ToByte(messageID.BeginTrial));
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISMActiveSeatHandle.BeginTrial() : sended [BeginTrial]."
            );

        } /* BeginTrial() */

        public void EndTrial()
        {

            this.TCPStream.WriteByte(System.Convert.ToByte(messageID.EndTrial));
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISMActiveSeatHandle.EndTrial() : sended [EndTrial]."
            );

        } /* EndTrial() */

        public void Start(double AngularAcceleration, double AngularSpeedSaturation, double MaxStimDuration)
        {
            
            this.TCPStream.WriteByte(System.Convert.ToByte(messageID.Start));

            // payload
            this.TCPStream.Write(System.BitConverter.GetBytes(AngularAcceleration), 0, 8);
            this.TCPStream.Write(System.BitConverter.GetBytes(AngularSpeedSaturation), 0, 8);
            this.TCPStream.Write(System.BitConverter.GetBytes(MaxStimDuration), 0, 8);

            // log
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISMActiveSeatHandle.Start() : sended [Start] with args [dAngularAcceleration:"
                + AngularAcceleration
                + "], [dAngularSpeedSaturation:"
                + AngularSpeedSaturation
                + "], [dMaxStimDuration:"
                + MaxStimDuration
                + "] !"
            );

        } /* Start() */

        public void Stop()
        {

            this.TCPStream.WriteByte(System.Convert.ToByte(messageID.Stop));
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISMActiveSeatHandle.Stop() : sended [Stop]."
            );

        } /* Stop() */

        public void Reset()
        {

            this.TCPStream.WriteByte(System.Convert.ToByte(messageID.Reset));
            UnityEngine.Debug.Log(
                "<color=Blue>Info: </color> ApollonISMActiveSeatHandle.Reset() : sended [Reset]."
            );

        } /* Reset() */

    } /* class ApollonISMActiveSeatHandle */
    
} /* namespace Labsim.apollon.backend */

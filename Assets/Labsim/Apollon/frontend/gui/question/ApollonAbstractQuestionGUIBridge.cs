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
namespace Labsim.apollon.frontend.gui
{

    public abstract class ApollonAbstractQuestionGUIBridge<TBehaviour>
        : ApollonAbstractFrontendBridge
        where TBehaviour : UnityEngine.MonoBehaviour
    {

        protected abstract ApollonFrontendManager.FrontendIDType InnerID { get; }

        protected override UnityEngine.MonoBehaviour WrapBehaviour()
        {

            foreach (UnityEngine.MonoBehaviour behaviour in UnityEngine.Resources.FindObjectsOfTypeAll<TBehaviour>())
            {
                if (behaviour.transform.name == ApollonEngine.GetEnumDescription(this.InnerID))
                {
                    return behaviour;
                }
            }

            // log
            UnityEngine.Debug.LogWarning(
                "<color=Orange>Warning: </color> ApollonAbstractQuestionGUIBridge.WrapBehaviour() : could not find object of type " 
                + typeof(TBehaviour).ToString()
                + " from Unity."
            );

            return null;

        } /* WrapBehaviour() */

        protected override ApollonFrontendManager.FrontendIDType WrapID()
        {
            return this.InnerID;
        }

        public override void onActivationRequested(object sender, ApollonFrontendManager.FrontendEventArgs arg)
        {
            switch (arg.ID)
            {
                case ApollonFrontendManager.FrontendIDType.None:
                    {
                        if (this.Behaviour != null)
                        {
                            this.Behaviour.gameObject.SetActive(false);
                        }
                        else
                        {
                            // put in a queue of corroutines
                        }
                        break;
                    }
                case var _ when arg.ID == this.InnerID:
                case ApollonFrontendManager.FrontendIDType.All:
                    {
                        if (this.Behaviour != null)
                        {
                            this.Behaviour.gameObject.SetActive(true);
                        }
                        else
                        {
                            // put in a queue of corroutines
                        }
                        break;
                    }
                default:
                    break;
            }

        } /* onActivationRequested() */

        public override void onInactivationRequested(object sender, ApollonFrontendManager.FrontendEventArgs arg)
        {
            switch (arg.ID)
            {
                case ApollonFrontendManager.FrontendIDType.None:
                    {
                        if (this.Behaviour != null)
                        {
                            this.Behaviour.gameObject.SetActive(true);
                        }
                        else
                        {
                            // put in a queue of corroutines
                        }
                        break;
                    }
                case var _ when arg.ID == this.InnerID:
                case ApollonFrontendManager.FrontendIDType.All:
                    {
                        if (this.Behaviour != null)
                        {
                            this.Behaviour.gameObject.SetActive(false);
                        }
                        else
                        {
                            // put in a queue of corroutines
                        }
                        break;
                    }
                default:
                    break;
            }

        } /* onInactivationRequested() */

    }  /* abstract class ApollonAbstractQuestionGUIBridge */

    public class ApollonQuestion01GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion01GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question01GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion01GUIBridge */

    
    public class ApollonQuestion02GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion02GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question02GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion02GUIBridge */

    
    public class ApollonQuestion03GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion03GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question03GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion03GUIBridge */

    
    public class ApollonQuestion04GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion04GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question04GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion04GUIBridge */

    
    public class ApollonQuestion05GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion05GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question05GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion05GUIBridge */

    
    public class ApollonQuestion06GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion06GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question06GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion06GUIBridge */

    
    public class ApollonQuestion07GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion07GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question07GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion07GUIBridge */

    
    public class ApollonQuestion08GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion08GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question08GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion08GUIBridge */

    
    public class ApollonQuestion09GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion09GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question09GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion09GUIBridge */

    public class ApollonQuestion10GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion10GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question10GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion09GUIBridge */

    public class ApollonQuestion11GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion11GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question11GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion11GUIBridge */

    
    public class ApollonQuestion12GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion12GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question12GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion12GUIBridge */

    
    public class ApollonQuestion13GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion13GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question13GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion13GUIBridge */

    
    public class ApollonQuestion14GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion14GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question14GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion14GUIBridge */

    
    public class ApollonQuestion15GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion15GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question15GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion15GUIBridge */

    
    public class ApollonQuestion16GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion16GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question16GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion16GUIBridge */

    
    public class ApollonQuestion17GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion17GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question17GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion17GUIBridge */

    
    public class ApollonQuestion18GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion18GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question18GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion18GUIBridge */

    
    public class ApollonQuestion19GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion19GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question19GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion19GUIBridge */    

    public class ApollonQuestion20GUIBridge 
        : ApollonAbstractQuestionGUIBridge<ApollonQuestion20GUIBehaviour>
    {

        private const ApollonFrontendManager.FrontendIDType _innerID = ApollonFrontendManager.FrontendIDType.Question20GUI; 

        protected override ApollonFrontendManager.FrontendIDType InnerID { get { return _innerID;} }  

    } /* class ApollonQuestion20GUIBridge */    

} /* } Labsim.apollon.frontend.gui */
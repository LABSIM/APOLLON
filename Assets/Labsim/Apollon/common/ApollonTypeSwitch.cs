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
namespace Labsim.apollon.common
{

    static class ApollonTypeSwitch
    {

        public class CaseInfo
        {

            public bool IsDefault { get; set; }

            public System.Type Target { get; set; }

            public System.Action<object> Action { get; set; }

        } /* class CaseInfo */

        public static void Do(object source, params CaseInfo[] cases)
        {
            var type = source.GetType();

            foreach (var entry in cases)
            {
                if (entry.IsDefault || type == entry.Target)
                {
                    entry.Action(source);
                    break;
                }
            }
        }

        public static CaseInfo Case<T>(System.Action action)
        {
            return new CaseInfo()
            {
                Action = x => action(),
                Target = typeof(T)
            };
        }

        public static CaseInfo Case<T>(System.Action<T> action)
        {
            return new CaseInfo()
            {
                Action = (x) => action((T)x),
                Target = typeof(T)
            };
        }

        public static CaseInfo Default(System.Action action)
        {
            return new CaseInfo()
            {
                Action = x => action(),
                IsDefault = true
            };
        }

    } /* static class ApollonTypeSwitch */

} /* } Labsim.apollon.common */
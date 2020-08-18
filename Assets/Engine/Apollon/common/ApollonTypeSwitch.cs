
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
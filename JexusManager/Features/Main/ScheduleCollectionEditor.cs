using Microsoft.Web.Administration;
using System;
using System.ComponentModel.Design;

namespace JexusManager.Main.Features.Main
{
#if !NETCOREAPP3_0
    internal class ScheduleCollectionEditor : CollectionEditor
    {
        public ScheduleCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override string GetDisplayText(object value)
        {
            Schedule time = (Schedule)value;
            return time.Time.ToString("c");
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type CreateCollectionItemType()
        {
            return typeof(Schedule);
        }
    }
#endif
}

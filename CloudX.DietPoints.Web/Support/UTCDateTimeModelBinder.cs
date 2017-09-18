using System;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace CloudX.DietPoints.Web.Support
{
    public class UTCDateTimeModelBinder : IModelBinder
    {
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            if (bindingContext.ModelType != typeof (DateTime) && bindingContext.ModelType != typeof (DateTime?))
                return false;

            var val = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (val == null)
                return false;

            var raw = val.RawValue as string;
            if (raw == null)
            {
                bindingContext.ModelState.AddModelError(bindingContext.ModelName, "Wrong value type");
                return false;
            }
            var result = DateTime.Parse(raw);

            if (result.Kind == DateTimeKind.Utc)
                result = result.ToLocalTime();

            bindingContext.Model = result;

            return true;
        }
    }
}
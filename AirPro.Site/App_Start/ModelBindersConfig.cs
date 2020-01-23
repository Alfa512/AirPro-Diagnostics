using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AirPro.Service.DTOs.Interface;
using AirPro.Site.Areas.Admin.Models.VehicleMakes;

namespace AirPro.Site.App_Start
{
    public class ModelBindersConfig
    {
        public static void RegisterBinders(ModelBinderDictionary binders)
        {
            binders.Add(typeof(IVehicleMakeToolDto), new VehicleMakeToolModelBinder());
        }
    }
}
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.Json;

namespace WebApp.Extensions
{
    public static class Extensions
    {

        public static void SetAlert(this ITempDataDictionary tempData, string type, string message)
        {
            tempData["Alerts"] = JsonSerializer.Serialize(new List<AlertModel>
            {
                new AlertModel{ Type = type, Message = message }
            });
        }
    }
}

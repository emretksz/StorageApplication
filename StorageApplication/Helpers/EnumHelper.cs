using Microsoft.AspNetCore.Mvc.Rendering;

namespace StorageApplication.Helpers
{

    public static class EnumHelper
    {
        public static List<SelectListItem> GetEnumSelectList<T>()
        {
            var enumValues = Enum.GetValues(typeof(T)).Cast<T>();
            var selectListItems = enumValues.Select(value => new SelectListItem
            {
                Value = value.ToString(),
                Text = value.ToString()
            }).ToList();

            return selectListItems;
        }
        public class EnumViewModel
        {
            public string WeightSelectedValue { get; set; }
            public string AmountSelectedValue { get; set; }
            public List<SelectListItem> WeightOptions { get; set; }
            public List<SelectListItem> AmountOptions { get; set; }
        }
    }

}

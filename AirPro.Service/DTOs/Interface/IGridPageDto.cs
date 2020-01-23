using System.Collections.Generic;

namespace AirPro.Service.DTOs.Interface
{
    public interface IGridPageDto<T>
    {
        int Current { get; set; }
        int RowCount { get; set; }
        IEnumerable<T> Rows { get; set; }
        int Total { get; set; }
    }
}
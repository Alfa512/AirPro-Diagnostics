using System;
using System.Collections.Generic;
using AirPro.Service.DTOs.Interface;

namespace AirPro.Service.DTOs.Concrete
{
    [Serializable]
    internal class GridPageDto<T> : IGridPageDto<T>
    {
        public int Current { get; set; }
        public int RowCount { get; set; }
        public IEnumerable<T> Rows { get; set; }
        public int Total { get; set; }
    }
}

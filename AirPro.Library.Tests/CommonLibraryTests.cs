using Microsoft.VisualStudio.TestTools.UnitTesting;
using AirPro.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirPro.Library.Tests
{
    [TestClass, ExcludeFromCodeCoverage]
    public class CommonLibraryTests
    {
        [TestMethod()]
        public void ConvertToUserTimeTest()
        {
            Assert.Fail();
            //DateTimeOffset? utc = DateTimeOffset.UtcNow;

            //foreach (TimeZoneInfo info in TimeZoneInfo.GetSystemTimeZones())
            //{
            //    var test = TimeZoneInfo.ConvertTimeFromUtc(utc.Value.DateTime, info);

            //    // Return Date by Info and Info Id.
            //    if (test != CommonLibrary.ConvertToUserTime(utc, info)) Assert.Fail();
            //    if (test != CommonLibrary.ConvertToUserTime(utc, info.Id)) Assert.Fail();
            //    if (test != CommonLibrary.ConvertToUserTime(utc.Value, info)) Assert.Fail();
            //    if (test != CommonLibrary.ConvertToUserTime(utc.Value, info.Id)) Assert.Fail();

            //    // Return Null from Null Dates.
            //    if (null != CommonLibrary.ConvertToUserTime(null, info)) Assert.Fail();
            //    if (null != CommonLibrary.ConvertToUserTime(null, info.Id)) Assert.Fail();
            //}
        }
    }
}
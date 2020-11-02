using DIGNDB.App.SmitteStop.Core.Models;
using NUnit.Framework;
using System.Collections.Generic;
using System.Text;
using DIGNDB.App.SmitteStop.Domain.Dto;

namespace DIGNDB.App.SmitteStop.Testing.ModelTest
{
    [TestFixture]
    public class CacheResultTests
    {
        [Test]
        public void CacheResult_SetFileBytesList_ShouldGetSameValue()
        {
            List<byte[]> expectedFileBytesList = new List<byte[]>() {
                Encoding.ASCII.GetBytes("string1"),
                Encoding.ASCII.GetBytes("string2"),
                Encoding.ASCII.GetBytes("string3"),
            };
            var cacheResult = new CacheResult();
            cacheResult.FileBytesList = expectedFileBytesList;
            Assert.AreEqual(expectedFileBytesList,cacheResult.FileBytesList);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CacheResult_SetFinalForTheDay_ShouldGetSameValue(bool isFinal)
        {
            var cacheResult = new CacheResult();
            cacheResult.FinalForTheDay = isFinal;
            Assert.AreEqual(isFinal, cacheResult.FinalForTheDay);
        }

        [Test]
        public void CacheResult_GetTimestamp_ShouldReturnZero()
        {
            var cacheResult = new CacheResult();
            Assert.AreEqual(0, cacheResult.Timestamp);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void CacheResult_SetNewerFilesExist_ShouldGetSameValue(bool isNewFileExist)
        {
            var cacheResult = new CacheResult();
            cacheResult.NewerFilesExist = isNewFileExist;
            Assert.AreEqual(isNewFileExist, cacheResult.NewerFilesExist);
        }
    }
}

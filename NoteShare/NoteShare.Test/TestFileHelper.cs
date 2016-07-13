using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NoteShare.Resources;

namespace NoteShare.Test
{
    [TestClass]
    public class TestFileHelper
    {
        [TestMethod]
        public void TestSearchFileTypeMap()
        {
            //Arrange
            var type1 = "txt";
            var type2 = "img";
            var type3 = "pdf";
            var type4 = "doc";
            var type5 = "xls";
            var type6 = "ppt";

            //Act
            var result1 = FileHelper.searchFileTypeMap(type1);
            var result2 = FileHelper.searchFileTypeMap(type2);
            var result3 = FileHelper.searchFileTypeMap(type3);
            var result4 = FileHelper.searchFileTypeMap(type4);
            var result5 = FileHelper.searchFileTypeMap(type5);
            var result6 = FileHelper.searchFileTypeMap(type6);

            //Assert
            Assert.AreEqual(1, result1.Count);
            Assert.AreEqual("text/plain", result1[0]);

            Assert.AreEqual(4, result2.Count);
            Assert.AreEqual("image/png", result2[0]);
            Assert.AreEqual("image/gif", result2[1]);
            Assert.AreEqual("image/jpg", result2[2]);
            Assert.AreEqual("image/jpeg", result2[3]);

            Assert.AreEqual(1, result3.Count);
            Assert.AreEqual("application/pdf", result3[0]);

            Assert.AreEqual(2, result4.Count);
            Assert.AreEqual("application/msword", result4[0]);
            Assert.AreEqual("application/vnd.openxmlformats-officedocument.wordprocessingml.document", result4[1]);

            Assert.AreEqual(3, result5.Count);
            Assert.AreEqual("application/excel", result5[0]);
            Assert.AreEqual("application/vnd.ms-excel", result5[1]);
            Assert.AreEqual("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", result5[2]);

            Assert.AreEqual(4, result2.Count);
            Assert.AreEqual("application/powerpoint", result6[0]);
            Assert.AreEqual("application/mspowerpoint", result6[1]);
            Assert.AreEqual("application/vnd.ms-powerpoint", result6[2]);
            Assert.AreEqual("application/vnd.openxmlformats-officedocument.presentationml.presentation", result6[3]);
        }

        [TestMethod]
        public void TestGetFileTypeFromMime()
        {
            //Arrange
            var mime1 = "application/pdf";
            var mime2 = "application/msword";
            var mime3 = "image/gif";
            var mime4 = "text/plain";
            var mime5 = "application/json";

            //Act
            var result1 = FileHelper.getFileTypeEnumFromMime(mime1);
            var result2 = FileHelper.getFileTypeEnumFromMime(mime2);
            var result3 = FileHelper.getFileTypeEnumFromMime(mime3);
            var result4 = FileHelper.getFileTypeEnumFromMime(mime4);
            var result5 = FileHelper.getFileTypeEnumFromMime(mime5);

            //Assert
            Assert.AreEqual(FileTypeEnum.PDF, result1);
            Assert.AreEqual(FileTypeEnum.DOWNLOADONLY, result2);
            Assert.AreEqual(FileTypeEnum.IMAGE, result3);
            Assert.AreEqual(FileTypeEnum.TEXT, result4);
            Assert.AreEqual(FileTypeEnum.TEXT, result5);
        }

        [TestMethod]
        public void TestDownloadFileName()
        {
            //Arrange
            var type1 = "application/pdf";
            var type2 = "application/msword";
            var type3 = "application/excel";
            var type4 = "application/mspowerpoint";
            var type5 = "image/gif";
            var type6 = "image/jpg";
            var type7 = "image/png";
            var type8 = "text/plain";
            var type9 = "asdfasdf";

            //Act
            var result1 = FileHelper.getDownloadFileName(type1);
            var result2 = FileHelper.getDownloadFileName(type2);
            var result3 = FileHelper.getDownloadFileName(type3);
            var result4 = FileHelper.getDownloadFileName(type4);
            var result5 = FileHelper.getDownloadFileName(type5);
            var result6 = FileHelper.getDownloadFileName(type6);
            var result7 = FileHelper.getDownloadFileName(type7);
            var result8 = FileHelper.getDownloadFileName(type8);
            var result9 = FileHelper.getDownloadFileName(type9);

            //Assert
            Assert.AreEqual("download.pdf", result1);
            Assert.AreEqual("download.doc", result2);
            Assert.AreEqual("download.xls", result3);
            Assert.AreEqual("download.ppt", result4);
            Assert.AreEqual("download.gif", result5);
            Assert.AreEqual("download.jpg", result6);
            Assert.AreEqual("download.png", result7);
            Assert.AreEqual("download.txt", result8);
            Assert.AreEqual("download.bin", result9);
        }
    }
}

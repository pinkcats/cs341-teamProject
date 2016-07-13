using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteShare.Resources
{
    public static class FileHelper
    {
        public static List<string> searchFileTypeMap(string userFileType)
        {
            List<string> fileTypes = new List<string>();

            if (userFileType == "txt")
            {
                fileTypes.Add("text/plain");
            }
            else if (userFileType == "img")
            {
                fileTypes.Add("image/png");
                fileTypes.Add("image/gif");
                fileTypes.Add("image/jpg");
                fileTypes.Add("image/jpeg");
            }
            else if (userFileType == "pdf")
            {
                fileTypes.Add("application/pdf");
            }
            else if (userFileType == "doc")
            {
                fileTypes.Add("application/msword");
                fileTypes.Add("application/vnd.openxmlformats-officedocument.wordprocessingml.document");
            }
            else if (userFileType == "xls")
            {
                fileTypes.Add("application/excel");
                fileTypes.Add("application/vnd.ms-excel");
                fileTypes.Add("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
            else if (userFileType == "ppt")
            {
                fileTypes.Add("application/powerpoint");
                fileTypes.Add("application/mspowerpoint");
                fileTypes.Add("application/vnd.ms-powerpoint");
                fileTypes.Add("application/vnd.openxmlformats-officedocument.presentationml.presentation");
            }

            return fileTypes;
        }

        public static FileTypeEnum getFileTypeEnumFromMime(string fileTypeString)
        {
            FileTypeEnum fileType = FileTypeEnum.TEXT;
            switch (fileTypeString)
            {
                case "application/pdf":
                    fileType = FileTypeEnum.PDF;
                    break;
                case "application/msword":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                case "application/excel":
                case "application/mspowerpoint":
                case "application/vnd.ms-excel":
                case "application/powerpoint":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                case "application/vnd.ms-powerpoint":
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                    fileType = FileTypeEnum.DOWNLOADONLY;
                    break;
                case "image/gif":
                case "image/jpg":
                case "image/jpeg":
                case "image/png":
                    fileType = FileTypeEnum.IMAGE;
                    break;
                case "text/plain":
                default:
                    fileType = FileTypeEnum.TEXT;
                    break;

            }

            return fileType;
        }

        public static string getDownloadFileName(string fileType)
        {
            var fileName = "download.bin";
            switch (fileType)
            {
                case "application/pdf":
                    fileName = "download.pdf";
                    break;
                case "application/msword":
                case "application/vnd.openxmlformats-officedocument.wordprocessingml.document":
                    fileName = "download.doc";
                    break;
                case "application/excel":
                case "application/vnd.ms-excel":
                case "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet":
                    fileName = "download.xls";
                    break;
                case "application/mspowerpoint":
                case "application/powerpoint":
                case "application/vnd.ms-powerpoint":
                case "application/vnd.openxmlformats-officedocument.presentationml.presentation":
                    fileName = "download.ppt";
                    break;
                case "image/gif":
                    fileName = "download.gif";
                    break;
                case "image/jpg":
                case "image/jpeg":
                    fileName = "download.jpg";
                    break;
                case "image/png":
                    fileName = "download.png";
                    break;
                case "text/plain":
                    fileName = "download.txt";
                    break;
                default:
                    fileName = "download.bin";
                    break;
            }

            return fileName;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace firstWeb.Core.Helper
{
   public static class FileHelper
    {
        /// <summary>
        /// 检测指定目录是否存在
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public static bool IsExistDirectory(string path)
        {
            return Directory.Exists(path);
        }

        /// <summary>
        /// 创建一个目录
        /// </summary>
        /// <param name="path">路径</param>
        public static void CreateFolder(string path)
        {
            if (!IsExistDirectory(path))
            {
                Directory.CreateDirectory(path);
            }
           
        }

        /// <summary>
        /// 删除文件 
        /// </summary>
        /// <param name="path">路径</param>
       public static void DelectFile(string path)
        {
            if (IsExistDirectory(path))
            {
                Directory.Delete(path);
            }
        }

        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static string GetExtensionName(string fileName)
        {
            int index = fileName.IndexOf('.')+1;
            return fileName.Substring(index);
        }

        
    }
}

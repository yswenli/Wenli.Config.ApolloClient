using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Wenli.Config.ApolloClient.Common
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public static class FileHelper
    {
        static object _locker = new object();

        /// <summary>
        /// 实体持久化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="t"></param>
        public static void Write<T>(string filePath, T t) where T : class, new()
        {
            Write(filePath, JSONHelper.SerializeObject(t));
        }

        /// <summary>
        /// 持久化
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public static void Write(string filePath, string content)
        {
            try
            {
                var fileName = Path.GetFileName(filePath);

                var directory = filePath.Replace(fileName, "");

                if (PathHelper.Create(directory))
                {
                    if (!File.Exists(filePath))
                    {
                        File.Create(filePath).Dispose();
                    }

                    using (var fs = File.Open(filePath, FileMode.Truncate, FileAccess.Write, FileShare.Write))
                    {
                        var bytes = Encoding.UTF8.GetBytes(content);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("FileHelper.Write ex:" + ex.Message);
            }
        }

        /// <summary>
        /// 从文件中读取实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T Read<T>(string filePath) where T : class, new()
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (var fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (var textReader = new StreamReader(fs))
                        {
                            var json = textReader.ReadToEnd();
                            if (!string.IsNullOrEmpty(json))
                            {
                                return JSONHelper.DeserializeObject<T>(json);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("FileHelper.Write ex:" + ex.Message);
            }
            return default(T);
        }

        /// <summary>
        /// 获取缓存名称
        /// </summary>
        /// <param name="path"></param>
        /// <param name="env"></param>
        /// <param name="cluster"></param>
        /// <param name="appID"></param>
        /// <returns></returns>
        public static string GetCacheName(string path, string env, string cluster, string appID)
        {
            if (PathHelper.Create(path))
            {
                var fileName = $"{env}_{cluster}_{appID}.json";

                return Path.Combine(path, fileName);
            }
            return string.Empty;
        }

        /// <summary>
        /// 读取集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Dictionary<string, T> ReadCollection<T>(string path) where T : class, new()
        {
            try
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*.json");

                    if (files != null)
                    {
                        var dic = new Dictionary<string, T>();

                        foreach (var file in files)
                        {
                            var fileName = Path.GetFileNameWithoutExtension(file);

                            var data = Read<T>(file);

                            dic[fileName] = data;
                        }
                        return dic;
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("FileHelper.ReadCollection ex:" + ex.Message);
            }
            return null;
        }


    }
    /// <summary>
    /// 目录工具类
    /// </summary>
    public static class PathHelper
    {
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static bool Create(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch { }
            return false;
        }



    }
}

using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Reporting.Service.Core.Common;
using System.Net;
using System.IO;

namespace Reporting.Service.Core
{
    public class Runtime
    {
        #region Singleton para el acceso al runtime

        public static readonly Runtime Instance = null;

        static Runtime()
        {
            Runtime.Instance = new Runtime();
        }

        #endregion
        
        private User _usuario { get; set; }
        public User Usuario
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return this._usuario;
                }
                else
                {
                    return HttpContext.Current.Session["__usr"] as User;
                }
            }
            set
            {
                if (HttpContext.Current == null)
                {
                    this._usuario = value;
                }
                else
                {
                    HttpContext.Current.Session["__usr"] = value;
                }
            }
        }

        
        public string UrlImage
        {
            get
            {
                return ConfigurationManager.AppSettings["Noticias.Image.url"];
            }
        }

        public static string GetUrlImagenUsuario(int Codigo)
        {
            if (ExistImage("http://192.168.2.188/ImagenesWEB/Empleados/" + Codigo.ToString() + ".jpg"))
                return "http://192.168.2.188/ImagenesWEB/Empleados/" + Codigo.ToString() + ".jpg";
            else if (ExistImage("http://192.168.2.188/ImagenesWEB/Empleados/" + Codigo.ToString() + ".png"))
                return "http://192.168.2.188/ImagenesWEB/Empleados/" + Codigo.ToString() + ".png";
            else
                return "http://192.168.2.188/ImagenesWEB/Empleados/default.jpg";

        }
        private static bool ExistImage(string url)
        {
            HttpWebResponse response = null;
            var request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "HEAD";
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                return true;
            }
            catch (WebException ex)
            {
                /* A WebException will be thrown if the status of the response is not `200 OK` */
                return false;
            }
            finally
            {
                // Don't forget to close your response.
                if (response != null)
                {
                    response.Close();
                }
            }
        }

        public string CurrentServerTempPath
        {
            get
            {
                string current = Path.Combine(HttpContext.Current.Server.MapPath("~/DataRepository/Temp/"), HttpContext.Current.Session.SessionID, this.tipoarchivo);

                if (!Directory.Exists(current))
                    Directory.CreateDirectory(current);
                return current;
            }
        }

        public void ClearTempFiles()
        {
            List<string> files = this.CurrentTempFiles;

            while (files.Count != 0)
            {
                this.DeleteTempFile(files[0]);
            }
        }

        public List<string> CurrentTempFiles
        {
            get
            {
                List<string> files = null;
                if (files == null)
                {
                    if ((files = HttpContext.Current.Session["__curFiles"] as List<string>) == null)
                    {
                        HttpContext.Current.Session["__curFiles"] = files = new List<string>();
                    }
                }

                return files;
            }
        }

        public void DeleteTempFile(string name)
        {
            string fullPath = Path.Combine(this.CurrentServerTempPath, name);

            File.Delete(fullPath);

            this.CurrentTempFiles.Remove(name);
        }

        public void AddTempFile(string name, byte[] data, FileMode action, bool replace = true)
        {
            string fullPath = Path.Combine(this.CurrentServerTempPath, name);
            if (File.Exists(fullPath) && action == FileMode.Create && replace) throw new FileLoadException(string.Format("El archivo temporal {0} ya existe en el servidor.", name));

            using (var fs = new System.IO.FileStream(fullPath, action == FileMode.Create ? System.IO.FileMode.OpenOrCreate : (action == FileMode.Append ? System.IO.FileMode.Append : FileMode.Truncate)))
            {
                fs.Write(data, 0, data.Length);
            }

            if (!this.CurrentTempFiles.Contains(name))
            {
                this.CurrentTempFiles.Add(name);
            }
            //the next line was in the "if", this was edited by mike
            this.LastTempFullFile = fullPath;
        }

        public string LastTempFullFile
        {
            get
            {
                return (string)HttpContext.Current.Session["__ltffile"];
            }
            set
            {
                HttpContext.Current.Session["__ltffile"] = value;
            }
        }
        public string tipoarchivo { get; set; }

        public string CurrentVirtualTempPath
        {
            get
            {
                string current = string.Format("~/DataRepository/Temp/{0}/{1}", HttpContext.Current.Session.SessionID, this.tipoarchivo);

                return current;
            }
        }
    }
}

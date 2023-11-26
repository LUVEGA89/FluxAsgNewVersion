using Reporting.Service.Core.Image;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.News
{
    public class NewsManager : DataRepository
    {
        public List<News> GetNews(DocumentStatus Estado, DocumentCategory Categoria)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNoticia");
            if (Categoria != DocumentCategory.Todos)
            {
                this.Database.AddInParameter(cmd, "@Categoria", DbType.Int32, Categoria);
                this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, Estado);
            }

            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<News> ListNotice = new List<News>();
            while (dr.Read())
            {
                ListNotice.Add(new News()
                {
                    Sequence = (int)dr["Sequence"],
                    Epigrafe = (DBNull.Value.Equals(dr["Epigrafe"])) ? string.Empty : (string)dr["Epigrafe"],
                    AnteTitutar = (DBNull.Value.Equals(dr["AnteTitutar"])) ? string.Empty : (string)dr["AnteTitutar"],
                    Titular = (string)dr["Titular"],
                    Subtitulo = (DBNull.Value.Equals(dr["Subtitulo"])) ? string.Empty : (string)dr["Subtitulo"],
                    Entradilla = (DBNull.Value.Equals(dr["Entradilla"])) ? string.Empty : (string)dr["Entradilla"],
                    Cuerpo = (string)dr["Cuerpo"],
                    UrlAudio = (DBNull.Value.Equals(dr["UrlAudio"])) ? string.Empty : (string)dr["UrlAudio"],
                    Categoria = (DocumentCategory)dr["Categoria"],
                    Estado = (DocumentStatus)dr["Estado"],
                    RegistradoPor = (DBNull.Value.Equals(dr["RegistradoPor"])) ? 0 : (int)dr["RegistradoPor"],
                    RegistradoEl = (DBNull.Value.Equals(dr["RegistradoEl"])) ? DateTime.Now : (DateTime)dr["RegistradoEl"],
                    ModificadoPor = (DBNull.Value.Equals(dr["ModificadoPor"])) ? 0 : (int)dr["ModificadoPor"],
                    ModificadoEl = (DBNull.Value.Equals(dr["ModificadoEl"])) ? DateTime.Now : (DateTime)dr["ModificadoEl"],
                    Imagen = GetImage((int)dr["Sequence"]),
                    Eliminado = (bool)dr["Eliminado"],
                    Destacado = (bool)dr["Destacado"]
                });
            }
            return ListNotice;
        }

        public List<News> GetToDoList()
        {

            DbCommand cmd = this.Database.GetStoredProcCommand("prGetToDoList");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<News> ListNotice = new List<News>();
            while (dr.Read())
            {
                ListNotice.Add(new News()
                {
                    Sequence = (int)dr["Sequence"],
                    Titular = (string)dr["Titular"],
                    RegistradoEl = (DBNull.Value.Equals(dr["RegistradoEl"])) ? DateTime.Now : (DateTime)dr["RegistradoEl"]
                });
            }
            return ListNotice;
        }
        public News GetNotice(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetNews");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            News Notice = new News();
            while (dr.Read())
            {
                Notice.Sequence = (int)dr["Sequence"];
                Notice.Epigrafe = (DBNull.Value.Equals(dr["Epigrafe"])) ? string.Empty : (string)dr["Epigrafe"];
                Notice.AnteTitutar = (DBNull.Value.Equals(dr["AnteTitutar"])) ? string.Empty : (string)dr["AnteTitutar"];
                Notice.Titular = (string)dr["Titular"];
                Notice.Subtitulo = (DBNull.Value.Equals(dr["Subtitulo"])) ? string.Empty : (string)dr["Subtitulo"];
                Notice.Entradilla = (DBNull.Value.Equals(dr["Entradilla"])) ? string.Empty : (string)dr["Entradilla"];
                Notice.Cuerpo = (string)dr["Cuerpo"];
                Notice.UrlAudio = (DBNull.Value.Equals(dr["UrlAudio"])) ? string.Empty : (string)dr["UrlAudio"];
                Notice.Categoria = (DocumentCategory)dr["Categoria"];
                Notice.Estado = (DocumentStatus)dr["Estado"];
                Notice.RegistradoPor = (DBNull.Value.Equals(dr["RegistradoPor"])) ? 0 : (int)dr["RegistradoPor"];
                Notice.RegistradoEl = (DBNull.Value.Equals(dr["RegistradoEl"])) ? DateTime.Now : (DateTime)dr["RegistradoEl"];
                Notice.ModificadoPor = (DBNull.Value.Equals(dr["ModificadoPor"])) ? 0 : (int)dr["ModificadoPor"];
                Notice.ModificadoEl = (DBNull.Value.Equals(dr["ModificadoEl"])) ? DateTime.Now : (DateTime)dr["ModificadoEl"];
                Notice.Imagen = GetImage(Notice.Sequence);
                Notice.Eliminado = (bool)dr["Eliminado"];
                Notice.Destacado = (bool)dr["Destacado"];
            }
            return Notice;
        }
        public Imagen GetImage(int Sequence)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetImagenNoticia");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Sequence);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            Imagen Imagen = new Imagen();
            while (dr.Read())
            {
                    Imagen.Sequence = (int)dr["Sequence"];
                    Imagen.Url = Runtime.Instance.UrlImage + (string)dr["Url"];
                    Imagen.Alt = (DBNull.Value.Equals(dr["Alt"])) ? string.Empty : (string)dr["Alt"];
                    Imagen.Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? string.Empty : (string)dr["Comentario"];
                    Imagen.Seccion = (DBNull.Value.Equals(dr["Seccion"])) ? string.Empty : (string)dr["Seccion"];
                    Imagen.Tipo = (ImagenType)dr["Tipo"];
                
            }
            return Imagen;
        }

        public void AddNews(News news)
        {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddNoticia");
                this.Database.AddInParameter(cmd, "@Epigrafe", DbType.String, news.Epigrafe);
                this.Database.AddInParameter(cmd, "@AnteTitutar", DbType.String, news.AnteTitutar);
                this.Database.AddInParameter(cmd, "@Titular", DbType.String, news.Titular);
                this.Database.AddInParameter(cmd, "@Subtitulo", DbType.String, news.Subtitulo);
                this.Database.AddInParameter(cmd, "@Entradilla", DbType.String, news.Entradilla);
                this.Database.AddInParameter(cmd, "@Cuerpo", DbType.String, news.Cuerpo);
                this.Database.AddInParameter(cmd, "@UrlAudio", DbType.String, news.UrlAudio);
                this.Database.AddInParameter(cmd, "@Categoria", DbType.Int32, news.Categoria);
                this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, news.Estado);
                this.Database.AddInParameter(cmd, "@Destacado", DbType.Int32, news.Destacado);
                this.Database.AddInParameter(cmd, "@RegistradoPor", DbType.Int32, news.RegistradoPor);


                this.Database.ExecuteNonQuery(cmd);
            

        }
        public void UpdateNews(News news)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prUpdateNoticia");
                this.Database.AddInParameter(cmd, "@Sequence", DbType.String, news.Sequence);
                this.Database.AddInParameter(cmd, "@Epigrafe", DbType.String, news.Epigrafe);
                this.Database.AddInParameter(cmd, "@AnteTitutar", DbType.String, news.AnteTitutar);
                this.Database.AddInParameter(cmd, "@Titular", DbType.String, news.Titular);
                this.Database.AddInParameter(cmd, "@Subtitulo", DbType.String, news.Subtitulo);
                this.Database.AddInParameter(cmd, "@Entradilla", DbType.String, news.Entradilla);
                this.Database.AddInParameter(cmd, "@Cuerpo", DbType.String, news.Cuerpo);
                this.Database.AddInParameter(cmd, "@UrlAudio", DbType.String, news.UrlAudio);
                this.Database.AddInParameter(cmd, "@Categoria", DbType.Int32, news.Categoria);
                this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, news.Estado);
                this.Database.AddInParameter(cmd, "@Destacado", DbType.Int32, news.Destacado);
                this.Database.AddInParameter(cmd, "@ModificadoPor", DbType.Int32, news.RegistradoPor);

                this.Database.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {

            }

        }
        public List<News> GetTopNews(DocumentStatus documentStatus, DocumentCategory documentCategory)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetTopNoticias");
            this.Database.AddInParameter(cmd, "@Estado", DbType.Int32, documentStatus);
            if (documentCategory != DocumentCategory.Todos)
            {
                this.Database.AddInParameter(cmd, "@Categoria", DbType.Int32, documentCategory);
            }
            
            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<News> ListNotice = new List<News>();
            while (dr.Read())
            {
                ListNotice.Add(new News()
                {
                    Sequence = (int)dr["Sequence"],
                    Epigrafe = (DBNull.Value.Equals(dr["Epigrafe"])) ? string.Empty : (string)dr["Epigrafe"],
                    AnteTitutar = (DBNull.Value.Equals(dr["AnteTitutar"])) ? string.Empty : (string)dr["AnteTitutar"],
                    Titular = (string)dr["Titular"],
                    Subtitulo = (DBNull.Value.Equals(dr["Subtitulo"])) ? string.Empty : (string)dr["Subtitulo"],
                    Entradilla = (DBNull.Value.Equals(dr["Entradilla"])) ? string.Empty : (string)dr["Entradilla"],
                    Cuerpo = (string)dr["Cuerpo"],
                    UrlAudio = (DBNull.Value.Equals(dr["UrlAudio"])) ? string.Empty : (string)dr["UrlAudio"],
                    Categoria = (DocumentCategory)dr["Categoria"],
                    Estado = (DocumentStatus)dr["Estado"],
                    RegistradoPor = (DBNull.Value.Equals(dr["RegistradoPor"])) ? 0 : (int)dr["RegistradoPor"],
                    RegistradoEl = (DBNull.Value.Equals(dr["RegistradoEl"])) ? DateTime.Now : (DateTime)dr["RegistradoEl"],
                    ModificadoPor = (DBNull.Value.Equals(dr["ModificadoPor"])) ? 0 : (int)dr["ModificadoPor"],
                    ModificadoEl = (DBNull.Value.Equals(dr["ModificadoEl"])) ? DateTime.Now : (DateTime)dr["ModificadoEl"],
                    Imagen = GetImage((int)dr["Sequence"]),
                    Eliminado = (bool)dr["Eliminado"],
                    Destacado = (bool)dr["Destacado"]
                });
            }
            return ListNotice;
        }

        public void DeleteNews(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteNews");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, id);
            IDataReader dr = this.Database.ExecuteReader(cmd);
        }
    }
}

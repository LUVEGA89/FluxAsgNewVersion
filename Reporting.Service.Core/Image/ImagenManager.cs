using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Image
{
    public class ImagenManager : DataRepository
    {
        public List<Imagen> GetImagen(ImagenType Tipo)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetImagen");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<Imagen> ListImagenes = new List<Imagen>();
            while (dr.Read())
            {
                ListImagenes.Add(new Imagen()
                {
                    Sequence = (int)dr["Sequence"],
                    Url = (string)dr["Url"],
                    Alt = (DBNull.Value.Equals(dr["Alt"])) ? string.Empty : (string)dr["Alt"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? string.Empty : (string)dr["Comentario"],
                    Seccion = (DBNull.Value.Equals(dr["Seccion"])) ? string.Empty : (string)dr["Seccion"],
                    Tipo = (ImagenType)dr["Tipo"]
                });
            }
            return ListImagenes;
        }
        public void AddImagen(Imagen img)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddImagen");
                this.Database.AddInParameter(cmd, "@Url", DbType.String, img.Url);
                this.Database.AddInParameter(cmd, "@Alt", DbType.String, img.Alt);
                this.Database.AddInParameter(cmd, "@Comentario", DbType.String, img.Comentario);
                this.Database.AddInParameter(cmd, "@Seccion", DbType.String, img.Seccion);
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, img.Tipo);
                if(img.Tipo == ImagenType.Normal)
                    this.Database.AddInParameter(cmd, "@Noticia", DbType.Int32, img.Noticia);

                this.Database.ExecuteNonQuery(cmd);
            }catch(Exception ex){

            }
            
        }

        public void DeleteImage(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteImage");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, id);
            this.Database.ExecuteNonQuery(cmd);
        }



        public void DeleteImageToNews(int Id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteImageNews");
            this.Database.AddInParameter(cmd, "@Noticia", DbType.Int32, Id);
            this.Database.ExecuteNonQuery(cmd);
        }
    }
}

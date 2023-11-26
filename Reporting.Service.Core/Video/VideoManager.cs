using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.Video
{
    public class VideoManager : DataRepository
    {
        public List<Video> GetVideo(VideoType Tipo)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetVideo");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Tipo);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<Video> ListVideo = new List<Video>();
            while (dr.Read())
            {
                ListVideo.Add(new Video()
                {
                    Sequence = (int)dr["Sequence"],
                    Url = (string)dr["Url"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? string.Empty : (string)dr["Comentario"],
                    Tipo = (VideoType)dr["Tipo"]
                });
            }
            return ListVideo;
        }
        public void AddVideo(Video img)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddVideo");
                this.Database.AddInParameter(cmd, "@Url", DbType.String, img.Url);
                this.Database.AddInParameter(cmd, "@Comentario", DbType.String, img.Comentario);
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, img.Tipo);

                this.Database.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {

            }

        }

        public IList<Video> GetVideo()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetAllVideo");

            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<Video> ListVideo = new List<Video>();
            while (dr.Read())
            {
                ListVideo.Add(new Video()
                {
                    Sequence = (int)dr["Sequence"],
                    Url = (string)dr["Url"],
                    Comentario = (DBNull.Value.Equals(dr["Comentario"])) ? string.Empty : (string)dr["Comentario"],
                    Tipo = (VideoType)dr["Tipo"]
                });
            }
            return ListVideo;
        }

        public void DeleteVideo(int Id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteVideo");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Id);
            this.Database.ExecuteNonQuery(cmd);
        }
    }
}

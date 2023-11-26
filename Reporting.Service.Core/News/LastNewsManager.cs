using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reporting.Service.Core.News
{
    public class LastNewsManager : DataRepository
    {
        public List<LastNews> GetLastNews()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetLastNews");
            IDataReader dr = this.Database.ExecuteReader(cmd);
            List<LastNews> ListLastNews = new List<LastNews>();
            while (dr.Read())
            {
                ListLastNews.Add(new LastNews()
                {
                    Sequence = (int)dr["Sequence"],
                    Summary = (string)dr["Summary"],
                    UrlShowMore = (string)dr["UrlShowMore"],
                    Enabled = (bool)dr["Enabled"],
                });
            }
            return ListLastNews;
        }
        public void AddLastNews(LastNews News)
        {
            try
            {
                DbCommand cmd = this.Database.GetStoredProcCommand("prAddLastNews");
                this.Database.AddInParameter(cmd, "@Summary", DbType.String, News.Summary);
                this.Database.AddInParameter(cmd, "@UrlShowMore", DbType.String, News.UrlShowMore);
                this.Database.AddInParameter(cmd, "@Enabled", DbType.Int16,((News.Enabled == true) ? 1 : 0 ));

                this.Database.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {

            }

        }

        public void DeleteLastNews(int Id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prDeleteLastNews");
            this.Database.AddInParameter(cmd, "@Sequence", DbType.Int32, Id);
            this.Database.ExecuteNonQuery(cmd);
        }
    }
}

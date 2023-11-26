using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Requerimiento.TipoReq
{
    public class TipoRequerimientoManager : Catalog<TipoRequerimiento, int, TipoRequerimientoCriteria>
    {

        Buzon.Area.AreaManager AreaManager = new Buzon.Area.AreaManager();
        public TipoRequerimientoManager()
            : base()
        {

        }
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override TipoRequerimiento LoadItem(IDataReader dr)
        {
            return new TipoRequerimiento()
            {
                Identifier = (int)dr["Identifier"],
                Descripcion = (string)dr["Descripcion"],
                Area = AreaManager.Find(int.Parse(dr["Area"].ToString()))
            };
        }

        protected override DbCommand PrepareAddStatement(TipoRequerimiento item)
        {
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prAddRequerimiento");
            this.Database.AddInParameter(DbCommand, "@Descripcion", DbType.String, item.Descripcion);
            this.Database.AddInParameter(DbCommand, "@Area", DbType.Int32, item.Area.Identifier);
            return DbCommand;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prDelRequerimiento");
            this.Database.AddInParameter(DbCommand, "@Identifier", DbType.Int32, id);
            return DbCommand;
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prGetRequerimiento");
            this.Database.AddInParameter(DbCommand, "@Identifier", DbType.Int32, id);
            return DbCommand;
        }

        protected override DbCommand PrepareUpdateStatement(TipoRequerimiento item)
        {
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prUpdRequerimiento");
            this.Database.AddInParameter(DbCommand, "@Identifier", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(DbCommand, "@Descripcion", DbType.String, item.Descripcion);
            return DbCommand;
        }

        public List<TipoRequerimiento> GetRequerimientos(TipoRequerimientoCriteria criteria)
        {
            var Lista = new List<TipoRequerimiento>();
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prFindRequerimiento");
            this.Database.AddInParameter(DbCommand, "@Estatus", DbType.Int32, criteria.Estatus);
            if(string.IsNullOrWhiteSpace(criteria.Area.ToString()) || criteria.Area == 0)
            {
                this.Database.AddInParameter(DbCommand, "@Area", DbType.Int32, null);
            }
            else
            {
                this.Database.AddInParameter(DbCommand, "@Area", DbType.Int32, criteria.Area);
            }
            
            IDataReader dr = this.Database.ExecuteReader(DbCommand);
            while (dr.Read())                
            {
                Lista.Add(this.LoadItem(dr));
            }
            return Lista;
        }
    }
}

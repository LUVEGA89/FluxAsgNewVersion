using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Buzon
{
    public class BuzonManager : Catalog<Buzon, int, BuzonCriteria>
    {
        public int Categoria { get; set; }

        public Core.Buzon.Area.AreaManager areaManager = new Area.AreaManager();

        public Core.Buzon.Categoria.CategoriaManager categoriaManager = new Categoria.CategoriaManager();

        public BuzonManager()
            : base()
        {

        }

        public BuzonManager(string Database)
            : base(Database)
        {

        }
        public BuzonManager(Database Database)
            : base(Database)
        {

        }

        protected override string FindPagedItemsProcedure => "prFindBuzon";

        protected override Buzon LoadItem(IDataReader dr)
        {
            Buzon item = new Buzon();
            item.Identifier = (int)dr["Identificador"];
            item.Nombre = (string)dr["Nombre"];
            item.Sugerencia = (string)dr["Sugerencia"];
            item.Tipo = (BuzonKind)dr["Tipo"];
            item.RegistradoEl = (DateTime)dr["RegistradoEl"];
            // valida si la categoria no viene nulo
            item.Categoria = (DBNull.Value.Equals(dr["Categoria"])) ? null : categoriaManager.Find((int)dr["Categoria"]);
            item.Area = (DBNull.Value.Equals(dr["Area"])) ? null : areaManager.Find((int)dr["Area"]);
            item.Sucursal = (DBNull.Value.Equals(dr["Sucursal"])) ? "" : (string)dr["Sucursal"];
            item.Estatus = (bool)dr["Estatus"];

            return item;

        }

        protected override DbCommand PrepareAddStatement(Buzon item)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prAddBuzon");

            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, item.Nombre);

            this.Database.AddInParameter(cmd, "@Categoria", DbType.Int32, item.Categoria.Identifier);

            if (item.Area != null)
            {
                this.Database.AddInParameter(cmd, "@Area", DbType.Int32, item.Area.Identifier);
            }
            this.Database.AddInParameter(cmd, "@Sugerencia", DbType.String, item.Sugerencia);

            if (!string.IsNullOrWhiteSpace(item.Sucursal))
            {
                this.Database.AddInParameter(cmd, "@Sucursal", DbType.String, item.Sucursal);
            }
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, item.Tipo);

            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(BuzonCriteria Criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(Criteria);
            if(Criteria.Tipo != 0)
            {
                this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, Criteria.Tipo);
            }
            if(Criteria.Inicio != null && Criteria.Termino != null)
            {
                this.Database.AddInParameter(cmd, "@Inicio", DbType.DateTime, Criteria.Inicio);
                this.Database.AddInParameter(cmd, "@Termino", DbType.DateTime, Criteria.Termino);
            }
            return cmd;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBuzon");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int32, id);
            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(Buzon item)
        {
            throw new NotImplementedException();
        }


        public DataTable GetReporteBuzonDay()
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetBuzonReporteDay");            
            cmd.CommandTimeout = 2000;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            DataTable dt = new DataTable();
            dt.Load(dr);
            return dt;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Requerimiento.Concepto
{
    public class ConceptoManager : Catalog<Concepto, int, ConceptoCritetria>
    {
        TipoReq.TipoRequerimientoManager TipoRequerimientoManager = new TipoReq.TipoRequerimientoManager();
        public ConceptoManager()
            : base()
        {

        }

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override Concepto LoadItem(IDataReader dr)
        {
            return new Concepto()
            {
                Identifier = (int)dr["Identifier"],
                Descripcion = (string)dr["Descripcion"],
                TipoRequerimiento = TipoRequerimientoManager.Find(int.Parse(dr["Requerimiento"].ToString()))
            };
        }

        protected override DbCommand PrepareAddStatement(Concepto item)
        {
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prAddConcepto");
            this.Database.AddInParameter(DbCommand, "@Descripcion", DbType.String, item.Descripcion);
            this.Database.AddInParameter(DbCommand, "@TipoRequerimiento", DbType.Int32, item.TipoRequerimiento.Identifier);
            return DbCommand;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prDelConcepto");
            this.Database.AddInParameter(DbCommand, "@Identifier", DbType.Int32, id);
            return DbCommand;
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prGetConcepto");
            this.Database.AddInParameter(DbCommand, "@Identifier", DbType.Int32, id);
            return DbCommand;
        }

        protected override DbCommand PrepareUpdateStatement(Concepto item)
        {
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prUpdConcepto");
            this.Database.AddInParameter(DbCommand, "@Identifier", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(DbCommand, "@Descripcion", DbType.String, item.Descripcion);
            return DbCommand;
        }

        public List<Concepto> GetRequerimientos(ConceptoCritetria criteria)
        {
            var Lista = new List<Concepto>();
            DbCommand DbCommand = this.Database.GetStoredProcCommand("Request.prFindConcepto");
            this.Database.AddInParameter(DbCommand, "@Estatus", DbType.Int32, criteria.Estatus);
            if(criteria.TipoRequerimiento == 0)
            {
                this.Database.AddInParameter(DbCommand, "@TipoRequerimiento", DbType.Int32, null);
            }
            else
            {
                this.Database.AddInParameter(DbCommand, "@TipoRequerimiento", DbType.Int32, criteria.TipoRequerimiento);
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

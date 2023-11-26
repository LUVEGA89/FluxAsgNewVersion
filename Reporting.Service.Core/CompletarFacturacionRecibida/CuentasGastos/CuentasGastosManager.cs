using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.CompletarFacturacionRecibida.CuentasGastos
{
    public class CuentasGastosManager : Catalog<CuentasGastos, string, CuentasGastosCriteria>
    {
        private string _empresa;
        public CuentasGastosManager(string Empresa)
        {
            _empresa = Empresa;
        }

        protected override string FindPagedItemsProcedure => "prFindCuentasGastos";

        protected override CuentasGastos LoadItem(IDataReader dr)
        {
            CuentasGastos cuentasGastos = new CuentasGastos();

            cuentasGastos.Identifier = (string)dr["AcctCode"];
            cuentasGastos.ActId = (string)dr["ActId"];
            cuentasGastos.AcctName = (string)dr["AcctName"];

            return cuentasGastos;
        }

        protected override DbCommand PrepareAddStatement(CuentasGastos item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetCuentaGastos");
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, _empresa);
            this.Database.AddInParameter(cmd, "@Id", DbType.String, id);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(CuentasGastos item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindPagedItemsStatement(CuentasGastosCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, criteria.Empresa);

            return cmd;
        }

        public string GetCodigoImpuesto(string CodigoSAT, decimal TasaoCuota, decimal LimiteInferior = 0.0m, decimal LimiteSuperior = 0.0m)
        {
            string Codigo = null;
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetImpuestoSap");
            this.Database.AddInParameter(cmd, "@CodigoSAT", DbType.String, CodigoSAT);
            this.Database.AddInParameter(cmd, "@TasaoCuota", DbType.Decimal, TasaoCuota);
            if(LimiteInferior != 0.0m)
                this.Database.AddInParameter(cmd, "@LimiteInferior", DbType.Decimal, LimiteInferior);
            if (LimiteSuperior != 0.0m)
                this.Database.AddInParameter(cmd, "@LimiteSuperior", DbType.Decimal, LimiteSuperior);
            this.Database.AddInParameter(cmd, "@Empresa", DbType.String, _empresa);

            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Codigo = (string)dr["CodigoSapImpuesto"];  
            }
            return Codigo;
        }
    }
}

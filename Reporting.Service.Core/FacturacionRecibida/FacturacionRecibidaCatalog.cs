using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.FacturacionRecibida
{
    public class FacturacionRecibidaCatalog : Catalog<FacturacionRecibida, Int64, FacturacionRecibidaCriteria>
    {
        private int aux;
        private string rfc;

        public FacturacionRecibidaCatalog(FacturacionKind Tipo, string rfc = "")
        {
            this.aux = (Int32)Tipo;
            this.rfc = rfc;
        }

        protected override string FindPagedItemsProcedure => "prFindFacturacionRecibida";

        protected override FacturacionRecibida LoadItem(IDataReader dr)
        {

            FacturacionRecibida _fac = new FacturacionRecibida();

            _fac.Identifier = (long)dr["Sequence"];
            _fac.Uuid = (string)dr["Uuid"];
            _fac.Tipo = (string)dr["Tipo"];
            _fac.RfcReceptor = (string)dr["RfcReceptor"];
            _fac.NombreReceptor = dr["NombreReceptor"] == DBNull.Value ? "" : (string)dr["NombreReceptor"];
            _fac.RfcEmisor = (string)dr["RfcEmisor"];
            _fac.NombreEmisor = (string)dr["NombreEmisor"];
            _fac.UsoCfdi = dr["UsoCfdi"] == DBNull.Value ? "" : (string)dr["UsoCfdi"];
            _fac.MetodoPago = dr["MetodoPago"] == DBNull.Value ? "" : (string)dr["MetodoPago"];
            _fac.FormaPago = dr["FormaPago"] == DBNull.Value ? "" : (string)dr["FormaPago"];
            _fac.Serie = dr["Serie"] == DBNull.Value ? "" : (string)dr["Serie"];
            _fac.Folio = dr["Folio"] == DBNull.Value ? "" : (string)dr["Folio"];

            _fac.Subtotal = (decimal)dr["Subtotal"];
            _fac.Retenciones = (decimal)dr["Retenciones"];
            _fac.Traslados = (decimal)dr["Traslados"];
            _fac.Total = (decimal)dr["Total"];
            _fac.FechaTimbrado = (DateTime)dr["Timbrado"];
            _fac.Estatus = (string)dr["EstatusDocumento"];
            _fac.Sucursal = dr["Sucursal"] == DBNull.Value ? "" : (string)dr["Sucursal"];

            //if (this.aux == 1)
            //{
                _fac.SapDoc = dr["DocNum"] == DBNull.Value ? 0 : (int)dr["DocNum"];
                _fac.FechaCaptura = dr["DocDate"] == DBNull.Value ? null : (DateTime?)dr["DocDate"];
                _fac.Monto = dr["DocTotal"] == DBNull.Value ? 0.0m : (decimal)dr["DocTotal"];
            _fac.Repetidos = dr["Repetidos"] == DBNull.Value ? "" : (string)dr["Repetidos"];
            _fac.NcRepetidos = dr["NcRepetidos"] == DBNull.Value ? "" : (string)dr["NcRepetidos"];
            //}

            return _fac;
        }

        protected override DbCommand PrepareAddStatement(FacturacionRecibida item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(long id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(long id)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetFacturaRecibida");
            this.Database.AddInParameter(cmd, "@Id", DbType.Int64, id);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, this.aux);
            //if (this.aux == 1)//Si es facturación recibida agregamos el parametro de la empresa
                this.Database.AddInParameter(cmd, "Empresa", DbType.String, this.rfc);

            return cmd;
        }

        protected override DbCommand PrepareFindPagedItemsStatement(FacturacionRecibidaCriteria criteria)
        {
            DbCommand cmd = base.PrepareFindPagedItemsStatement(criteria);
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, criteria.Al);
            this.Database.AddInParameter(cmd, "@RfcReceptor", DbType.String, criteria.RfcReceptor);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, (Int32)criteria.Tipo);
            this.Database.AddInParameter(cmd, "@TipoComprobante", DbType.Int32, criteria.TipoComprobante);

            return cmd;
        }

        protected override DbCommand PrepareUpdateStatement(FacturacionRecibida item)
        {
            throw new NotImplementedException();
        }

        public List<string> FindEmisores(FacturacionKind tipo)
        {
            List<string> emisores = new List<string>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetEmisores");
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, tipo);
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                emisores.Add((string)dr["RfcReceptor"]);
            }
            return emisores;
        }

        public FacturacionRecibida[] FindPagedItemsSteuben(FacturacionRecibidaCriteria criteria)
        {
            List<FacturacionRecibida> list = new List<FacturacionRecibida>();
            DbCommand cmd = this.Database.GetStoredProcCommand("prGetFacturaRecibidaSteuben");
            this.Database.AddInParameter(cmd, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(cmd, "@Al", DbType.DateTime, criteria.Al);
            this.Database.AddInParameter(cmd, "@RfcReceptor", DbType.String, criteria.RfcReceptor);
            this.Database.AddInParameter(cmd, "@Tipo", DbType.Int32, (Int32)criteria.Tipo);
            this.Database.AddInParameter(cmd, "@TipoComprobante", DbType.Int32, criteria.TipoComprobante);

            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);

            while (dr.Read())
            {
                list.Add(this.LoadItem(dr));
            }

            return list.ToArray();
        }
    }
}

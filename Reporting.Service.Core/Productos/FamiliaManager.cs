using Reporting.Service.Core.Clientes;
using System;
using SAPbobsCOM;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Reporting.Service.Core.Productos
{
    public class FamiliaManager : DataRepository4
    {
        public bool CoreInsertFamily(string Nombre, int MayoreoDesde, int MayoreoDistribuidorDesde)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spInsertFamily");
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            this.Database.AddInParameter(cmd, "@MayoreoDesde", DbType.Int16, MayoreoDesde);
            this.Database.AddInParameter(cmd, "@MayoreoDistribuidorDesde", DbType.Int16, MayoreoDistribuidorDesde);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
        public List<Familia> CoreListFamilySearch(string Nombre)
        {
            List<Familia> Detalle = new List<Familia>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetListFamilySearch");
            this.Database.AddInParameter(cmd, "@Nombre", DbType.String, Nombre);
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Familia
                {
                    Sequence = DBNull.Value.Equals(dr["Sequence"]) ? 0 : (int)dr["Sequence"],
                    Nombre = (string)dr["Nombre"].ToString(),
                    MayoreoDesde = DBNull.Value.Equals(dr["MayoreoDesde"]) ? 0 : (int)dr["MayoreoDesde"],
                    MayoreoDistribuidorDesde = DBNull.Value.Equals(dr["MayoreoDistribuidorDesde"]) ? 0 : (int)dr["MayoreoDistribuidorDesde"]
                });
            }
            return Detalle;
        }

        public List<Familia> CoreGetAllFamily()
        {
            List<Familia> Detalle = new List<Familia>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spGetAllFamilies");
            cmd.CommandTimeout = 600;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new Familia
                {
                    Sequence = DBNull.Value.Equals(dr["Sequence"]) ? 0 : (int)dr["Sequence"],
                    Codigo = DBNull.Value.Equals(dr["Codigo"]) ? 0 : (int)dr["Codigo"],
                    Nombre = (string)dr["Nombre"].ToString(),
                    MayoreoDesde = DBNull.Value.Equals(dr["MayoreoDesde"]) ? 0 : (int)dr["MayoreoDesde"],
                    MayoreoDistribuidorDesde = DBNull.Value.Equals(dr["MayoreoDistribuidorDesde"]) ? 0 : (int)dr["MayoreoDistribuidorDesde"]
                });
            }
            return Detalle;
        }
        public bool CoreUpdateFamilyInProduct(int Codigo, string Sku)
        {
            DbCommand cmd = this.Database.GetStoredProcCommand("spUpdateFamilyInProduct");
            this.Database.AddInParameter(cmd, "@Codigo", DbType.String, Codigo);
            this.Database.AddInParameter(cmd, "@Sku", DbType.String, Sku);
            IDataReader dr = this.Database.ExecuteReader(cmd);
            if (dr.RecordsAffected > 0)
                return true;
            else
                return false;
        }
    }
}
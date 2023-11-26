using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Reportes.Mayoreo
{
    public class MayoreoManager : Catalog<Mayoreo, int, MayoreoCriteria>
    {
        public MayoreoManager()
            : base()
        {

        }

        public MayoreoManager(string Database)
            : base(Database)
        {

        }

        public MayoreoManager(Database Database)
            : base(Database)
        {

        }
        protected override string FindPagedItemsProcedure => "prFindReporteMayoreo";

        protected override Mayoreo LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(Mayoreo item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(Mayoreo item)
        {
            throw new NotImplementedException();
        }

        public List<Mayoreo> GetReporteMayoreoXMes(MayoreoCriteria criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, criteria.Al);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Familia = (string)dr["Familia"];
                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                item.Mes = (int)dr["Mes"];
                // 2018

                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Familia = (string)dr["Familia"];
                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    item1.Mes = (int)dr["Mes"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Familia = (string)dr["Familia"];
                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    item2.Mes = (int)dr["Mes"];
                    lista.Add(item2);
                }
            }

            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        public List<Meses> GetMeses(MayoreoCriteria criteria)
        {
            List<Meses> lista = new List<Meses>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoMeses");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, criteria.Al);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Meses item = new Meses();
                item.Identifier = (int)dr["Mes"];
                item.MesName = (string)dr["MesFormat"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();
            return lista;
        }

        public List<Mayoreo> GetReporteMayoreoXMesItem(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoXMesItem");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.ItemCode = (string)dr["ItemCode"];
                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.ItemCode = (string)dr["ItemCode"];
                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.ItemCode = (string)dr["ItemCode"];
                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }

            command.Dispose();
            dr.Close();
            dr.Dispose();
            return lista;
        }

        public List<string> GetReporteFamiliaCabecera(MayoreoCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoFamiliaCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                lista.Add((string)dr["ItmsGrpNam"]);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        // TRAER TODOS LOS SKUS PARA ARMAR EL REPORTE DE ACUERDO AL FILTRO
        public List<string> GetReporteSKUSCabecera(MayoreoCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoSKUSCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                lista.Add((string)dr["ItemCode"]);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        // CABECERA CATEGORIAS
        public List<Categoria> GetReporteCategoriasCacbecera(MayoreoCriteria Criteria)
        {
            List<Categoria> lista = new List<Categoria>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetMayoreoCategoriaCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Categoria item = new Categoria();
                item.Nombre = (string)dr["Categoria"];
                item.Tipo = (int)dr["Tipo"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Mayoreo> GetReporteMayoreoXMesItemCategoria(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoXMesItemCategoria");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Categoria = (string)dr["Categoria"];
                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Categoria = (string)dr["Categoria"];
                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Categoria = (string)dr["Categoria"];
                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        public List<Cliente> GetReporteMayoreoClienteCabecera(MayoreoCriteria Criteria)
        {
            List<Cliente> lista = new List<Cliente>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetMayoreoClienteCabecera");
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Clasificado", DbType.String, Criteria.Clasificado);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Cliente item = new Cliente();
                item.Identifier = (string)dr["CardCode"];
                item.Nombre = (string)dr["Cliente"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<string> GetReporteMayoreoClienteCabeceraSKU(MayoreoCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetMayoreoClienteSKUCabecera]");
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Clasificado", DbType.String, Criteria.Clasificado);
            this.Database.AddInParameter(command, "@CardCode", DbType.String, Criteria.Cliente);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                lista.Add((string)dr["ItemCode"]);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Mayoreo> GetReporteMayoreoXMesItemCategoriaCliente(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteMayoreoXMesItemCategoriaCliente]");
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Clasificado", DbType.String, Criteria.Clasificado);

            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.CardCode = (string)dr["CardCode"];
                item.Cliente = (string)dr["Cliente"];
                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.CardCode = (string)dr["CardCode"];
                    item1.Cliente = (string)dr["Cliente"];
                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.CardCode = (string)dr["CardCode"];
                    item2.Cliente = (string)dr["Cliente"];
                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Mayoreo> GetReporteMayoreoXMesItemCategoriaClienteSKU(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteMayoreoXMesItemCategoriaClienteSKU]");
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Clasificado", DbType.String, Criteria.Clasificado);
            this.Database.AddInParameter(command, "@CardCode", DbType.String, Criteria.Cliente);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.ItemCode = (string)dr["ItemCode"];
                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.ItemCode = (string)dr["ItemCode"]; ;
                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.ItemCode = (string)dr["ItemCode"]; ;
                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        // EJECUTIVO

        public List<Mayoreo> GetReporteMayoreoEjecutivoXMes(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEjecutivoXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Mes = (int)dr["Mes"];
                item.EstatusEjecutivo = (string)dr["EstatusEjecutivo"];
                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Mes = (int)dr["Mes"];
                    item1.EstatusEjecutivo = (string)dr["EstatusEjecutivo"];
                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Mes = (int)dr["Mes"];
                    item2.EstatusEjecutivo = (string)dr["EstatusEjecutivo"];
                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();


            return lista;
        }

        public List<Ejecutivo> GetReporteMayoreoEjecutivoCabecera(MayoreoCriteria Criteria)
        {
            List<Ejecutivo> lista = new List<Ejecutivo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEjecutivoCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Ejecutivo item = new Ejecutivo();
                item.Identifier = (int)dr["SlpCode"];
                item.Nombre = (string)dr["SlpName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        public List<Mayoreo> GetReporteMayoreoEjecutivoXMesItem(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEjecutivoXMesItem");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Mes = (int)dr["Mes"];

                item.Ejecutivo = new Ejecutivo();
                item.Ejecutivo.Identifier = (int)dr["SlpCode"];
                item.Ejecutivo.Nombre = (string)dr["Ejecutivo"];

                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Mes = (int)dr["Mes"];

                    item1.Ejecutivo = new Ejecutivo();
                    item1.Ejecutivo.Identifier = (int)dr["SlpCode"];
                    item1.Ejecutivo.Nombre = (string)dr["Ejecutivo"];

                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Mes = (int)dr["Mes"];

                    item2.Ejecutivo = new Ejecutivo();
                    item2.Ejecutivo.Identifier = (int)dr["SlpCode"];
                    item2.Ejecutivo.Nombre = (string)dr["Ejecutivo"];

                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();


            return lista;
        }


        public List<Cliente> GetReporteMayoreoEjecutivoClienteCabecera(MayoreoCriteria Criteria)
        {
            List<Cliente> lista = new List<Cliente>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEjecutivoCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@IdEjecutivo", DbType.Int32, Criteria.IdEjecutivo);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Cliente item = new Cliente();
                item.Identifier = (string)dr["CardCode"];
                item.Nombre = (string)dr["CardName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        public List<Mayoreo> GetReporteMayoreoEjecutivoXMesItemCliente(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEjecutivoXMesItem");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@IdEjecutivo", DbType.Int32, Criteria.IdEjecutivo);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Mes = (int)dr["Mes"];

                item.CardCode = (string)dr["CardCode"];
                item.Cliente = (string)dr["CardName"];

                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Mes = (int)dr["Mes"];

                    item1.CardCode = (string)dr["CardCode"];
                    item1.Cliente = (string)dr["CardName"];

                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Mes = (int)dr["Mes"];

                    item2.CardCode = (string)dr["CardCode"];
                    item2.Cliente = (string)dr["CardName"];

                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();


            return lista;
        }


        //TIPO EJECUTIVO - EJECUTIVO - CLIENTE - SKU 

        public List<string> GetReporteMayoreoEjecutivoClienteSKUCabecera(MayoreoCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEjecutivoCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@IdEjecutivo", DbType.Int32, Criteria.IdEjecutivo);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                lista.Add((string)dr["ItemCode"]);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        public List<Mayoreo> GetReporteMayoreoEjecutivoXMesItemClienteSKU(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEjecutivoXMesItem");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@IdEjecutivo", DbType.Int32, Criteria.IdEjecutivo);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Mes = (int)dr["Mes"];

                item.ItemCode = (string)dr["ItemCode"];

                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Mes = (int)dr["Mes"];

                    item1.ItemCode = (string)dr["ItemCode"];

                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Mes = (int)dr["Mes"];

                    item2.ItemCode = (string)dr["ItemCode"];

                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();


            return lista;
        }


        // TIPO EJECUTIVO - CLIENTE - PIEZAS

        #region Mayoreo TipoEjecutico - Cliente - Piezas

        public List<Cliente> GetReporteMayoreoClientePzsCabecera(MayoreoCriteria Criteria)
        {
            List<Cliente> lista = new List<Cliente>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoClientePzsCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Cliente item = new Cliente();
                item.Identifier = (string)dr["CardCode"];
                item.Nombre = (string)dr["CardName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        public List<Mayoreo> GetReporteMayoreoClientePzsXMesItem(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoClientePzsXMesItem");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Mes = (int)dr["Mes"];

                item.CardCode = (string)dr["CardCode"];
                item.Cliente = (string)dr["CardName"];


                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Mes = (int)dr["Mes"];

                    item1.CardCode = (string)dr["CardCode"];
                    item1.Cliente = (string)dr["CardName"];

                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Mes = (int)dr["Mes"];

                    item2.CardCode = (string)dr["CardCode"];
                    item2.Cliente = (string)dr["CardName"];

                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        // lista de vendedores
        public List<Ejecutivo> GetReporteMayoreoClientePzsCabeceraCliente(MayoreoCriteria Criteria)
        {
            List<Ejecutivo> lista = new List<Ejecutivo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoClientePzsCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Ejecutivo item = new Ejecutivo();
                item.Identifier = (int)dr["SlpCode"];
                item.Nombre = (string)dr["SlpName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Mayoreo> GetReporteMayoreoClientePzsXMesItemCliente(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoClientePzsXMesItem");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Mes = (int)dr["Mes"];

                item.Ejecutivo = new Ejecutivo();
                item.Ejecutivo.Identifier = (int)dr["SlpCode"];
                item.Ejecutivo.Nombre = (string)dr["SlpName"];

                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Mes = (int)dr["Mes"];

                    item1.Ejecutivo = new Ejecutivo();
                    item1.Ejecutivo.Identifier = (int)dr["SlpCode"];
                    item1.Ejecutivo.Nombre = (string)dr["SlpName"];

                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Mes = (int)dr["Mes"];

                    item2.Ejecutivo = new Ejecutivo();
                    item2.Ejecutivo.Identifier = (int)dr["SlpCode"];
                    item2.Ejecutivo.Nombre = (string)dr["SlpName"];

                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        // lista de vendedores sku

        public List<string> GetReporteMayoreoClientePzsCabeceraClienteSKU(MayoreoCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoClientePzsCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@IdEjecutivo", DbType.Int32, Criteria.IdEjecutivo);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                lista.Add((string)dr["ItemCode"]);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Mayoreo> GetReporteMayoreoClientePzsXMesItemClienteSKU(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoClientePzsXMesItem");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@TipoEjecutivo", DbType.String, Criteria.TipoEjecutivo);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@IdEjecutivo", DbType.Int32, Criteria.IdEjecutivo);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Tipo = (int)dr["YEAR"];
                item.Mes = (int)dr["Mes"];

                item.ItemCode = (string)dr["ItemCode"];

                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Tipo = (int)dr["YEAR"];
                    item1.Mes = (int)dr["Mes"];

                    item1.ItemCode = (string)dr["ItemCode"];

                    item1.Cantidad = (Decimal)dr["Cantidad"];
                    item1.Total = (decimal)dr["Total"];
                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Tipo = (int)dr["YEAR"];
                    item2.Mes = (int)dr["Mes"];

                    item2.ItemCode = (string)dr["ItemCode"];

                    item2.Cantidad = (Decimal)dr["Cantidad"];
                    item2.Total = (decimal)dr["Total"];
                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        #endregion


        #region MayoreoPedidos

        // GetReporteMayoreoPedidoVendedorCabecera
        // GetReporteMayoreoPedidoXMes

        // vendedor
        public List<Pedidos> GetReporteMayoreoPedidoXMes(MayoreoCriteria Criteria)
        {
            List<Pedidos> lista = new List<Pedidos>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoPedidoXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Pedidos item = new Pedidos();
                item.Mes = (int)dr["Mes"];
                item.YEAR = (int)dr["YEAR"];
                item.NPedidos = (int)dr["NPedidos"];
                item.PromedoPedidos = (decimal)dr["PromedioPedidos"];
                if (dr["SlpCode"] != DBNull.Value && dr["SlpName"] != DBNull.Value)
                {
                    item.Ejecutivo = new Ejecutivo();
                    item.Ejecutivo.Identifier = (int)dr["SlpCode"];
                    item.Ejecutivo.Nombre = (string)dr["SlpName"];
                }

                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Pedidos item1 = new Pedidos();
                    item1.Mes = (int)dr["Mes"];
                    item1.YEAR = (int)dr["YEAR"];
                    item1.NPedidos = (int)dr["NPedidos"];
                    item1.PromedoPedidos = (decimal)dr["PromedioPedidos"];

                    if (dr["SlpCode"] != DBNull.Value && dr["SlpName"] != DBNull.Value)
                    {
                        item1.Ejecutivo = new Ejecutivo();
                        item1.Ejecutivo.Identifier = (int)dr["SlpCode"];
                        item1.Ejecutivo.Nombre = (string)dr["SlpName"];
                    }

                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Pedidos item2 = new Pedidos();
                    item2.Mes = (int)dr["Mes"];
                    item2.YEAR = (int)dr["YEAR"];
                    item2.NPedidos = (int)dr["NPedidos"];
                    item2.PromedoPedidos = (decimal)dr["PromedioPedidos"];

                    if (dr["SlpCode"] != DBNull.Value && dr["SlpName"] != DBNull.Value)
                    {
                        item2.Ejecutivo = new Ejecutivo();
                        item2.Ejecutivo.Identifier = (int)dr["SlpCode"];
                        item2.Ejecutivo.Nombre = (string)dr["SlpName"];
                    }

                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();


            return lista;
        }

        public List<Ejecutivo> GetReporteMayoreoPedidoVendedorCabecera(MayoreoCriteria Criteria)
        {
            List<Ejecutivo> lista = new List<Ejecutivo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoPedidoCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Ejecutivo item = new Ejecutivo();
                item.Identifier = (int)dr["SlpCode"];
                item.Nombre = (string)dr["SlpName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        // clientes 
        public List<Pedidos> GetReporteMayoreoPedidoXMesItem(MayoreoCriteria Criteria)
        {
            List<Pedidos> lista = new List<Pedidos>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoPedidoXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@IdEjecutivo", DbType.String, Criteria.IdEjecutivo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Pedidos item = new Pedidos();
                item.Mes = (int)dr["Mes"];
                item.YEAR = (int)dr["YEAR"];
                item.NPedidos = (int)dr["NPedidos"];
                item.PromedoPedidos = (decimal)dr["PromedioPedidos"];

                if (dr["CardCode"] != DBNull.Value && dr["CardName"] != DBNull.Value)
                {
                    item.Cliente = new Cliente();
                    item.Cliente.Identifier = (string)dr["CardCode"];
                    item.Cliente.Nombre = (string)dr["CardName"];
                }

                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Pedidos item1 = new Pedidos();
                    item1.Mes = (int)dr["Mes"];
                    item1.YEAR = (int)dr["YEAR"];
                    item1.NPedidos = (int)dr["NPedidos"];
                    item1.PromedoPedidos = (decimal)dr["PromedioPedidos"];

                    if (dr["CardCode"] != DBNull.Value && dr["CardName"] != DBNull.Value)
                    {
                        item1.Cliente = new Cliente();
                        item1.Cliente.Identifier = (string)dr["CardCode"];
                        item1.Cliente.Nombre = (string)dr["CardName"];
                    }

                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Pedidos item2 = new Pedidos();
                    item2.Mes = (int)dr["Mes"];
                    item2.YEAR = (int)dr["YEAR"];
                    item2.NPedidos = (int)dr["NPedidos"];
                    item2.PromedoPedidos = (decimal)dr["PromedioPedidos"];

                    if (dr["CardCode"] != DBNull.Value && dr["CardName"] != DBNull.Value)
                    {
                        item2.Cliente = new Cliente();
                        item2.Cliente.Identifier = (string)dr["CardCode"];
                        item2.Cliente.Nombre = (string)dr["CardName"];
                    }

                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();


            return lista;
        }

        public List<Cliente> GetReporteMayoreoPedidoVendedorCabeceraCliente(MayoreoCriteria Criteria)
        {
            List<Cliente> lista = new List<Cliente>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoPedidoCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@IdEjecutivo", DbType.String, Criteria.IdEjecutivo);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Cliente item = new Cliente();
                item.Identifier = (string)dr["CardCode"];
                item.Nombre = (string)dr["CardName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        #endregion


        #region EstadoCliente

        #region EstadoCabecera
        public List<Mayoreo> GetReporteMayoreoEdoClienteXMes(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEdoClienteXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Mes = (int)dr["Mes"];
                item.Tipo = (int)dr["YEAR"];

                item.StateS = (string)dr["StateS"];

                item.Total = (decimal)dr["Total"];

                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Mes = (int)dr["Mes"];
                    item1.Tipo = (int)dr["YEAR"];

                    item1.StateS = (string)dr["StateS"];

                    item1.Total = (decimal)dr["Total"];

                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Mes = (int)dr["Mes"];
                    item2.Tipo = (int)dr["YEAR"];

                    item2.StateS = (string)dr["StateS"];

                    item2.Total = (decimal)dr["Total"];

                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        public List<string> GetReporteMayoreoEdoCabecera(MayoreoCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteMayoreoEdoClienteCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);

            while (dr.Read())
            {
                lista.Add((string)dr["StateS"]);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }
        #endregion


        #region Estado-Cliente
        public List<Cliente> GetReporteMayoreoEdoClienteCabecera(MayoreoCriteria Criteria)
        {
            List<Cliente> lista = new List<Cliente>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteMayoreoEdoClienteCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@Estado", DbType.String, Criteria.StateS);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Cliente item = new Cliente();
                item.Identifier = (string)dr["CardCode"];
                item.Nombre = (string)dr["CardName"];

                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }
        
        public List<Mayoreo> GetReporteMayoreoEdoClienteXMesItem(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEdoClienteXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@Estado", DbType.String, Criteria.StateS);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Mes = (int)dr["Mes"];
                item.Tipo = (int)dr["YEAR"];

                item.CardCode = (string)dr["CardCode"];
                item.Cliente = (string)dr["CardName"];

                item.Total = (decimal)dr["Total"];

                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Mes = (int)dr["Mes"];
                    item1.Tipo = (int)dr["YEAR"];

                    item1.CardCode = (string)dr["CardCode"];
                    item1.Cliente = (string)dr["CardName"];

                    item1.Total = (decimal)dr["Total"];

                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Mes = (int)dr["Mes"];
                    item2.Tipo = (int)dr["YEAR"];

                    item2.CardCode = (string)dr["CardCode"];
                    item2.Cliente = (string)dr["CardName"];

                    item2.Total = (decimal)dr["Total"];

                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }
        #endregion

        
        #region Estado-Cliente-Vendedor

        // lista de vendedores 
        public List<Ejecutivo> GetReporteMayoreoEdoClienteVendedorCabecera(MayoreoCriteria Criteria)
        {
            List<Ejecutivo> lista = new List<Ejecutivo>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteMayoreoEdoClienteCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@Estado", DbType.String, Criteria.StateS);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Ejecutivo item = new Ejecutivo();
                item.Identifier = (int)dr["SlpCode"];
                item.Nombre = (string)dr["SlpName"];

                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        // estado - cliente - vendedor
        public List<Mayoreo> GetReporteMayoreoEdoClienteXMesItemCliente(MayoreoCriteria Criteria)
        {
            List<Mayoreo> lista = new List<Mayoreo>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteMayoreoEdoClienteXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@Estado", DbType.String, Criteria.StateS);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Mayoreo item = new Mayoreo();
                item.Mes = (int)dr["Mes"];
                item.Tipo = (int)dr["YEAR"];

                item.Ejecutivo = new Ejecutivo();
                item.Ejecutivo.Identifier = (int)dr["SlpCode"];
                item.Ejecutivo.Nombre = (string)dr["SlpName"];

                item.Total = (decimal)dr["Total"];

                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item1 = new Mayoreo();
                    item1.Mes = (int)dr["Mes"];
                    item1.Tipo = (int)dr["YEAR"];

                    item1.Ejecutivo = new Ejecutivo();
                    item1.Ejecutivo.Identifier = (int)dr["SlpCode"];
                    item1.Ejecutivo.Nombre = (string)dr["SlpName"];

                    item1.Total = (decimal)dr["Total"];

                    lista.Add(item1);
                }
            }
            // 2017
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Mayoreo item2 = new Mayoreo();
                    item2.Mes = (int)dr["Mes"];
                    item2.Tipo = (int)dr["YEAR"];

                    item2.Ejecutivo = new Ejecutivo();
                    item2.Ejecutivo.Identifier = (int)dr["SlpCode"];
                    item2.Ejecutivo.Nombre = (string)dr["SlpName"];

                    item2.Total = (decimal)dr["Total"];

                    lista.Add(item2);
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }
        #endregion

        #endregion

    }
}

using Microsoft.Practices.EnterpriseLibrary.Data;
using Reporting.Service.Core.Reportes.Mayoreo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Reportes.Retail
{
    public class RetailManager : Catalog<Retail, int, RetailCriteria>
    {
        public RetailManager()
            : base()
        {

        }

        public RetailManager(string Database)
            : base(Database)
        {

        }

        public RetailManager(Database Database)
            : base(Database)
        {

        }


        protected override string FindPagedItemsProcedure => "rpt.prFindRetail";

        protected override Retail LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(Retail item)
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

        protected override DbCommand PrepareUpdateStatement(Retail item)
        {
            throw new NotImplementedException();
        }

        // lista de meses
        public List<Meses> GetMeses(RetailCriteria criteria)
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

        #region Familia- SKU
        // lista de familias cabecera
        public List<string> GetReporteRetailFamiliaCabecera(RetailCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteRetailFamiliaCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
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

        // lista detalle de familias x mes
        public List<Retail> GetReporteRetailXMes(RetailCriteria criteria)
        {
            List<Retail> lista = new List<Retail>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteRetailXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, criteria.Al);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, criteria.Tipo);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Retail item = new Retail();
                item.YEAR = (int)dr["YEAR"];
                item.Familia = (string)dr["Familia"];
                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                item.Mes = (int)dr["Mes"];

                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Retail item1 = new Retail();
                    item1.YEAR = (int)dr["YEAR"];
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
                    Retail item2 = new Retail();
                    item2.YEAR = (int)dr["YEAR"];
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

        // cabecera de familia sku
        public List<string> GetReporteRetailFamiliaSKUSCabecera(RetailCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteRetailFamiliaCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
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

        // lista detalle de familia sku item
        public List<Retail> GetReporteRetailXMesItem(RetailCriteria Criteria)
        {
            List<Retail> lista = new List<Retail>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteRetailXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);// 2 sku
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Retail item = new Retail();
                item.YEAR = (int)dr["YEAR"];
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
                    Retail item1 = new Retail();
                    item1.YEAR = (int)dr["YEAR"];
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
                    Retail item2 = new Retail();
                    item2.YEAR = (int)dr["YEAR"];
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


        #region Familia Categoria Categoria1 Clasificacion Cliente SKU

        // cabecera de familia categoria
        public List<string> GetReporteRetailFamiliaCategoriasAllsCabecera(RetailCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteRetailFamiliaCategoriasCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Clasificado", DbType.String, Criteria.Clasificado);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);// 3 lista de categorias
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                switch (Criteria.Tipo)
                {
                    case RetailKind.Categoria:
                        if ((int)dr["Tipo"] == 1) // item categoria
                        {
                            lista.Add((string)dr["Categoria"]);
                        }
                        break;
                    case RetailKind.Categoria1:
                        if ((int)dr["Tipo"] == 2) // item categoria1
                        {
                            lista.Add((string)dr["Categoria"]);
                        }
                        break;
                    case RetailKind.Clasificacion:
                        if ((int)dr["Tipo"] == 3) // item clasificacion
                        {
                            lista.Add((string)dr["Categoria"]);
                        }
                        break;
                    case RetailKind.SKU:
                        if ((int)dr["Tipo"] == 2) // item clasificacion
                        {
                            lista.Add((string)dr["ItemCode"]);
                        }
                        break;
                    default:
                        break;
                }
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        // lista de clientes  
        public List<Reportes.Mayoreo.Cliente> GetReporteRetailFamiliaCategoriasClienteCabecera(RetailCriteria Criteria)
        {
            List< Reportes.Mayoreo.Cliente > lista = new List<Reportes.Mayoreo.Cliente>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteRetailFamiliaCategoriasCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Clasificado", DbType.String, Criteria.Clasificado);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);//6 lista de clientes
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Reportes.Mayoreo.Cliente item = new Reportes.Mayoreo.Cliente();
                item.Identifier = (string)dr["CardCode"];
                item.Nombre = (string)dr["CardName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }


        // detalle de familia categoria
        public List<Retail> GetReporteRetailXMesItemsAll(RetailCriteria Criteria)
        {
            List<Retail> lista = new List<Retail>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteRetailXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@Familia", DbType.String, Criteria.Familia);
            this.Database.AddInParameter(command, "@Categoria", DbType.String, Criteria.Categoria);
            this.Database.AddInParameter(command, "@Categoria1", DbType.String, Criteria.Categoria1);
            this.Database.AddInParameter(command, "@Clasificado", DbType.String, Criteria.Clasificado);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Retail item = new Retail();
                item.YEAR = (int)dr["YEAR"];

                // llenar el objecto de los canales de retail
                switch (Criteria.Tipo)
                {
                    case RetailKind.Categoria:
                        item.Categoria = (string)dr["Categoria"];
                        break;
                    case RetailKind.Categoria1:
                        item.Categoria1 = (string)dr["Categoria"];
                        break;
                    case RetailKind.Clasificacion:
                        item.Clasificado = (string)dr["Categoria"];
                        break;
                    case RetailKind.Cliente:
                        item.Cliente = new Reportes.Mayoreo.Cliente();
                        item.Cliente.Identifier = (string)dr["CardCode"];
                        item.Cliente.Nombre = (string)dr["CardName"];
                        break;
                    case RetailKind.SKU:
                        item.ItemCode = (string)dr["ItemCode"];
                        break;
                }

                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Retail item1 = new Retail();
                    item1.YEAR = (int)dr["YEAR"];

                    // llenar el objecto de los canales de retail
                    switch (Criteria.Tipo)
                    {
                        case RetailKind.Categoria:
                            item1.Categoria = (string)dr["Categoria"];
                            break;
                        case RetailKind.Categoria1:
                            item1.Categoria1 = (string)dr["Categoria"];
                            break;
                        case RetailKind.Clasificacion:
                            item1.Clasificado = (string)dr["Categoria"];
                            break;
                        case RetailKind.Cliente:
                            item1.Cliente = new Reportes.Mayoreo.Cliente();
                            item1.Cliente.Identifier = (string)dr["CardCode"];
                            item1.Cliente.Nombre = (string)dr["CardName"];
                            break;
                        case RetailKind.SKU:
                            item1.ItemCode = (string)dr["ItemCode"];
                            break;

                    }

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
                    Retail item2 = new Retail();
                    item2.YEAR = (int)dr["YEAR"];

                    // llenar el objecto de los canales de retail
                    switch (Criteria.Tipo)
                    {
                        case RetailKind.Categoria:
                            item2.Categoria = (string)dr["Categoria"];
                            break;
                        case RetailKind.Categoria1:
                            item2.Categoria1 = (string)dr["Categoria"];
                            break;
                        case RetailKind.Clasificacion:
                            item2.Clasificado = (string)dr["Categoria"];
                            break;
                        case RetailKind.Cliente:
                            item2.Cliente = new Reportes.Mayoreo.Cliente();
                            item2.Cliente.Identifier = (string)dr["CardCode"];
                            item2.Cliente.Nombre = (string)dr["CardName"];
                            break;
                        case RetailKind.SKU:
                            item2.ItemCode = (string)dr["ItemCode"];
                            break;

                    }

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


        #region Cliente - SKU - Pzs
        // cabecera de clientes
        // lista de clientes  
        public List<Reportes.Mayoreo.Cliente> GetReporteRetailClientePzsCabecera(RetailCriteria Criteria)
        {
            List<Reportes.Mayoreo.Cliente> lista = new List<Reportes.Mayoreo.Cliente>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteRetailClientePzsCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);//6 lista de clientes
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Reportes.Mayoreo.Cliente item = new Reportes.Mayoreo.Cliente();
                item.Identifier = (string)dr["CardCode"];
                item.Nombre = (string)dr["CardName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        // listado skus
        public List<string> GetReporteRetailClientePzsSKUCabecera(RetailCriteria Criteria)
        {
            List<string> lista = new List<string>();
            DbCommand command = this.Database.GetStoredProcCommand("[rpt].[prGetReporteRetailClientePzsCabecera]");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.Int32, Criteria.Mes);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);//6 lista de clientes
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

        //lista de items
        // detalle de familia categoria
        // tipo 
        // 100  listado de clientes
        // 200  listado de skus
        public List<Retail> GetReporteRetailClientePzsXMes(RetailCriteria Criteria)
        {
            List<Retail> lista = new List<Retail>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteRetailClientePzsXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);
            this.Database.AddInParameter(command, "@Mes", DbType.String, Criteria.Mes);
            this.Database.AddInParameter(command, "@Tipo", DbType.Int32, Criteria.Tipo);
            this.Database.AddInParameter(command, "@Cliente", DbType.String, Criteria.Cliente);
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Retail item = new Retail();
                item.YEAR = (int)dr["YEAR"];

                // llenar el objecto de los canales de retail
                switch (Criteria.Tipo)
                {
                    case RetailKind.ClientePzs:
                        item.Mes = (int)dr["Mes"];
                        item.Cliente = new Reportes.Mayoreo.Cliente();
                        item.Cliente.Identifier = (string)dr["CardCode"];
                        item.Cliente.Nombre = (string)dr["CardName"];
                        break;
                    case RetailKind.ClientePzsSKU:
                        item.ItemCode = (string)dr["ItemCode"];
                        break;
                }

                item.Cantidad = (Decimal)dr["Cantidad"];
                item.Total = (decimal)dr["Total"];
                // 2018
                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Retail item1 = new Retail();
                    item1.YEAR = (int)dr["YEAR"];

                    // llenar el objecto de los canales de retail
                    switch (Criteria.Tipo)
                    {
                        case RetailKind.ClientePzs:
                            item1.Mes = (int)dr["Mes"];
                            item1.Cliente = new Reportes.Mayoreo.Cliente();
                            item1.Cliente.Identifier = (string)dr["CardCode"];
                            item1.Cliente.Nombre = (string)dr["CardName"];
                            break;
                        case RetailKind.ClientePzsSKU:
                            item1.ItemCode = (string)dr["ItemCode"];
                            break;

                    }

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
                    Retail item2 = new Retail();
                    item2.YEAR = (int)dr["YEAR"];

                    // llenar el objecto de los canales de retail
                    switch (Criteria.Tipo)
                    {
                        case RetailKind.ClientePzs:
                            item2.Mes = (int)dr["Mes"];
                            item2.Cliente = new Reportes.Mayoreo.Cliente();
                            item2.Cliente.Identifier = (string)dr["CardCode"];
                            item2.Cliente.Nombre = (string)dr["CardName"];
                            break;
                        case RetailKind.SKU:
                            item2.ItemCode = (string)dr["ItemCode"];
                            break;

                    }

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

        #region 8020VTCL

        // cliente
        public List<Reportes.Mayoreo.Cliente> GetReporteRetail8020VTCLCabecera(RetailCriteria Criteria)
        {
            List<Reportes.Mayoreo.Cliente> lista = new List<Reportes.Mayoreo.Cliente>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteRetail8020VTCLCabecera");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, Criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, Criteria.Al);            
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Reportes.Mayoreo.Cliente item = new Reportes.Mayoreo.Cliente();
                item.Identifier = (string)dr["CardCode"];
                item.Nombre = (string)dr["CardName"];
                lista.Add(item);
            }
            command.Dispose();
            dr.Close();
            dr.Dispose();

            return lista;
        }

        public List<Retail> GetReporteRetail8020VTCLXMes(RetailCriteria criteria)
        {
            List<Retail> lista = new List<Retail>();
            DbCommand command = this.Database.GetStoredProcCommand("rpt.prGetReporteRetail8020VTCLXMes");
            this.Database.AddInParameter(command, "@Del", DbType.DateTime, criteria.Del);
            this.Database.AddInParameter(command, "@Al", DbType.DateTime, criteria.Al);            
            command.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                Retail item = new Retail();
                item.YEAR = (int)dr["YEAR"];

                item.Cliente = new Reportes.Mayoreo.Cliente();
                item.Cliente.Identifier = (string)dr["CardCode"];
                item.Cliente.Nombre = (string)dr["CardName"];

                item.Total = (decimal)dr["Total"];
                item.Mes = (int)dr["Mes"];

                lista.Add(item);
            }
            if (dr.NextResult())
            {
                while (dr.Read())
                {
                    Retail item1 = new Retail();
                    item1.YEAR = (int)dr["YEAR"];

                    item1.Cliente = new Reportes.Mayoreo.Cliente();
                    item1.Cliente.Identifier = (string)dr["CardCode"];
                    item1.Cliente.Nombre = (string)dr["CardName"];

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
                    Retail item2 = new Retail();
                    item2.YEAR = (int)dr["YEAR"];

                    item2.Cliente = new Reportes.Mayoreo.Cliente();
                    item2.Cliente.Identifier = (string)dr["CardCode"];
                    item2.Cliente.Nombre = (string)dr["CardName"];
                    
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

        #endregion

    }
}

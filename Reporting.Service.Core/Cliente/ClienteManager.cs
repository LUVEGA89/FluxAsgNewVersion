using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Cliente
{
    public class ClienteManager : Catalog<Cliente, int, ClienteFilter>
    {
        protected override string FindPagedItemsProcedure => "prFindCliente";

        protected override Cliente LoadItem(IDataReader dr)
        {
            Cliente item = new Cliente();
            item.Identifier = (int)dr["Sequence"];
            item.Rfc = (string)dr["Rfc"];
            item.Nombre = (string)dr["Nombre"];
            item.Descripcion = (string)dr["Descricpion"];
            item.Ref = (dr["Ref"] == DBNull.Value) ? "" : dr["Ref"].ToString();

            //Datos de dirección
            item.DireccionFacturacion = new DireccionFacturacion();
            item.DireccionFacturacion.Calle = (string)dr["Calle"];
            item.DireccionFacturacion.NumeroCalle = (string)dr["Numero"];
            item.DireccionFacturacion.Colonia = (string)dr["Colonia"];
            item.DireccionFacturacion.Cp = (string)dr["CodigoPostal"];
            item.DireccionFacturacion.CiudadMunicpio = (string)dr["CiudadMunicipio"];
            item.DireccionFacturacion.Estado = (string)dr["Estado"];
            item.DireccionFacturacion.IdPais = (string)dr["IdPais"];

            //Datos Bancarios
            item.DatosBancarios = new DatosBancarios();
            //item.DatosBancarios.IdBanco = (string)dr["IdBanco"];
            item.DatosBancarios.IdBanco = dr["IdBanco"].ToString();
            item.DatosBancarios.CuentaBanco = (string)dr["CuentaBanco"];
            item.DatosBancarios.TitularCuenta = (string)dr["TitularCuenta"];
            item.DatosBancarios.ClabeInterbancaria = (string)dr["ClabeInterbancaria"];
            item.DatosBancarios.DireccionCuentaBancaria = (string)dr["DireccionCuentaBancaria"];
            item.DatosBancarios.ContactPerson = (dr["ContactPerson"] == DBNull.Value) ? "" : dr["ContactPerson"].ToString();
            item.DatosBancarios.EmailFacturacion = (dr["EmailFacturacion"] == DBNull.Value) ? "" : dr["EmailFacturacion"].ToString();
            item.DatosBancarios.DireccionEnvio = (dr["DireccionEnvio"] == DBNull.Value) ? "" : dr["DireccionEnvio"].ToString();

            return item;

        }

        protected override DbCommand PrepareAddStatement(Cliente item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddCliente");
            this.Database.AddInParameter(command, "@Rfc", DbType.String, item.Rfc);
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre);
            this.Database.AddInParameter(command, "@Descricpion", DbType.String, item.Descripcion);
            this.Database.AddInParameter(command, "@Calle", DbType.String, item.DireccionFacturacion.Calle);
            this.Database.AddInParameter(command, "@Numero", DbType.String, item.DireccionFacturacion.NumeroCalle);
            this.Database.AddInParameter(command, "@CodigoPostal", DbType.String, item.DireccionFacturacion.Cp);
            this.Database.AddInParameter(command, "@Colonia", DbType.String, item.DireccionFacturacion.Colonia);
            this.Database.AddInParameter(command, "@CiudadMunicipio", DbType.String, item.DireccionFacturacion.CiudadMunicpio);
            this.Database.AddInParameter(command, "@Estado", DbType.String, item.DireccionFacturacion.Estado);
            this.Database.AddInParameter(command, "@IdPais", DbType.String, item.DireccionFacturacion.IdPais);
            this.Database.AddInParameter(command, "@IdBanco", DbType.String, item.DatosBancarios.IdBanco);
            this.Database.AddInParameter(command, "@TitularCuenta", DbType.String, item.DatosBancarios.TitularCuenta);
            this.Database.AddInParameter(command, "@CuentaBanco", DbType.String, item.DatosBancarios.CuentaBanco);
            this.Database.AddInParameter(command, "@ClabeInterbancaria", DbType.String, item.DatosBancarios.ClabeInterbancaria);
            this.Database.AddInParameter(command, "@DireccionCuentaBancaria", DbType.String, item.DatosBancarios.DireccionCuentaBancaria);
            this.Database.AddInParameter(command, "@RegistradoPor", DbType.String, "74c78946-2334-4d7c-9fd6-575669a60644");
            this.Database.AddInParameter(command, "@Ref", DbType.String, item.Ref);
            this.Database.AddInParameter(command, "@ContactPerson", DbType.String, item.DatosBancarios.ContactPerson);
            this.Database.AddInParameter(command, "@EmailFacturacion", DbType.String, item.DatosBancarios.EmailFacturacion);
            this.Database.AddInParameter(command, "@DireccionEnvio", DbType.String, item.DatosBancarios.DireccionEnvio);

            return command;
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            DbCommand dbCommand = this.Database.GetStoredProcCommand("prGetCliente");
            this.Database.AddInParameter(dbCommand, "@Id", DbType.String, id);
            return dbCommand;
        }

        protected override DbCommand PrepareUpdateStatement(Cliente item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prUpdateCliente");
            this.Database.AddInParameter(command, "@Id", DbType.Int32, item.Identifier);
            this.Database.AddInParameter(command, "@Rfc", DbType.String, item.Rfc);
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre);
            this.Database.AddInParameter(command, "@Descricpion", DbType.String, item.Descripcion);
            this.Database.AddInParameter(command, "@Calle", DbType.String, item.DireccionFacturacion.Calle);
            this.Database.AddInParameter(command, "@Numero", DbType.String, item.DireccionFacturacion.NumeroCalle);
            this.Database.AddInParameter(command, "@CodigoPostal", DbType.String, item.DireccionFacturacion.Cp);
            this.Database.AddInParameter(command, "@Colonia", DbType.String, item.DireccionFacturacion.Colonia);
            this.Database.AddInParameter(command, "@CiudadMunicipio", DbType.String, item.DireccionFacturacion.CiudadMunicpio);
            this.Database.AddInParameter(command, "@Estado", DbType.String, item.DireccionFacturacion.Estado);
            this.Database.AddInParameter(command, "@IdPais", DbType.String, item.DireccionFacturacion.IdPais);
            this.Database.AddInParameter(command, "@IdBanco", DbType.String, item.DatosBancarios.IdBanco);
            this.Database.AddInParameter(command, "@TitularCuenta", DbType.String, item.DatosBancarios.TitularCuenta);
            this.Database.AddInParameter(command, "@CuentaBanco", DbType.String, item.DatosBancarios.CuentaBanco);
            this.Database.AddInParameter(command, "@ClabeInterbancaria", DbType.String, item.DatosBancarios.ClabeInterbancaria);
            this.Database.AddInParameter(command, "@DireccionCuentaBancaria", DbType.String, item.DatosBancarios.DireccionCuentaBancaria);
            this.Database.AddInParameter(command, "@Ref", DbType.String, item.Ref);
            this.Database.AddInParameter(command, "@ContactPerson", DbType.String, item.DatosBancarios.ContactPerson);
            this.Database.AddInParameter(command, "@EmailFacturacion", DbType.String, item.DatosBancarios.EmailFacturacion);
            this.Database.AddInParameter(command, "@DireccionEnvio", DbType.String, item.DatosBancarios.DireccionEnvio);
            return command;
        }
    }
}
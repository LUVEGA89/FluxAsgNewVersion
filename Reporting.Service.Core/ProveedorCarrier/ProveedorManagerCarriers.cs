using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using WikiCore.Data;

namespace Resporting.Service.Core.ProveedorCarrier
{
    public class ProveedorManagerCarriers : Catalog<ProveedorCarriers, int, ProveedorCarriesFilter>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override ProveedorCarriers LoadItem(IDataReader dr)
        {
            ProveedorCarriers item = new ProveedorCarriers();
            item.Identifier = (int)dr["Sequence"];
            item.Nombre = (string)dr["Nombre"];
            item.NombreLargo = (string)dr["NombreLargo"];
            item.TipoAuto = (int)dr["TipoAuto"];
            item.Placas = (string)dr["Placas"];
            item.DescripcionVehiculo = (string)dr["DescripcionVehiculo"];
            item.Capacidad = (string)dr["Capacidad"];
            return item;
        }

        protected override DbCommand PrepareAddStatement(ProveedorCarriers item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddCarriers");
            this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre.ToUpper());
            this.Database.AddInParameter(command, "@NombreLargo", DbType.String, item.NombreLargo.ToUpper());
            this.Database.AddInParameter(command, "@TipoAuto", DbType.Int32, item.TipoAuto);
            this.Database.AddInParameter(command, "@Placas", DbType.String, item.Placas.ToUpper());
            this.Database.AddInParameter(command, "@DescripcionVehiculo", DbType.String, item.DescripcionVehiculo.ToUpper());
            this.Database.AddInParameter(command, "@Capacidad", DbType.String, item.Capacidad.ToUpper());
            this.Database.AddOutParameter(command, "@IdCarrier", DbType.Int32, 4);
            return command;
        }

        protected override void BeforeAddExecuted(DataContext<ProveedorCarriers, int, ProveedorCarriesFilter> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        // metodos para seguimiento a cliente
        protected override void CommandAddComplete(DataContext<ProveedorCarriers, int, ProveedorCarriesFilter> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@IdCarrier"].Value;
            context.Command.Parameters.Clear();
            base.CommandAddComplete(context);
            context.Transaction.Commit();
        }

        protected override DbCommand PrepareDeleteStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(int id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(ProveedorCarriers item)
        {
            throw new NotImplementedException();
        }


        public bool AddProvedorCarrier(int idProvedor, int idCarrier)
        {
            bool bandera = false;
            DbCommand command = this.Database.GetStoredProcCommand("prAddProvidersCarriers");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, idProvedor);
            this.Database.AddInParameter(command, "@IdCarriers", DbType.Int32, idCarrier);
            int resultado = this.Database.ExecuteNonQuery(command);
            if (resultado > 0)
            {
                bandera = true;
            }
            return bandera;
        }

        public List<ProveedorCarriers> getCarriers(int idProveedor)
        {
            List<ProveedorCarriers> datos = new List<ProveedorCarriers>();
            DbCommand command = this.Database.GetStoredProcCommand("prGetCarrier");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, idProveedor);
            IDataReader dr = this.Database.ExecuteReader(command);
            while (dr.Read())
            {
                datos.Add(this.LoadItem(dr));
            }
            return datos;
        }

    }
}
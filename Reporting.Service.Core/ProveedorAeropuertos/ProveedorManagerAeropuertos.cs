using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using WikiCore.Data;

namespace Resporting.Service.Core.ProveedorAeropuertos
{
    public class ProveedorManagerAeropuertos : Catalog<ProveedorAeropuertos, int, ProveedorAeropuertosFilter>
    {
        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override ProveedorAeropuertos LoadItem(IDataReader dr)
        {
            ProveedorAeropuertos item = new ProveedorAeropuertos();            
            return item;
        }

        protected override DbCommand PrepareAddStatement(ProveedorAeropuertos item)
        {
            DbCommand command = this.Database.GetStoredProcCommand("prAddAeropuertos");
            /*this.Database.AddInParameter(command, "@Nombre", DbType.String, item.Nombre.ToUpper());
            this.Database.AddInParameter(command, "@Siglas", DbType.String, item.Nombre.ToUpper());
            this.Database.AddInParameter(command, "@PaisId", DbType.String, item.Nombre.ToUpper());
            this.Database.AddOutParameter(command,"@IdAeropuerto", DbType.Int32, 4);
            */return command;
        }

        protected override void BeforeAddExecuted(DataContext<ProveedorAeropuertos, int, ProveedorAeropuertosFilter> context)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            context.Transaction = connection.BeginTransaction();
        }

        // metodos para seguimiento a cliente
        protected override void CommandAddComplete(DataContext<ProveedorAeropuertos, int, ProveedorAeropuertosFilter> context)
        {
            context.Item.Identifier = (int)context.Command.Parameters["@IdAeropuerto"].Value;
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

        protected override DbCommand PrepareUpdateStatement(ProveedorAeropuertos item)
        {
            throw new NotImplementedException();
        }


        public bool AddProvedorAeropuertos(int idProvedor, int idAeropuerto)
        {
            bool bandera = false;
            DbCommand command = this.Database.GetStoredProcCommand("prAddProvidersAeropuertos");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, idProvedor);
            this.Database.AddInParameter(command, "@IdCarriers", DbType.Int32, idAeropuerto);
            int resultado = this.Database.ExecuteNonQuery(command);
            if (resultado > 0)
            {
                bandera = true;
            }
            return bandera;
        }

        public bool AddProvedorAirports2(int idProvedor, string clabeAeropuerto)
        {
            bool bandera = false;
            DbCommand command = this.Database.GetStoredProcCommand("prAddProvidersAeropuertos");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, idProvedor);
            this.Database.AddInParameter(command, "@CodigoAeropuerto", DbType.String, clabeAeropuerto);
            this.Database.AddInParameter(command, "@ActualizadoPor", DbType.Guid, Guid.Parse("74c78946-2334-4d7c-9fd6-575669a60644"));
            int resultado = this.Database.ExecuteNonQuery(command);
            if (resultado > 0)
            {
                bandera = true;
            }
            return bandera;
        }

        public bool AddProvedorAirlines(int idProvedor, string aerolinea,string numeroViajero, string estatusAeropuerto)
        {
            bool bandera = false;
            DbCommand command = this.Database.GetStoredProcCommand("prAddProvidersAirlines");
            this.Database.AddInParameter(command, "@IdProveedor", DbType.Int32, idProvedor);
            this.Database.AddInParameter(command, "@IdAerolinea", DbType.String, aerolinea);
            this.Database.AddInParameter(command, "@NumViajeroFrecuente", DbType.String, numeroViajero);
            this.Database.AddInParameter(command, "@EstatusAerolinea", DbType.String, estatusAeropuerto);
            this.Database.AddInParameter(command, "@ActualizadoPor", DbType.Guid, Guid.Parse("74c78946-2334-4d7c-9fd6-575669a60644"));
            int resultado = this.Database.ExecuteNonQuery(command);
            if (resultado > 0)
            {
                bandera = true;
            }
            return bandera;
        }

        public List<ProveedorAeropuertos> getCarriers(int idProveedor)
        {
            List<ProveedorAeropuertos> datos = new List<ProveedorAeropuertos>();
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
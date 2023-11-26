using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.ListasPrecios
{
    public class ListaPrecioManager : Catalog<ListaPrecio, int, ListaPrecioCriteria>
    {
        public ListaPrecioManager()
            : base()
        {

        }

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override ListaPrecio LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(ListaPrecio item)
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

        protected override DbCommand PrepareUpdateStatement(ListaPrecio item)
        {
            throw new NotImplementedException();
        }


        public bool InsertListPriceTransaction(string ItemCode, List<ListaPrecio> ListasPrecios, string Currency, string Ovrwritten, int Estatus, string UsuarioModificador,string Rol)
        {
            DbConnection connection = this.Database.CreateConnection();
            connection.Open();
            DbTransaction dbTransaction = connection.BeginTransaction();

            try
            {
                DbCommand command = this.Database.GetStoredProcCommand("prAddModificiacionPrecio");
                this.Database.AddInParameter(command, "@ItemCode", DbType.String, ItemCode);
                this.Database.AddInParameter(command, "@UsuarioRegistrador", DbType.String, UsuarioModificador);
                this.Database.AddOutParameter(command, "@IdModificacion", DbType.Int32, 4);                
                this.Database.ExecuteNonQuery(command, dbTransaction);

                var IdModificaicon = Convert.ToInt32(this.Database.GetParameterValue(command, "@IdModificacion"));
                if (IdModificaicon == 0)
                {
                    throw new Exception("Out Parametrer value null in store procedure prAddModificiacionPrecio");
                }

                for (int i = 0; i < ListasPrecios.Count; i++)
                {
                    DbCommand cmd = this.Database.GetStoredProcCommand("spInsertListPrice");
                    this.Database.AddInParameter(cmd, "@ItemCode", DbType.String, ItemCode);
                    this.Database.AddInParameter(cmd, "@PriceList", DbType.Int16, ListasPrecios[i].Nombre);
                    this.Database.AddInParameter(cmd, "@ListaModificada", DbType.Int16, ListasPrecios[i].ListaModificada);
                    if (ListasPrecios[i].Precio != 0)
                    {
                        this.Database.AddInParameter(cmd, "@Price", DbType.Decimal, ListasPrecios[i].Precio);
                    }
                    else
                    {
                        throw new Exception();
                    }

                    this.Database.AddInParameter(cmd, "@Currency", DbType.String, Currency);
                    this.Database.AddInParameter(cmd, "@Ovrwritten", DbType.String, Ovrwritten);
                    this.Database.AddInParameter(cmd, "@Estatus", DbType.Int16, Estatus);
                    this.Database.AddInParameter(cmd, "@UsuarioModificador", DbType.String, Rol);
                    this.Database.AddInParameter(cmd, "@IdModificacion", DbType.String, IdModificaicon);
                    this.Database.ExecuteNonQuery(cmd, dbTransaction);

                }

                dbTransaction.Commit();
                return true;

            }
            catch (Exception ex)
            {
                dbTransaction.Rollback();
                if (dbTransaction.Connection.State == ConnectionState.Open)
                {
                    dbTransaction.Connection.Close();
                }
                return false;
            }
        }
    }
}

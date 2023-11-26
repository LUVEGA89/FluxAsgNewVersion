using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikiCore.Data;

namespace Reporting.Service.Core.Almacen.AlmacenPartida
{
    public class AlmacenPartidaManager : Catalog<AlmacenPartida, string, AlmacenPartidaCriteria>
    {
        public AlmacenPartidaManager()
            : base()
        {

        }        

        protected override string FindPagedItemsProcedure => throw new NotImplementedException();

        protected override AlmacenPartida LoadItem(IDataReader dr)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareAddStatement(AlmacenPartida item)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareDeleteStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareFindStatement(string id)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand PrepareUpdateStatement(AlmacenPartida item)
        {
            throw new NotImplementedException();
        }

        public List<AlmacenPartida> GetAlmacenPartidas()
        {
            try
            {
                List<AlmacenPartida> items = new List<AlmacenPartida>();
                DbCommand command = this.Database.GetStoredProcCommand("dbo.prGetAlmacenPartidas");                
                command.CommandTimeout = 0;
                IDataReader dr = this.Database.ExecuteReader(command);
                while (dr.Read())
                {
                    AlmacenPartida item = new AlmacenPartida();
                    item.Identifier = (string)dr["Identificador"];
                    item.Nombre = (string)dr["Nombre"];
                    items.Add(item);
                }

                return items;
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
    }
}

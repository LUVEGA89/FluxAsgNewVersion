using System;
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
namespace Reporting.Service.Core.AdminProyectos
{
    public class AdminProyectosManager : DataRepository
    {

        public List<AdminProyecto> CoreApGetProyecto()
        {
            List<AdminProyecto> Detalle = new List<AdminProyecto>();
            DbCommand cmd = this.Database.GetStoredProcCommand("spApGetLista");
            cmd.CommandTimeout = 0;
            IDataReader dr = this.Database.ExecuteReader(cmd);
            while (dr.Read())
            {
                Detalle.Add(new AdminProyecto
                {
                    IdLista = (int)dr["IdLista"],
                    Nombre = (string)dr["Nombre"]
                });
            }
            return Detalle;
        }



    }
}
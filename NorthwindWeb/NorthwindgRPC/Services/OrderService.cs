using Grpc.Core;
using Microsoft.Data.SqlClient;
using System.Data;

namespace NorthwindgRPC
{
    public class OrderService : Orders.OrdersBase
    {
        private readonly ILogger<OrderService> _logger;

        public OrderService(ILogger<OrderService> logger)
        {
            _logger = logger;
        }

        string cadena = "server=.;database=Northwind; trusted_Connection=true;" +
            "MultipleActiveResultSets=true; TrustServerCertificate=false; Encrypt=false";

        List<Orden> ListaPorCliente(string customerID)
        {
            List<Orden> lista = new List<Orden>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Pedidos_Por_Cliente", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CustomerID", customerID);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    lista.Add(new Orden()
                    {
                        OrderID = dr.GetInt32(0),
                        CustomerID = dr.GetString(1),
                        CompanyName = dr.IsDBNull(2) ? "" : dr.GetString(2),

                        OrderDate = dr.IsDBNull(3) ? "" : dr.GetDateTime(3).ToString("dd-MM-yyyyd"),
                        RequiredDate = dr.IsDBNull(4) ? "" : dr.GetDateTime(4).ToString("dd-MM-yyyy"),
                        ShippedDate = dr.IsDBNull(5) ? "" : dr.GetDateTime(5).ToString("dd-MM-yyyy"),

                        ShipName = dr.IsDBNull(6) ? "" : dr.GetString(6),
                        ShipCity = dr.IsDBNull(7) ? "" : dr.GetString(7),
                        ShipCountry = dr.IsDBNull(8) ? "" : dr.GetString(8)
                    });
                }
            }
            return lista;
        }

        List<Orden> Lista(DateTime inicio, DateTime fin)
        {
            List<Orden> temporal = new List<Orden>();

            using (SqlConnection con = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("USP_Orders_ListarEntreFechas", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FechaInicio", inicio);
                cmd.Parameters.AddWithValue("@FechaFin", fin);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    temporal.Add(new Orden()
                    {
                        OrderID = dr.GetInt32(0),
                        CustomerID = dr.GetString(1),
                        CompanyName = dr.IsDBNull(2) ? "" : dr.GetString(2),

                        OrderDate = dr.IsDBNull(3) ? "" : dr.GetDateTime(3).ToString("dd-MM-yyyy"),
                        RequiredDate = dr.IsDBNull(4) ? "" : dr.GetDateTime(4).ToString("dd-MM-yyyy"),
                        ShippedDate = dr.IsDBNull(5) ? "" : dr.GetDateTime(5).ToString("dd-MM-yyyy"),

                        ShipName = dr.IsDBNull(6) ? "" : dr.GetString(6),
                        ShipCity = dr.IsDBNull(7) ? "" : dr.GetString(7),
                        ShipCountry = dr.IsDBNull(8) ? "" : dr.GetString(8)
                    });
                }
            }
            return temporal;
        }

        public override Task<OrdenResponse> GetBetweenDates(DateFilter request, ServerCallContext context)
        {
            OrdenResponse response = new OrdenResponse();

            DateTime inicio = DateTime.Parse(request.FechaInicio);
            DateTime fin = DateTime.Parse(request.FechaFin);

            response.Items.AddRange(Lista(inicio, fin));
            return Task.FromResult(response);
        }



        public override Task<OrdenResponse> GetByCustomer(CustomerFilter request, ServerCallContext context)
        {
            OrdenResponse response = new OrdenResponse();
            response.Items.AddRange(ListaPorCliente(request.CustomerID));
            return Task.FromResult(response);
        }

    }
}

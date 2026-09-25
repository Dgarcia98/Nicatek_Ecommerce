using Domain.Entities;
using Domain.Interfaces;
using Infraestructure.Data;
using Microsoft.Data.SqlClient;

namespace Infraestructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DbConection _conexion;
        public NotificationRepository(DbConection conexion) => _conexion = conexion;

        private Notifications MapearNotification(SqlDataReader dr) => new Notifications
        {
            NotificationId           = Convert.ToInt32(dr["notificationId"]),
            NotificationUserId       = Convert.ToInt32(dr["notificationUserId"]),
            NotificationTitle        = dr["notificationTitle"] != DBNull.Value ? dr["notificationTitle"].ToString() : null,
            NotificationBody         = dr["notificationBody"] != DBNull.Value ? dr["notificationBody"].ToString() : null,
            NotificationType         = dr["notificationType"] != DBNull.Value ? dr["notificationType"].ToString() : null,
            NotificationReferenceId  = dr["notificationReferenceId"] != DBNull.Value ? Convert.ToInt32(dr["notificationReferenceId"]) : (int?)null,
            NotificationIsRead       = Convert.ToBoolean(dr["notificationIsRead"]),
            NotificationCreatorId    = Convert.ToInt32(dr["notificationCreatorId"]),
            NotificationCreationDate = dr["notificationCreationDate"] != DBNull.Value ? Convert.ToDateTime(dr["notificationCreationDate"]) : (DateTime?)null,
            NotificationStatusId     = Convert.ToBoolean(dr["notificationStatusId"]),
        };

        public async Task<IEnumerable<Notifications>> ListarNotificacionesAsync(int userId)
        {
            var lista = new List<Notifications>();
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_Notifications]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "LST");
            cmd.Parameters.AddWithValue("@notificationUserId", userId);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            using var dr = await cmd.ExecuteReaderAsync();
            while (await dr.ReadAsync()) lista.Add(MapearNotification(dr));
            return lista;
        }

        public async Task<int> ContarNoLeidasAsync(int userId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_Notifications]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "CNT");
            cmd.Parameters.AddWithValue("@notificationUserId", userId);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();
            return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 0;
        }

        public async Task NuevaNotificacionAsync(Notifications oNotification)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_Notifications]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "INS");
            cmd.Parameters.AddWithValue("@notificationUserId", oNotification.NotificationUserId);
            cmd.Parameters.AddWithValue("@notificationTitle", oNotification.NotificationTitle ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@notificationBody", oNotification.NotificationBody ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@notificationType", oNotification.NotificationType ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@notificationReferenceId", oNotification.NotificationReferenceId.HasValue ? (object)oNotification.NotificationReferenceId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@notificationCreatorId", oNotification.NotificationCreatorId);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task MarcarComoLeidaAsync(int id)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_Notifications]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "MRK");
            cmd.Parameters.AddWithValue("@notificationId", id);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task MarcarTodasComoLeidasAsync(int userId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_Notifications]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "MRA");
            cmd.Parameters.AddWithValue("@notificationUserId", userId);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarTodasAsync(int userId)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_Notifications]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "DAL");
            cmd.Parameters.AddWithValue("@notificationUserId", userId);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EliminarNotificacionAsync(int id)
        {
            using var conn = _conexion.CreateConection();
            using var cmd = new SqlCommand("[SQM_GENERAL].[sp_Tbl_Notifications]", conn) { CommandType = System.Data.CommandType.StoredProcedure };
            cmd.Parameters.AddWithValue("@Mode", "DEL");
            cmd.Parameters.AddWithValue("@notificationId", id);
            cmd.Parameters.Add("@O_Msg", System.Data.SqlDbType.VarChar, 255).Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add("@O_Num", System.Data.SqlDbType.Int).Direction = System.Data.ParameterDirection.Output;
            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}

using Microsoft.Data.SqlClient;
using ProductManagement_Api.Models;
using System.Data;

namespace ProductManagement_Api.Data
{
    public class ItemRepository
    {
        private readonly IConfiguration _config;
        private readonly string _connString;

        public ItemRepository(IConfiguration config)
        {
            _config = config;
            _connString = _config.GetConnectionString("DefaultConnection");
        }

        public async Task AddItemAsync(ItemModel model)
        {
            using SqlConnection con = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand("AddItem", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemCode", model.ItemCode);
            cmd.Parameters.AddWithValue("@Description", model.Description);
            cmd.Parameters.AddWithValue("@SellingPrice", model.SellingPrice);
            cmd.Parameters.AddWithValue("@CostPrice", model.CostPrice);
            if (!string.IsNullOrEmpty(model.PhotoBase64))
            {
                byte[] photoBytes = Convert.FromBase64String(model.PhotoBase64);
                var p = cmd.Parameters.Add("@Photo", SqlDbType.VarBinary, -1);
                p.Value = photoBytes;
            }
            else
            {
                var p = cmd.Parameters.Add("@Photo", SqlDbType.VarBinary, -1);
                p.Value = DBNull.Value;
            }
            cmd.Parameters.AddWithValue("@PunchDate", model.PunchDate == default ? DateTime.Now : model.PunchDate);
            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task EditItemAsync(ItemModel model)
        {
            try
            {
                using SqlConnection con = new SqlConnection(_connString);
                using SqlCommand cmd = new SqlCommand("EditItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemCode", model.ItemCode);
                cmd.Parameters.AddWithValue("@Description", model.Description);
                cmd.Parameters.AddWithValue("@SellingPrice", model.SellingPrice);
                cmd.Parameters.AddWithValue("@CostPrice", model.CostPrice);
                if (!string.IsNullOrWhiteSpace(model.PhotoBase64))
                {
                    try
                    {
                        byte[] photoBytes = Convert.FromBase64String(model.PhotoBase64);

                        var p = cmd.Parameters.Add("@Photo", SqlDbType.VarBinary, -1);
                        p.Value = photoBytes;
                    }
                    catch (FormatException ex)
                    {
                        throw new Exception("Invalid Base64 string for Photo. Please send a valid image Base64.", ex);
                    }
                }
                else
                {
                    var p = cmd.Parameters.Add("@Photo", SqlDbType.VarBinary, -1);
                    p.Value = DBNull.Value;
                }
                cmd.Parameters.AddWithValue("@PunchDate",
                    model.PunchDate == default ? DateTime.Now : model.PunchDate);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"EditItemAsync failed: {ex.Message}", ex);
            }
        }
        public async Task DeleteItemAsync(string itemCode)
        {
            using SqlConnection con = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand("DeleteItem", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ItemCode", itemCode);

            await con.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
        public async Task<List<ItemModel>> GetAllItemsAsync()
        {
            List<ItemModel> list = new List<ItemModel>();

            using SqlConnection con = new SqlConnection(_connString);
            using SqlCommand cmd = new SqlCommand("GetAllItems", con);
            cmd.CommandType = CommandType.StoredProcedure;

            await con.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var item = new ItemModel
                {
                    ItemCode = reader["ItemCode"]?.ToString() ?? string.Empty,
                    Description = reader["Description"]?.ToString() ?? string.Empty,
                    SellingPrice = reader["SellingPrice"] == DBNull.Value ? 0 : Convert.ToDouble(reader["SellingPrice"]),
                    CostPrice = reader["CostPrice"] == DBNull.Value ? 0 : Convert.ToDouble(reader["CostPrice"]),
                    PunchDate = reader["PunchDate"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["PunchDate"])
                };
                if (reader["Photo"] != DBNull.Value)
                {
                    byte[] photoBytes = (byte[])reader["Photo"];
                    item.PhotoBase64 = Convert.ToBase64String(photoBytes);
                }
                else
                {
                    item.PhotoBase64 = null;
                }
                list.Add(item);
            }
            return list;
        }
    }
}

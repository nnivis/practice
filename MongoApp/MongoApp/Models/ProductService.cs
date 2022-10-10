using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
 
namespace MongoApp.Models
{
    public class ProductService
    {
        IGridFSBucket gridFS;  
        IMongoCollection<Product> Products; 
        public ProductService()
        {
            
            string connectionString = "mongodb://localhost:27017/gameestore";
            var connection = new MongoUrlBuilder(connectionString);        
            MongoClient client = new MongoClient(connectionString);          
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);           
            gridFS = new GridFSBucket(database);         
            Products = database.GetCollection<Product>("Products");
        }       
        public async Task<IEnumerable<Product>> GetProducts(int? minPrice, int? maxPrice, string name)
        {
            var builder = new FilterDefinitionBuilder<Product>();
            var filter = builder.Empty; 

            if (!String.IsNullOrWhiteSpace(name))  //Сортировка по имени
            {
                filter = filter & builder.Regex("Name", new BsonRegularExpression(name));
            }
            if (minPrice.HasValue)  //Сортировка по минимальной цене
            {
                filter = filter & builder.Gte("Price", minPrice.Value);
            }
            if (maxPrice.HasValue)  // Сортировка по максимальной цене
            {
                filter = filter & builder.Lte("Price", maxPrice.Value);
            }
 
            return await Products.Find(filter).ToListAsync();
        }
 
        public async Task<Product> GetProduct(string id)
        {
            return await Products.Find(new BsonDocument("_id", new ObjectId(id))).FirstOrDefaultAsync();
        }
        public async Task Create(Product p)
        {
            await Products.InsertOneAsync(p);
        }
        public async Task Update(Product p)
        {
            await Products.ReplaceOneAsync(new BsonDocument("_id", new ObjectId(p.Id)), p);
        }
        public async Task Remove(string id)
        {
            await Products.DeleteOneAsync(new BsonDocument("_id", new ObjectId(id)));
        }
        public async Task<byte[]> GetImage(string id)
        {
            return await gridFS.DownloadAsBytesAsync(new ObjectId(id));
        }
        public async Task StoreImage(string id, Stream imageStream, string imageName)
        {
            Product p = await GetProduct(id);
            if (p.HasImage())
            {
                await gridFS.DeleteAsync(new ObjectId(p.ImageId));
            }
            ObjectId imageId = await gridFS.UploadFromStreamAsync(imageName, imageStream);
            p.ImageId = imageId.ToString();
            var filter = Builders<Product>.Filter.Eq("_id", new ObjectId(p.Id));
            var update = Builders<Product>.Update.Set("ImageId", p.ImageId);
            await Products.UpdateOneAsync(filter, update);
        }
    }
}
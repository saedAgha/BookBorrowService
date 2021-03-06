using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BooksBorrowService.IntegrationTests.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace AspNetCoreTests.IntegrationTests
{
    [Collection("Sequential")]
    public class BorrowBookRequestsControllerTests : TestBase
    {

        private readonly string _baseUrl = "/api/BorrowBookRequests";
        public BorrowBookRequestsControllerTests(TestApplicationFactory<FakeStartup> factory) : base(factory)
        {
        }

 
        [Fact]
        public async Task CreateBorrowBookRequest_Success()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;

            var data = new JObject
            {
                new JProperty("CustomerId", "1"),
                new JProperty("LibrarianId", "7")
                };

            // Act
            var response = await client.PostAsync($"{_baseUrl}/book/{bookId}/borrow", WrapWithByteContent(data));

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task CreateBorrowBookRequest_NotExistLibrarian()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;
            var data = new JObject
                {
                    new JProperty("CustomerId", "1"),
                    new JProperty("LibrarianId", "2")
                };

            // Act
            var response = await client.PostAsync($"{_baseUrl}/book/{bookId}/borrow", WrapWithByteContent(data));
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task CreateBorrowBookRequest_NotExistUser()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;


            var data = new JObject
            {
                new JProperty("CustomerId", "6"),
                new JProperty("LibrarianId", "7")
            };

            // Act
            var response = await client.PostAsync($"{_baseUrl}/book/{bookId}/borrow", WrapWithByteContent(data));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task CreateBorrowBookRequest_NotExistBook()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 6;
            var data = new JObject
            {
                new JProperty("CustomerId", "1"),
                new JProperty("LibrarianId", "7")
            };

            // Act
            var response = await client.PostAsync($"{_baseUrl}/book/{bookId}/borrow", WrapWithByteContent(data));
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task CreateBorrowBookRequest_NotAllowedUser()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;
            var data = new JObject
            {
                new JProperty("CustomerId", "1"),
                new JProperty("LibrarianId", "7")
            };

            // Act
            var response = await client.PostAsync($"{_baseUrl}/book/{bookId}/borrow", WrapWithByteContent(data));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            
            response = await client.PostAsync($"{_baseUrl}/book/{bookId}/borrow", WrapWithByteContent(data));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


        }

        [Fact]
        public async Task ReturnBorrowedBook_NotExistLibrarian()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;
            var data = new JObject
            {
                new JProperty("CustomerId", "1"),
                new JProperty("LibrarianId", "2")
            };

            // Act
            var response = await client.PatchAsync($"{_baseUrl}/book/{bookId}/return", WrapWithByteContent(data));
            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }

        [Fact]
        public async Task ReturnBorrowedBook_NotExistUser()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;

            var data = new JObject
            {
                new JProperty("CustomerId", "6"),
                new JProperty("LibrarianId", "7")
            };

            // Act
            var response = await client.PatchAsync($"{_baseUrl}/book/{bookId}/return", WrapWithByteContent(data));

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task ReturnBorrowedBook_NotExistBook()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 6;
            var data = new JObject
            {
                new JProperty("CustomerId", "1"),
                new JProperty("LibrarianId", "7")
            };

            // Act
            var response = await client.PatchAsync($"{_baseUrl}/book/{bookId}/return", WrapWithByteContent(data));
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);

        }

        [Fact]
        public async Task ReturnBorrowedBook_NotAllowedUser_NoRequestRegistered()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;
            var data = new JObject
            {
                new JProperty("CustomerId", "1"),
                new JProperty("LibrarianId", "7")
            };

            // Act
            var response = await client.PatchAsync($"{_baseUrl}/book/{bookId}/return", WrapWithByteContent(data));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

        }
        [Fact]
        public async Task ReturnBorrowedBook_NotAllowedUser_NoUnReturnedRequestRegistered()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;
            var data = new JObject
            {
                new JProperty("CustomerId", "1"),
                new JProperty("LibrarianId", "7")
            };

            // Act
            var response = await client.PostAsync($"{_baseUrl}/book/{bookId}/borrow", WrapWithByteContent(data));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            response = await client.PatchAsync($"{_baseUrl}/book/{bookId}/return", WrapWithByteContent(data));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            response = await client.PatchAsync($"{_baseUrl}/book/{bookId}/return", WrapWithByteContent(data));
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);


        }

        [Fact]
        public async Task ReturnBorrowedBook_Success()
        {
            // Arrange
            using var client = Factory.CreateClient();

            int bookId = 1;
            var data = new JObject
            {
                new JProperty("CustomerId", "1"),
                new JProperty("LibrarianId", "7")
            };

            // Act
            var response = await client.PostAsync($"{_baseUrl}/book/{bookId}/borrow", WrapWithByteContent(data));
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            response = await client.PatchAsync($"{_baseUrl}/book/{bookId}/return", WrapWithByteContent(data));
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        }

    

    
        private ByteArrayContent WrapWithByteContent(JObject jObject)
        {
            var myContent = JsonConvert.SerializeObject(jObject);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent;
        }
    }
}
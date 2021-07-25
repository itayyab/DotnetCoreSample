using DotnetCoreSample;
using DotnetCoreSample.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreSample.Controllers;

namespace UnitTests
{
    [TestCaseOrderer("UnitTests.PriorityOrderer", "UnitTests")]
    public class UsersControllerTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        IConfigurationRoot configuration;


        public UsersControllerTest()
         {
             configuration = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();

            WebHostBuilder webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureServices(s =>
                {
                    s.AddDbContext<DotnetCoreSampleContext>(options => options.UseSqlServer(configuration.GetConnectionString("DotnetCoreSampleContextTest")));
                });


            webHostBuilder.UseStartup<Startup>();

              _server = new TestServer(webHostBuilder);
             _client = _server.CreateClient();
         }

        [Fact, TestPriority(6)]
        public async Task GetAllUsers()
        {
            var maxresponse = await _client.GetAsync("/api/Users/-1");
            maxresponse.EnsureSuccessStatusCode();
            var maxresponseString = await maxresponse.Content.ReadAsStringAsync();
            User maxuser = JsonConvert.DeserializeObject<User>(maxresponseString);
            long userid = maxuser.id;
            Assert.NotEqual(0, userid);
            // Act
            var response = await _client.GetAsync("/api/Users");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal("[{\"id\":1,\"email\":\"Test@1\",\"name\":\"Tayyab\"}]", responseString);
        }
        [Fact, TestPriority(2)]
        public async Task AddUser()
        {
            var user = new User();
            user.email = "test";
            user.name = "test";
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Users", stringContent);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            User deserializeduser = JsonConvert.DeserializeObject<User>(responseString);
            long userid = deserializeduser.id;
            // Assert
            Assert.Equal("{\"id\":"+ userid + ",\"email\":\"test\",\"name\":\"test\"}", responseString);
        }
        [Fact, TestPriority(3)]
        public async Task UpdateUser()
        {

            var maxresponse = await _client.GetAsync("/api/Users/-1");
            maxresponse.EnsureSuccessStatusCode();
            var maxresponseString = await maxresponse.Content.ReadAsStringAsync();
            User maxuser = JsonConvert.DeserializeObject<User>(maxresponseString);
            long userid = maxuser.id;

            Assert.NotEqual(1, userid);
            var user = new User();
            user.id = userid;
            user.email = "test";
            user.name = "test";
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/Users/" + userid, stringContent);
            response.EnsureSuccessStatusCode();
            var responseStringx =  response.IsSuccessStatusCode;
            var responseString = await response.Content.ReadAsStringAsync();
            //User deserializeduser = JsonConvert.DeserializeObject<User>(responseString);
            //userid = deserializeduser.id;
            Assert.True(responseStringx);
            //Assert.Equal("{\"id\":" + userid + ",\"email\":\"test\",\"name\":\"test\"}", responseString);
        }
        [Fact, TestPriority(3)]
        public async Task UpdateUserBadRequest()
        {

            
            var user = new User();
            user.id = -1;
            user.email = "test";
            user.name = "test";
            var stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PutAsync("/api/Users/-2", stringContent);
            //response.EnsureSuccessStatusCode();
 
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact, TestPriority(4)]
        public async Task GetUser()
        {
            var maxresponse = await _client.GetAsync("/api/Users/-1");
            maxresponse.EnsureSuccessStatusCode();
            var maxresponseString = await maxresponse.Content.ReadAsStringAsync();
            User maxuser = JsonConvert.DeserializeObject<User>(maxresponseString);
            long userid = maxuser.id;
            Assert.NotEqual(1, userid);
            // Act
            var response = await _client.GetAsync("/api/Users/"+userid);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal("{\"id\":" + userid + ",\"email\":\"test\",\"name\":\"test\"}", responseString);
        }

        [Fact, TestPriority(7)]
        public async Task GetUserNotFound()
        {
           
            // Act
            var response = await _client.GetAsync("/api/Users/-2");
            //response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            //Assert.Equal("{\"id\":0\",\"email\":\"test\",\"name\":\"test\"}", responseString);
        }
        [Fact, TestPriority(5)]
        public async Task DeleteUser()
        {
            var maxresponse = await _client.GetAsync("/api/Users/-1");
            maxresponse.EnsureSuccessStatusCode();
            var maxresponseString = await maxresponse.Content.ReadAsStringAsync();
            User maxuser = JsonConvert.DeserializeObject<User>(maxresponseString);

            long userid = maxuser.id;
            Assert.NotEqual(1, userid);
            // Act
            var response = await _client.DeleteAsync("/api/Users/" + userid);
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            User deserializeduser = JsonConvert.DeserializeObject<User>(responseString);
            // Assert
            Assert.Equal("{\"id\":" + deserializeduser.id + ",\"email\":\"test\",\"name\":\"test\"}", responseString);
        }

        [Fact, TestPriority(8)]
        public async Task DeleteUserNull()
        {
          
            // Act
            var response = await _client.DeleteAsync("/api/Users/-1");
            //response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();
            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact, TestPriority(9)]
        public async Task TestUserExists()
        {



            // Act
             UsersController usersController = new UsersController(new DotnetCoreSampleContext(CreateOptions()));
           // var response = await _client.DeleteAsync("/ErrorCheck");
           
            var result = usersController.UserExists(-1);
            Assert.False(result);
            //var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
           // Assert.Equal("Error", redirectToActionResult.ActionName);

            //response.EnsureSuccessStatusCode();
            //var responseString = await response.Content.ReadAsStringAsync();
            // Assert
          //  Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        private DbContextOptions<DotnetCoreSampleContext> CreateOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<DotnetCoreSampleContext>();
           
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DotnetCoreSampleContextTest"));

            return optionsBuilder.Options;
        }
    }
}

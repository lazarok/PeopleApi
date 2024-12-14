using Testcontainers.MySql;

namespace People.IntegrationTests.Infrastructure;

public class MySqlContainerFixture : IAsyncLifetime
{
    public const string FixtureName = "MySqlContainerFixture";
    public MySqlContainer MySqlContainer { get; private set; }
    public string ConnectionString => MySqlContainer.GetConnectionString();

    public MySqlContainerFixture()
    {
        MySqlContainer = new MySqlBuilder()
            .WithImage("mysql:8")
            .WithUsername("root")
            .WithPassword("P@ssw0rd!")
            .WithDatabase("people-paycash-integration-test")
            .WithPortBinding(hostPort: 53882, containerPort: 3306)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await MySqlContainer.StartAsync();
    }

    public async Task DisposeAsync()
    {
        await MySqlContainer.StopAsync();
    }
}


[CollectionDefinition(MySqlContainerFixture.FixtureName)]
public class DatabaseCollection : ICollectionFixture<MySqlContainerFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
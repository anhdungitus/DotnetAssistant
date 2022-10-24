using DotNetAssistant.Data;
using DotNetAssistant.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAssistant.Test;

public class Tests
{
    private IRepository<Customer>? _customerRepository;

    [SetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=dotnet.assistant.api;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;"));
        services.AddTransient<IRepository<Customer>, EntityRepository<Customer>>();
        var serviceProvider = services.BuildServiceProvider();
        _customerRepository = serviceProvider.GetService<IRepository<Customer>>();
    }

    [Test]
    public async Task Can_Init_Repository()
    {
        var data = await _customerRepository?.GetAllPagedAsync(async query =>
        {
            query = query.Where(s => s.Id > 0);
            return query;
        }, 2, 5);

        data.Should().HaveCountGreaterThan(0);
    }
}
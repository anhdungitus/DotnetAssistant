using DotNetAssistant.Core.Caching;
using DotNetAssistant.Data;
using DotNetAssistant.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetAssistant.Test;

public class Tests
{
    private IRepository<Customer>? _customerRepository;
    private IRepository<Question>? _questionRepository;

    [OneTimeSetUp]
    public void Setup()
    {
        var services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=dotnet.assistant.api;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;"));
        services.AddScoped<IRepository<Customer>, EntityRepository<Customer>>();
        services.AddScoped<IRepository<Question>, EntityRepository<Question>>();
        
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        services.AddSingleton<IMemoryCache>(memoryCache);
        services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
        
        var serviceProvider = services.BuildServiceProvider();
        _customerRepository = serviceProvider.GetService<IRepository<Customer>>();
        _questionRepository = serviceProvider.GetService<IRepository<Question>>();
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

    [Test]
    public async Task GenerateQuestionBulkFileCsv()
    {
        var sqlStatements = new List<string>();
        for (var i = 0; i < 1000000; i++)
        {
            sqlStatements.Add($"{i},This is a question {i},2022-9-9");
        }
        
        await File.WriteAllLinesAsync(@"D:\bulkInsetQuestion.csv", sqlStatements);
    }

    [Test]
    public async Task GetQuestionData()
    {
        var data = await _questionRepository?.GetAllPagedAsync(questions => questions, 10, 100)!;
        data.Count.Should().Be(100);
    }
    
    [Test]
    public async Task RepositoryGetById()
    {
        var data = await _questionRepository?.GetByIdAsync(1)!;
        var data2 = await _questionRepository?.GetByIdAsync(1, _ => default)!;
        var data3 = await _questionRepository?.GetByIdAsync(1, _ => default)!;
        data.Should().NotBeNull();
        data2.Id.Should().Be(data3.Id);
    }
    
    [Test]
    public async Task RepositoryUpdate()
    {
        var data = await _questionRepository?.GetByIdAsync(1, _ => default)!;
        data.Should().NotBeNull();

        var updatedText = Guid.NewGuid().ToString();
        data.Text = updatedText;
        await _questionRepository.UpdateAsync(data);

        var updatedData = await _questionRepository?.GetByIdAsync(1, _ => default)!;
        updatedData.Text.Should().Be(updatedText);
    }
    
    [Test]
    public async Task RepositoryInsert()
    {
        var data = new Question
        {
            Text = Guid.NewGuid().ToString(),
            CreatedOnUtc = DateTime.UtcNow
        };
        await _questionRepository?.InsertAsync(data)!;
        var list = await _questionRepository.GetByIdAsync(data.Id);
    }
}
namespace DotNetAssistant.Test;

public class TypeFundamental
{

    [OneTimeSetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task TestCreateNewObject()
    {
        var de = new DerivedClass();
        var typeAttributes = System.Attribute.GetCustomAttributes(de.GetType());
    }
}

public class AuthorAttribute : System.Attribute  
{  
    private string name;  
    public double version;  
  
    public AuthorAttribute(string name)  
    {  
        this.name = name;  
        version = 1.0;  
    }  
}  


public class BaseClass
{
    public string BaseType { get; set; }

    public BaseClass(string s)
    {
        BaseType = s;
    }
}

[Author("x")]
public class DerivedClass : BaseClass
{
    public string DerivedType { get; set; }

    public DerivedClass() : base("s")
    {
        DerivedType = Guid.NewGuid().ToString();
        BaseType = DerivedType;
    }
}
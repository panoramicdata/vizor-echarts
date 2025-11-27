# xUnit Quick Reference for Playwright Tests

## Test Attributes

```csharp
// Single test
[Fact]
public async Task MyTest() { }

// Parameterized test
[Theory]
[MemberData(nameof(TestData))]
public async Task MyParameterizedTest(string param1, int param2) { }

// Test data provider
public static IEnumerable<object[]> TestData => new List<object[]>
{
    new object[] { "value1", 42 },
    new object[] { "value2", 100 }
};
```

## Assertions

```csharp
// Boolean
Assert.True(condition);
Assert.True(condition, "Custom message");
Assert.False(condition);

// Equality
Assert.Equal(expected, actual);
Assert.NotEqual(expected, actual);

// Null checks
Assert.Null(obj);
Assert.NotNull(obj);

// Collections
Assert.Empty(collection);
Assert.NotEmpty(collection);
Assert.Contains(item, collection);

// Exceptions
await Assert.ThrowsAsync<Exception>(() => MethodThatThrows());

// Ranges
Assert.InRange(actual, low, high);
```

## Lifecycle

```csharp
public class MyTests : IAsyncLifetime
{
    // Runs before each test
    public async Task InitializeAsync()
    {
        // Setup code
    }

    // Runs after each test
    public async Task DisposeAsync()
    {
        // Cleanup code
    }
}
```

## Common Patterns

### Basic Test
```csharp
[Fact]
public async Task TestName()
{
    // Arrange
    var url = "http://localhost:5185/chart";
    
    // Act
    await Page.GotoAsync(url);
    await WaitForChartAsync();
    
    // Assert
    var isVisible = await IsChartVisibleAsync();
    Assert.True(isVisible);
}
```

### Theory Test
```csharp
public static IEnumerable<object[]> Charts => new List<object[]>
{
    new object[] { "pie", "simple" },
    new object[] { "bar", "stacked" }
};

[Theory]
[MemberData(nameof(Charts))]
public async Task Chart_Renders(string category, string route)
{
    await Page.GotoAsync($"{BaseUrl}/{category}/{route}");
    await WaitForChartAsync();
    
    var isVisible = await IsChartVisibleAsync();
    Assert.True(isVisible);
}
```

## Running Tests

```powershell
# All tests
dotnet test

# Specific class
dotnet test --filter "FullyQualifiedName~ChartTests"

# Specific test
dotnet test --filter "SimplePieChart"

# With detailed output
dotnet test --logger "console;verbosity=detailed"

# List all tests
dotnet test --list-tests
```

## Migration from NUnit

| NUnit | xUnit |
|-------|-------|
| [Test] | [Fact] |
| [TestCase(1, 2)] | [Theory] + [InlineData(1, 2)] |
| [TestCaseSource] | [Theory] + [MemberData] |
| [TestFixture] | (not needed) |
| [SetUp] | IAsyncLifetime.InitializeAsync() |
| [TearDown] | IAsyncLifetime.DisposeAsync() |
| Assert.That(x, Is.True) | Assert.True(x) |
| Assert.That(x, Is.EqualTo(y)) | Assert.Equal(y, x) |
| Assert.That(x, Is.Empty) | Assert.Empty(x) |
| TestContext.WriteLine() | ITestOutputHelper.WriteLine() |
| [Parallelizable] | (enabled by default) |

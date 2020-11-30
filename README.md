# FuzzDotNet

[![Build](https://img.shields.io/github/workflow/status/pensono/FuzzDotNet/Build/master?label=Build)](https://github.com/pensono/FuzzDotNet/actions?query=workflow%3ABuild+branch%3Amaster) [![MSTest](https://img.shields.io/nuget/v/FuzzDotNet.MSTest.svg?label=MSTest&logo=nuget)](https://www.nuget.org/packages/FuzzDotNet.MSTest) [![xunit](https://img.shields.io/nuget/v/FuzzDotNet.Xunit.svg?label=xunit&logo=nuget)](https://www.nuget.org/packages/FuzzDotNet.Xunit)

FuzzDotNet is a library enabling fuzz-testing and the generation synthetic application data.

[Getting Started](./docs/Getting%20Started.md)

## Fuzz Testing by Contract

Contracts are useful tools for building software. he author of a CRUD service may want to test that after creating a resource, an identical model can be read back. To test this contract, the author may write a test like this:

```csharp
[TestMethod]
[DataSource(/* ... */)]
public void TestCreateRead()
{
    api.Create(modelId, model);

    var read = api.Read(modelId);
    Assert.AreEqual(model, read);
}
```

This test has a few limitations. First, it only tests a single case. This can be mitigated by making a data-driven test, but then the single case only becomes a handful. Secondly, it is likely not to exercise enough corner cases. Does it test unicode characters? What about an input which is too long? How about one with a SQL injection? Finally, it is difficult to maintain, any changes to the model also require updating the test cases to match.

Instead of writing a test case for each of these examples, the developer can dramatically increase their coverage by running a fuzzed test several hundred times. This will automatically find counter examples which can be incorporated into the test suite.

```csharp
[FuzzTestMethod]
[DataSource(/* ... */)]
public void TestCreateRead(ModelId modelId, Model model)
{
    api.Create(modelId, model);

    var read = api.Read(modelId);
    Assert.AreEqual(model, read);
}
```

Note that fuzzing is not guaranteed to find all failing test cases, but it will report any failures to be permanently included into the test suite.

## Building Test Cases

Sometimes your data model will be complicated enough that the default generators FuzzDotNet won't be able to create a useful instances. The best way to address this is to write a generator.

```csharp
public class TreeSetGenerator : Generator
{
    public override bool CanGenerate(Type type) => type == typeof(TreeSet<int>);

    public override object? Generate(IFuzzProfile profile, Type type, FuzzRandom random)
    {
        // Create a reasonably sized tree-set of dubious values
        var result = new TreeSet<int>();
        var elementGenerator = new NaughtyIntGenerator();

        var size = random.Uniform(0, 20);
        for (var i = 0; i < length; i++)
        {
            result.Add(elementGenerator.Generate(profile, type, random));
        }

        return result;
    }
}
```

This generator can then be used in several different ways.

To a specific parameter:

```csharp
[FuzzTestMethod]
public void TestAddContains([TreeSetGenerator] TreeSet<int> set, int element) 
{
    list.Add(element);
    Assert.IsTrue(list.Contains(element));
}
```

TODO: Example for a specific class, and to add it to the profile to make it the default for a project.

**Note:** As the complexity of your model grows, the distribution of values fuzzed over has a [great impact][1] on the technique's overall effectiveness. Carefully consider the values generated for best results.

[1]: http://homepages.inf.ed.ac.uk/hleather/publications/2018_deepfuzzing_issta.pdf

## Inductive Testing

TODO: Show how to use fuzzing to test inductive invariants

## Fuzz Testing Methodology

Formally, fuzz testing can be described as follows:

1. Identify a set of [universally-quantified](https://en.wikipedia.org/wiki/Universal_quantification) specifications
1. Translate the specification into a data driven test
1. Add test cases which cover basic functionality
1. Run the test with a fuzzer to identify new test cases.
1. Simplify the failed cases and add them to the data-driven test

## Contributing

We love contributions! See [CONTRIBUTING.md](./CONTRIBUTING.md) for more info.
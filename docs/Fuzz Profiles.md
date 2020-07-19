# Fuzz Profiles

Configuring custom fuzz profiles is not necessary to use FuzzDotNet. Regardless, most projects will opt to configure them for a couple reasons.

- Generators for specific types can be created. This is useful for complex, domain driven types where the default generators don't cut it.
- Formatting can be configured. For example, if your project uses json based data driven tests the `JsonFormatter` will be useful.
- Notification can be configured. This allows integration into your development automation tools. Send emails or automatically create bugs for unique test failures.

Additionally, many projects may use different profiles to perform different tasks. For example, a project may have one for unit testing, one to generate synthetic loads, one to generate API examples, and one to run continuously in a CI system.

## Defining Fuzz Profiles

### Generators

### Notifiers

## Using Fuzz Profiles

The easiest way to integrate fuzz profiles is to subclass `FuzzTestMethodAttribute`.

```csharp
        private class CustomFuzzTestMethodAttribute : FuzzTestMethodAttribute
        {
            protected override IFuzzProfile CreateFuzzProfile()
            {
                return new CustomFuzzProfile();
            }
        }
```

Then, you may use it as follows.

```csharp
        [CustomFuzzTestMethod]
        public void FuzzTest(int value) {
            // ...
        }
```

## Per-method Fuzz Profiles

You may set a fuzz profile on a per-method basis. This overrides any fuzz profile set by overriding `FuzzTestMethodAttribute.CreateFuzzProfile`.

```csharp
        [FuzzTestMethod(FuzzProfileType = typeof(CustomFuzzProfile)]
        public void FuzzTest(int value) {
            // ...
        }
```
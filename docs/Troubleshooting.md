# Troubleshooting

## Why isn't this generator being used?

**Problem**: A generator should be chosen based on the Fuzz Profile being used, however it is not.

**Suggestion**: Add the generator as an attribute. This will force it to be chosen, and will generate an error message if it cannot generate the given type.
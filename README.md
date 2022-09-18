# unittest
Test helper methods.

Example:
[Theory]
[JsonData("filepath")]
public void Test(TestData testData, Expected expected)
{
// todo
}

Also, class to generate combinations of params, somehow.  Signature(s) TBD...
possibly

[Theory]
[Class(typeof(Generator))] -- Standard attrib - generator to work out combinations

or
[Theory]
[CaseGenerator(typeof(GeneratorConfig))] -- Syntax sugar on above, plus additional optional config (like "exactly these values for this string")

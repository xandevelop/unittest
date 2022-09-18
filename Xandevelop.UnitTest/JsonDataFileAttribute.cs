using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit.Sdk;

namespace Xandevelop.UnitTest
{
    // Modified from https://www.ankursheel.com/blog/load-test-data-json-file-xunit-tests
    //
    // 1. I don't like always having to have a test object with actual and expected and properties I want - I prefer just to have one object sometimes.
    // 2. The type arguments in the attribute are redundant - they can be obtained from the signature of the method with the attrib rather than as extra parameters.
    //    e.g. [JsonData(typeof(X)] public void Foo(X x) - the type X is repeated twice.
    //         You *can* make it work more like: [JsonData] public void Foo(X x) - here, JsonData inspects the signature of Foo to determine type X.

    public class JsonDataFileAttribute : DataAttribute
    {
     
        private readonly string _filePath;

        private readonly string _propertyName;


        public JsonDataFileAttribute(string filePath)
        {
            _filePath = filePath;
        }

        public JsonDataFileAttribute(string filePath, string propertyName) : this(filePath)
        {
            _propertyName = propertyName;
        }

        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            if (testMethod == null)
            {
                throw new ArgumentNullException(nameof(testMethod));
            }

            var path = Path.IsPathRooted(_filePath)
                ? _filePath
                : Path.GetRelativePath(Directory.GetCurrentDirectory(), _filePath);

            if (!File.Exists(path))
            {
                throw new ArgumentException($"Could not find file at path: {path}");
            }

            var parameters = testMethod.GetParameters();
            var t = parameters[0].ParameterType;

            var fileData = File.ReadAllText(_filePath);

            if (string.IsNullOrEmpty(_propertyName)) 
            {
                return GetData(fileData, t);
            }
            else
            {
                // Only use the specified property as the data
                var allData = JObject.Parse(fileData);
                var data = allData[_propertyName]?.ToString();

                return GetData(data, t);
            }
        }

        private IEnumerable<object[]> GetData(string jsonData, Type t)
        {
            var objectList = new List<object[]>();
            var generic = typeof(List<>).MakeGenericType(t);
            if (jsonData != null)
            {
                dynamic datalist = JsonConvert.DeserializeObject(jsonData, generic);

                if (datalist != null)
                {
                    foreach (var data in datalist)
                    {
                        if (data != null)
                        {
                            objectList.Add(new object[] { data });
                        }
                    }
                }
            }

            return objectList;
        }
    }
}

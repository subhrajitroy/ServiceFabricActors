using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Fabric.Interop;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;


namespace Actors.UnitTests
{
    /// <summary>
    /// Summary description for ActorUnitTests
    /// </summary>
    [TestFixture]
    public class ActorUnitTests
    {

        [Test]
        public void ShouldUpdateWithPatch()
        {
            var address = new Address {AddressLine1 = "House 1", ZipCode = "ABCD100"};
            var user = new User {Name = "John Doe", Address = address, MobileNumbers = new List<int> {123} };

            dynamic userPatch = new ExpandoObject();
            userPatch.Address = new ExpandoObject();
            userPatch.Address.AddressLine1 = "Flat 2";
            var patch1 = new Patch {Json = JsonConvert.SerializeObject(userPatch),Path = "$.address.addressLine1"};

            dynamic mobileNumberPatch = new ExpandoObject();
            mobileNumberPatch.MobileNumbers = new List<int> {234};
            var patch2 = new Patch {Json = JsonConvert.SerializeObject(mobileNumberPatch),Path = "$.mobileNumbers"};
            
            user.UpdateFrom(patch1);
            user.UpdateFrom(patch2);

            Assert.That(user.Address.AddressLine1,Is.EqualTo("Flat 2"));
            Assert.That(user.Address.ZipCode,Is.EqualTo("ABCD100"));
//            Assert.That(user.MobileNumbers.Count,Is.EqualTo(0));
            Assert.That(user.MobileNumbers[0],Is.EqualTo(123));
        }
        
    }

    public class User
    {
        public string Name { get; set; }
        public Address Address { get; set; }

        public List<int> MobileNumbers { get; set; }

        public void UpdateFrom(Patch patch)
        {
//            var deserializeObject = JObject.Parse(patch.Json);
//            var currentObjectAsJObject = JObject.FromObject(this);
//            var patchPath = patch.Path;
//            var lastIndexOfPathSeparator = patchPath.LastIndexOf(".", StringComparison.Ordinal);
//            var pathTillParent = patchPath.Substring(0, lastIndexOfPathSeparator);
//            var key = patchPath.Substring(lastIndexOfPathSeparator + 1, patchPath.Length - lastIndexOfPathSeparator);
//            var selectToken = currentObjectAsJObject.SelectToken(patchPath);
//            selectToken[key] = "jOY 2";
            JsonConvert.PopulateObject(patch.Json,this);
        }

    }

    public class Address
    {
        public string AddressLine1 { get; set; }
        public string ZipCode { get; set; }

        public List<int> PhoneNumbers { get; set; }
    }

    public class Patch
    {
        public string Json { get; set; }

        public string Path { get; set; }
    }
}

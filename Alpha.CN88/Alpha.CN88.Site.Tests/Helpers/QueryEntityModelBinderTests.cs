using System;
using System.Web.Mvc;
using Alpha.CN88.Site.Helpers;
using Alpha.CN88.Site.Models;
using Alpha.CN88.Site.Requests.Default;
using NUnit.Framework;

namespace Alpha.CN88.Site.Tests.Helpers
{
    public class QueryEntityModelBinderTests : QueryEntityModelBinder
    {
        protected override object CreateInstanceOfAssignableReportBaseQuery(Type modelType)
        {
            return "CreateInstanceOfAssignableReportBaseQuery";
        }

        protected override object CallBaseCreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            return "CallBaseCreateModel";
        }

        [Test]
        public void CreateModel_ConstructorParamIsAssignableFromBaseQuery_CallCreateInstanceOfAssignableBaseQuery()
        {
            // Arrange
            var problemAccountQuery = typeof(IndexQuery);

            // Act
            var createdInstance = CreateModel(null, null, problemAccountQuery);

            // Assert
            Assert.AreEqual("CreateInstanceOfAssignableReportBaseQuery", createdInstance);
        }

        [Test]
        public void CreateModel_ConstructorParamIsNotAssignableFromReportBaseQuery_CallBaseCreateModel()
        {
            // Arrange
            var userType = typeof(User);

            // Act
            var createdInstance = CreateModel(null, null, userType);

            // Assert
            Assert.AreEqual("CallBaseCreateModel", createdInstance);
        }
    }
}
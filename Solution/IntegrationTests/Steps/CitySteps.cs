using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Domain.Entities;
using Domain.Repositories;
using Domain.UnitOfWork;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using TechTalk.SpecFlow;

namespace IntegrationTests.Steps
{
    [Binding]
    [Scope(Feature = "CityActions")]
    public sealed class CitySteps
    {
        [Given(@"a city with the name ""(.*)"" is found in the database")]
        public void GivenACityWithTheNameIsFoundInTheDatabase(string cityName)
        {
            var container = CommonSteps.FeatureContext.Get<ILifetimeScope>();
            var uow = container.Resolve<IUnitOfWork>();

            var foundCity = uow.GetRepository<ICityReadRepository>()
                .GetAll()
                .FirstOrDefault(x => x.Name.ToLower() == cityName.ToLower());

            if (foundCity == null)
            {
                foundCity = new City()
                {
                    Name = cityName
                };
                uow.BeginTransaction();
                uow.GetRepository<ICityWriteRepository>().Add(foundCity);
                uow.Commit();
            }

            var pathParams = CommonSteps.ScenarioContext.Get<string>("pathParams");

            if (pathParams == string.Empty)
                pathParams = "?id=" + foundCity.CityId;

            CommonSteps.ScenarioContext.Set(pathParams, "pathParams");
        }

        [Then(@"the response should be an array of Cities")]
        public async Task TheResponseShouldBeAnArrayOfCities()
        {
            var response = CommonSteps.ScenarioContext.Get<HttpResponseMessage>();

            var json = await response.Content.ReadAsStringAsync();

            json.Should().BeOfType<IEnumerable<City>>();
        }
    }
}

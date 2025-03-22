using DAL;
using DAL.Venues;
using Entities.Venues;
using Microsoft.Extensions.DependencyInjection;
using TestsCore.Providers;
using VenueApplication.Repository;

namespace VenueTests.DALTests
{
    public class SectionRepositoryTests
    {
        private IServiceProvider serviceProvider
          => ServiceConfigurationProvider.Get<VenueContext>(services => services.AddVenuesRepositories());

        [Fact]
        public async Task GetByVenueId_ExistingVenue_AddedExist()
        {
            var provider = serviceProvider;
            var venueRepository = provider.GetService<IVenueRepository>();
            var sectionRepository = provider.GetService<ISectionRepository>();
            var venue = new Venue { Name = "1" };
            await venueRepository.Create(venue);
            await venueRepository.Save();
            var sections = new List<Section>
            {
                new() { Name = "1", VenueId = venue.Id },
                new() { Name = "2", VenueId = venue.Id },
            };
            await sectionRepository.Create(sections[0]);
            await sectionRepository.Create(sections[1]);
            await sectionRepository.Save();

            var result = await sectionRepository.GetByVenueId(venue.Id);

            Assert.NotNull(result);
            Assert.Equal(sections.Count, result.Count);
            Assert.DoesNotContain(result, s => s.Name != "1" && s.Name != "2");
        }
    }
}

using DAL;
using DAL.Venues;
using DAL.Venues.Repository;
using Microsoft.Extensions.DependencyInjection;
using TestsCore;

namespace VenueTests.DAL
{
    public class SectionRepositoryTests
    {
        [Fact]
        public async Task GetByVenueId_WithExistingVenue_AddedExist()
        {
            var serviceProvider = ServiceConfigurationProvider.Get<VenueContext>(services => services.AddVenuesRepositories());
            var venueRepository = serviceProvider.GetService<IVenueRepository>();
            var sectionRepository = serviceProvider.GetService<ISectionRepository>();
            var venue = new Venue { Name = "1" };
            await venueRepository.Create(venue);
            await venueRepository.Save();
            var sections = new List<Section>
            {
                new() { Name = "1", VenueId = venue.Id },
                new() { Name = "2", VenueId= venue.Id },
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

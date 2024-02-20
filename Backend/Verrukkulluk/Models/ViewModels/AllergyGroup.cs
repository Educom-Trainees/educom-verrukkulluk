namespace Verrukkulluk.ViewModels
{
    public class AllergyGroup {
        public required int Id { get; init; }

        public required int Count { get; init; }

        public required Allergy Allergy { get;  init; }
    }
}
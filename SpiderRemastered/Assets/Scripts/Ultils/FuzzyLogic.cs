namespace SFRemastered.Ultils
{
    // Fuzzy logic can handle uncertainties and imprecisions in decision-making processes
    public static class FuzzyLogic
    {
        public static float CalculateFuzzyValue(float input, float min, float max)
        {
            if (input < min) return 0;
            if (input > max) return 1;
            return (input - min) / (max - min);
        }
    }
}
namespace ReactiveMedia
{
    public enum Tendencies
    {
        Neutral,
        Spy,
        Terrorist,
        //Resistance,
        //Oneirica,
        Auteur
    }

    public enum DebugTendencies
    {
        DEBUG_T1,
        DEBUG_T2,
        DEBUG_T3,
        DEBUG_T4
    }

    public enum Locales
    {
        WhitespaceLab,
        Apartment,
        Cafe,
        Gallery,
        Church,
        Underpass,
        Bunker,
        Datacentre,
        Denouement,
    }

    public enum DebugLocales
    {
        DEBUG_Zone0,
        DEBUG_Zone1,
        DEBUG_Zone2,
        DEBUG_Zone3,
        DEBUG_Zone4
    }

    public enum RequestType
    {
        Global,
        Locale
    }

    public enum OldLoaderMode // this needs to be replaced / removed in affected scripts
    {
        Tendency,
        Preset,
        Random
    }

    public enum TendencyAlgorithm
    {
        MaxValue, // winning tendency by largest attention value
        MinValue,
        //FirstPastThePost, // winning tendency past a threshold
        Proportional, // proportion of decision/placement points given to winning tendency
        InverseProportion, // i.e. load an "opposite" tendency as specified as the larger
        CompetitorDistribution, // distribute the tendency total amongst all others than the winning tendency
        Preset,
        Random
    }

    public struct AttnDataStruct
    {
        public string name;
        public Tendencies tendency;
        public Locales locale;
        public double attentionRating;
    }

    // below would be potential alt impl for using with reactivemediasettings SO.
    public struct AttnDataStructAlt
    {
        public string name;
        public string tendency;
        public string locale;
        public double attentionRating;
    }
}
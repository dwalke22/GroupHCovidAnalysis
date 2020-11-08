namespace Covid19Analysis.StateResources
{
    internal static class States
    {
        public static string[] StatesArray()
        {
            var states = "AL,AK,AZ,AR,CA,CO,CT,DE,FL,GA,HI,ID,IL,IN,IA,KS,KY,LA,ME,MD,MA,MI,MN,MS,MO,MT,NE,NV,NH,NJ,NM,NY,NC,ND,OH,OK,OR,PA,RI,SC,SD,TN,TX,UT,VT,VA,WA,WV,WI,WY,AS,DC,FM,GU,MH,MP,PW,PR,VI";
            return states.Split(",");
        }
    }
}

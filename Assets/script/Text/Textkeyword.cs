public static class Textkeyword
{
    public static string GetKeywordBasedOnFlags()
    {
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Greeting) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_02clear))
            return "Hint25";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_02clear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_04clear))
            return "Hint26";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_04clear) &&
           !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_05clear))
            return "Hint27";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_05clear) &&
          !FlagManager.Instance.GetFlag(FlagManager.FlagType.Tu_06clear))
            return "Hint28";
        if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.Dondon1kaiwa))
            return "Hint1";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Dondon1kaiwa) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Motosen))
            return "Hint2";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Motosen) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear))
            return "Hint3";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.DialPasswordclear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear))
            return "Hint4";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.IllustPasswordclear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.capsuleclear))
            return "Hint5";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.capsuleclear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear))
            return "Hint6";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ColorPasswordclear) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Akeyget))
            return "Hint7";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Akeyget) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Adooropen))
            return "Hint8";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackAanim) &&
            !FlagManager.Instance.GetFlagByType(Item.Type.BTB))
            return "Hint9";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA) &&
            FlagManager.Instance.GetFlagByType(Item.Type.BTB))
            return "Hint10";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Bstartsceneok) &&
        //   !FlagManager.Instance.GetFlag(FlagManager.FlagType.Funfun1kaiwa))
        //    return "Hint11";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Funfun1kaiwa) &&
    　　　　//　!FlagManager.Instance.GetFlagByType(Item.Type.diacup) ||
  　　　　　 // !FlagManager.Instance.GetFlagByType(Item.Type.starcup) ||
 　　　　　  // !FlagManager.Instance.GetFlagByType(Item.Type.heartcup))
        //    return "Hint12";
        //if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear) &&
        //     FlagManager.Instance.GetFlagByType(Item.Type.diacup) &&
        //     FlagManager.Instance.GetFlagByType(Item.Type.starcup) &&
        //     FlagManager.Instance.GetFlagByType(Item.Type.heartcup))
        //    return "Hint13";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.SmellPasswordclear) &&
        //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.Bluekeyget))
        //    return "Hint14";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Bluekeyget) &&
        //    !FlagManager.Instance.GetFlagByType(Item.Type.BTB) &&
        //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA))
        //    return "Hint15";
        //if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA) &&
        //    FlagManager.Instance.GetFlagByType(Item.Type.BTB) &&
        //    !FlagManager.Instance.GetFlagByType(Item.Type.diacup2) ||
        //    !FlagManager.Instance.GetFlagByType(Item.Type.starcup2) ||
        //    !FlagManager.Instance.GetFlagByType(Item.Type.heartcup2))
        //    return "Hint16";
        //if (!FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackA) &&
        //    FlagManager.Instance.GetFlag(FlagManager.FlagType.burntcup3get) &&
        //    FlagManager.Instance.GetFlagByType(Item.Type.diacup2) &&
        //     FlagManager.Instance.GetFlagByType(Item.Type.starcup2) &&
        //     FlagManager.Instance.GetFlagByType(Item.Type.heartcup2) &&
        //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.ThreePasswordclear))
        //    return "Hint17";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.ThreePasswordclear) &&
        //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.MonitorPasswordclear))
        //    return "Hint18";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MonitorPasswordclear) &&
        //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.Pillaropen) &&
        //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear))
        //    return "Hint19";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.MixPasswordclear) &&
        //    FlagManager.Instance.GetFlag(FlagManager.FlagType.Pillaropen) &&
        //    !FlagManager.Instance.GetFlag(FlagManager.FlagType.potopen) &&
        //    !FlagManager.Instance.GetFlagByType(Item.Type.watercan))
        //    return "Hint20";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.potopen) &&
        //    FlagManager.Instance.GetFlagByType(Item.Type.watercan))
        //    return "Hint21";
        //if (FlagManager.Instance.GetFlag(FlagManager.FlagType.Cstartsceneok) &&
        //   !FlagManager.Instance.GetFlag(FlagManager.FlagType.Yappi1kaiwa))
        //    return "Hint22";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackBanim) &&
            !FlagManager.Instance.GetFlag(FlagManager.FlagType.Leverdown))
            return "Hint23";
        if (FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackB) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.comebackBanim) &&
            FlagManager.Instance.GetFlag(FlagManager.FlagType.Leverdown))
            return "Hint24";



        return null; // 該当するキーワードがない場合
    }
}
